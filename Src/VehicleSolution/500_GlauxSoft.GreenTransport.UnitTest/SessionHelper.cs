
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlauxSoft.Business;
using Evidence.Business;

namespace GlauxSoft.GreenTransport.UnitTest
{
    internal enum SessionUser
    {
        DefaultUser,
        DefaultUserFrench,
        //Wenigrechte
    }

    internal struct Login
    {
        internal string Username { get; set; }
        internal string Password { get; set; }
        internal EvdLangId Language { get; set; }

        internal Login(string user, string pass)
            : this()
        {
            Username = user;
            Password = pass;
            //Language = new EvdLangId(Properties.Settings.Default.Language);
        }
        internal Login(string user, string pass, int langID)
            : this(user, pass)
        {
            Language = new EvdLangId(langID);
        }
    }

    /// <summary>
    /// Class to simplify evidence session management in unit tests projects.
    /// </summary>
    public class SessionHelper
    {

        #region Fields

        private static Properties.Settings settings = GlauxSoft.GreenTransport.UnitTest.Properties.Settings.Default;

        // add/adjust logins if required 
        private static Dictionary<SessionUser, Login> _logins = new Dictionary<SessionUser, Login>()
        {
            { SessionUser.DefaultUser, new Login(settings.Username, settings.Password) },
            { SessionUser.DefaultUserFrench, new Login(settings.Username, settings.Password, 12) },
            //{ SessionUser.Wenigrechte, new Login("wenigrechte", "test") }
        };

        private static Dictionary<SessionUser, EvdSession> _sessions = new Dictionary<SessionUser, EvdSession>();
        private static readonly Uri _uri = new Uri(string.Format("tcp://{0}:{1}/{2}", settings.Host, settings.Port, settings.ServiceUri));

        #endregion // Fields


        #region Properties

        public static EvdSession Session
        {
            get { return ConnectSession(SessionUser.DefaultUser); }
        }

        public static Uri Uri { get { return _uri; } }

        #endregion // Properties


        #region Internal Methods

        /// <summary>
        /// Sets up the evidence session with admin privileges
        /// </summary>
        internal static void Connect()
        {
            ConnectSession(SessionUser.DefaultUser);
        }

        /// <summary>
        /// Unregisters session
        /// </summary>
        internal static void Disconnect()
        {
            try
            {
                SessionManager.Unregister();
            }
            catch
            {
                // do nothing
            }
        }

        /// <summary>
        /// Removes objects that were created with ObjectHelper and 
        /// clears session cache.
        /// </summary>
        internal static void TearDownTestEnvironment()
        {
            // delete bo's
            //ObjectHelper.DeleteObjects();

            // logout sessions and clear cache
            foreach (EvdSession session in _sessions.Values)
            {
                try
                {
                    session.Logout();
                }
                catch (Exception)
                {
                    // do nothing
                }
            }

            _sessions.Clear();
        }

        /// <summary>
        /// Activates a session with specific privileges
        /// </summary>
        internal static EvdSession ConnectSession(SessionUser user)
        {
            try
            {
                // create session if it does not exist and add to cached sessions
                if (!_sessions.ContainsKey(user) || _sessions[user].Agent == null)
                    CreateSession(user);

                // get session from cached sessions
                EvdSession sess = _sessions[user];

                // if correct session is not registered
                if (!IsSessionRegistered(sess))
                {
                    try
                    {
                        SessionManager.Unregister(); // clear session in SessionManager
                    }
                    catch (Exception)
                    {
                        //Do Nothing because it does not matter                
                    }
                    SessionManager.Register(sess); // register correct session
                }

                // return session
                return sess;
            }
            catch (Exception ex)
            {
                throw new Exception("Error connecting session", ex);
            }
        }

        #endregion // Internal Methods


        #region Private Methods

        private static void CreateSession(SessionUser user)
        {
            try
            {
                // get login (hardcoded, extend as needed)
                Login login = _logins[user];

                // if there is still a session stored for the user (but broken, e.g. agent is null), remove it from cache
                if (_sessions.ContainsKey(user))
                    _sessions.Remove(user);

                // add new session to cache
                _sessions.Add(user, EvdSession.Logon(login.Username, login.Password, login.Language, Uri));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to create session for user '{0}'", user.ToString()), ex);
            }
        }

        /// <summary>
        /// Checks if the session exists and is registered
        /// </summary>
        private static bool IsSessionRegistered(EvdSession session)
        {
            return (session != null
                && session.Agent != null
                && session.Agent.ServerAlive
                && session.Agent.SessionValid()
                && SessionManager.HasSessionRegistered
                && SessionManager.Session.GetHashCode().Equals(session.GetHashCode()));
        }

        #endregion // Private Methods
    }
}
