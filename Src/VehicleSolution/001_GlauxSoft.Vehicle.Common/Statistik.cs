using Evidence.Business;
using Evidence.Services;
using GlauxSoft.Business;
using GlauxSoft.Registry;

namespace GlauxSoft.smartLbz.Common.Registry
{
    [RegistryFolder("GreenTransport")]
    public class Statistik : GlobalFolder
    {
        public Statistik() : base(SessionManager.Session.Agent, SessionManager.Session.Directory)
        {

        }

        public Statistik(IAgent agent, IBusinessDirectory businessDirectory) : base(agent, businessDirectory)
        {

        }

        [RegistryKey(Name = "Kanton KBSB Statistik", DefaultValue = "Laufbahnzentrum Stadt Zürich, Kanton Zürich")]
        public string KantonKBSBStatistik
        {
            get { return GetValue(() => KantonKBSBStatistik); }
            set { SetValue(() => KantonKBSBStatistik, value); }
        }

        [RegistryKey(Name = "Erstes Statistik-Jahr", DefaultValue = "3")]
        public int ErstesStatistikJahr
        {
            get { return GetValue(() => ErstesStatistikJahr); }
            set { SetValue(() => ErstesStatistikJahr, value); }
        }

        [RegistryKey(Name = "ProduktNetz2 Report - ObjectID", DefaultValue = "128086904")]
        public int ProduktNetz2
        {
            get { return GetValue(() => ProduktNetz2); }
            set { SetValue(() => ProduktNetz2, value); }
        }

        [RegistryKey(Name = "ProduktStipendien", DefaultValue = "0")]
        public int ProduktStipendien
        {
            get { return GetValue(() => ProduktStipendien); }
            set { SetValue(() => ProduktStipendien, value); }
        }

        [RegistryKey(Name = "ProduktKundencenterAdministration", DefaultValue = "0")]
        public int ProduktKundencenterAdministration
        {
            get { return GetValue(() => ProduktKundencenterAdministration); }
            set { SetValue(() => ProduktKundencenterAdministration, value); }
        }

        [RegistryKey(Name = "ProduktPublikationen", DefaultValue = "0")]
        public int ProduktPublikationen
        {
            get { return GetValue(() => ProduktPublikationen); }
            set { SetValue(() => ProduktPublikationen, value); }
        }

        [RegistryKey(Name = "ProduktIndoKundencenter", DefaultValue = "0")]
        public int ProduktIndoKundencenter
        {
            get { return GetValue(() => ProduktIndoKundencenter); }
            set { SetValue(() => ProduktIndoKundencenter, value); }
        }

        [RegistryKey(Name = "StandardReportPVLehrstellenvermittlung", DefaultValue = "0")]
        public int StandardReportPvLehrstellenvermittlung
        {
            get { return GetValue(() => StandardReportPvLehrstellenvermittlung); }
            set { SetValue(() => StandardReportPvLehrstellenvermittlung, value); }
        }

        [RegistryKey(Name = "LeistungVorstellungsgespraechUeben", DefaultValue = "0")]
        public int LeistungVorstellungsgespraechUeben
        {
            get { return GetValue(() => LeistungVorstellungsgespraechUeben); }
            set { SetValue(() => LeistungVorstellungsgespraechUeben, value); }
        }

        [RegistryKey(Name = "LeistungLehrstellensucheHeute", DefaultValue = "0")]
        public int LeistungLehrstellensucheHeute
        {
            get { return GetValue(() => LeistungLehrstellensucheHeute); }
            set { SetValue(() => LeistungLehrstellensucheHeute, value); }
        }

        [RegistryKey(Name = "LeistungLeveImSchulhaus", DefaultValue = "0")]
        public int LeistungLeveImSchulhaus
        {
            get { return GetValue(() => LeistungLeveImSchulhaus); }
            set { SetValue(() => LeistungLeveImSchulhaus, value); }
        }

        [RegistryKey(Name = "LeistungBewerbungscheck", DefaultValue = "0")]
        public int LeistungBewerbungscheck
        {
            get { return GetValue(() => LeistungBewerbungscheck); }
            set { SetValue(() => LeistungBewerbungscheck, value); }
        }

        [RegistryKey(Name = "LeistungBewerbungswerkstatt", DefaultValue = "0")]
        public int LeistungBewerbungswerkstatt
        {
            get { return GetValue(() => LeistungBewerbungswerkstatt); }
            set { SetValue(() => LeistungBewerbungswerkstatt, value); }
        }
    }
}
