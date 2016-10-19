using System;

using System.Reflection;
using Evidence.Business;
using Evidence.ServerExtensibility;
using Evidence.WinClient.Extensibility;
using GlauxSoft.EvidenceServer;
using System.ComponentModel;
using Evidence.Services;

    using GlauxSoft.Business;
    using System.ComponentModel;

namespace GlauxSoft.GreenTransport.VehicleOrderWatcher
{
    [Serializable]
    public class EvdServerExtension : IEvdServerExtensibility
    {
        #region Fields

        private IEvdServer Server { get; set; }

        #endregion

        #region Constructors

        public EvdServerExtension()
        {
        }

        #endregion

        #region IEvdServerExtensibility

        #region ...Properties

        public string Description
        {
            get
            {
                AssemblyDescriptionAttribute descAttr = null;

                string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                descAttr = (AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0];
                if (descAttr != null)
                    return descAttr.Description + ", Version: " + version;

                return string.Empty;
            }
        }

        #endregion

        #region ...Methods

        public void OnConnection(IEvdServer server)
        {
            this.Server = server;
            ((IEvdServerBeforeEvent)Server.get_ServerEvent(ServerEvents.BeforeSaveObject)).AddEventHandler(OnBeforeSave);
            ((IEvdServerBeforeEvent)Server.get_ServerEvent(ServerEvents.BeforeCreateCopy)).AddEventHandler(OnBeforeCreateCopy);
            ((IEvdServerAfterEvent)Server.get_ServerEvent(ServerEvents.AfterSaveObject)).AddEventHandler(OnAfterSave);
        }

        public void OnDisconnection()
        {
            ((IEvdServerBeforeEvent)Server.get_ServerEvent(ServerEvents.BeforeSaveObject)).RemoveEventHandler(OnBeforeSave);
            ((IEvdServerBeforeEvent)Server.get_ServerEvent(ServerEvents.BeforeCreateCopy)).RemoveEventHandler(OnBeforeCreateCopy);
            ((IEvdServerAfterEvent)Server.get_ServerEvent(ServerEvents.AfterSaveObject)).RemoveEventHandler(OnAfterSave);
        }

        #endregion

        #endregion

        #region Handlers

        public void OnAfterSave(object sender, EventArgs e)
        {
            IAgent agent = null;
            try
            {
                // check input parameters 
                agent = sender as IAgent;
                if (agent == null) throw new NullReferenceException(string.Format("Agent is null in ServerAddIn {0}.", this.Description));
                EvidenceObject obj = ((AfterSaveObjEventArgs)e).SaveObjectArgs.Obj;
                
                agent.WriteLog(string.Format("Saved object with ID {0} of class {1}.", obj.ObjectID, obj.ClassName),
                    Evidence.Diagnostics.ClientEventSourceType.ServerAddIn, Evidence.Diagnostics.EventLogEntryType.Information, null, null, System.Net.Dns.GetHostName(), null, null);
            }
            catch (Exception ex)
            {
                this.HandleException(agent, ex, e);
            }
        }

        public void OnBeforeCreateCopy(object sender, EventArgs e)
        {
            IAgent agent = null;
            try
            {
                // check input parameters 
                agent = sender as IAgent;
                if (agent == null) throw new NullReferenceException(string.Format("Agent is null in ServerAddIn {0}.", this.Description));
                BeforeCreateCopyEventArgs args = (BeforeCreateCopyEventArgs)e;
                args.Cancel = true;
                args.CancelMessage = "Sorry";
            }
            catch (Exception ex)
            {
                this.HandleException(agent, ex, e);
            }
        }

        public void OnBeforeSave(object sender, CancelEventArgs e)
        {
            IAgent agent = null;

            try
            {
                // check input parameters
                agent = sender as IAgent;

                if (agent == null)
                    throw new NullReferenceException(string.Format("Agent is null in ServerAddIn {0}.", this.Description));

                // Note: Use the small business directory on server side, because of the performance of business directory creation in large solutions.
                IBusinessDirectory directory = new SmallBusinessDirectory(agent);

                

            }
            catch (Exception ex)
            {
                this.HandleException(agent, ex, e);
            }
        }

        #endregion

        #region Private methods

        private void HandleException(IAgent agent, Exception ex, EventArgs e)
        {
            // Log message
            string msg = ex.Message;
            // Extract inner exception messages also
            while (ex.InnerException != null)
            {
                msg += Environment.NewLine + ex.InnerException.Message;
                ex = ex.InnerException;
            }

            // is cancelation possible?
            if (typeof(EvdCancelEventArgs).IsInstanceOfType(e))
            {
                EvdCancelEventArgs args = e as EvdCancelEventArgs;
                args.Cancel = true;
                args.CancelMessage = msg;
            }
            else if (typeof(System.ComponentModel.CancelEventArgs).IsInstanceOfType(e))
            {
                System.ComponentModel.CancelEventArgs args = e as System.ComponentModel.CancelEventArgs;
                args.Cancel = true;
            }

            try
            {
                // Write log
                if (agent != null)
                {
                    // Try to write log over agent
                    agent.WriteLog(msg, Evidence.Diagnostics.ClientEventSourceType.ServerAddIn, Evidence.Diagnostics.EventLogEntryType.Error, this.Description, null, null, null, null);
                }
                else if (Server != null)
                {
                    // Try to write log over server instance
                    Server.WriteLog(2, msg, this.Description); // 0=Info, 1=Warning, 2=Error
                }
            }
            catch
            {
                // Last chance... write log to windows eventlog
                // ...check, If source exist, otherwise create them
                if (!System.Diagnostics.EventLog.SourceExists(GlauxSoft.Constants.EVENTLOGSOURCE))
                {
                    System.Diagnostics.EventLog.CreateEventSource(GlauxSoft.Constants.EVENTLOGSOURCE, GlauxSoft.Constants.EVENTLOGSOURCE);
                }
                // ...write log entry
                System.Diagnostics.EventLog.WriteEntry(GlauxSoft.Constants.EVENTLOGSOURCE, msg, System.Diagnostics.EventLogEntryType.Error);
            }

        }

        #endregion
    }
}