using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace slotmachine_api.Models.Settings
{
    public class SlotMachineDatabaseSettings : ISlotMachineDatabaseSettings
    {
        public string SpinHistoryCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string SettingsCollection { get; set; }
    }

    public interface ISlotMachineDatabaseSettings
    {
        string SpinHistoryCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string SettingsCollection { get; set; }
    }
}