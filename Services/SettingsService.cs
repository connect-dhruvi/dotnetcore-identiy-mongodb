using MongoDB.Driver;
using slotmachine_api.Models;
using slotmachine_api.Models.Settings;
using System.Collections.Generic;
using System.Linq;

namespace slotmachine_api.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IMongoCollection<Setting> _settings;

        public SettingsService(ISlotMachineDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _settings = (IMongoCollection<Setting>)database.GetCollection<Setting>(settings.SettingsCollection);
        }

        public List<Setting> Get() => _settings.Find(settings => true).ToList();

        public Setting GetByKey(string key) => _settings.Find(settings => settings.Key
            .Equals(key))
            .FirstOrDefault();

        public Setting Get(string id) => _settings.Find(Settings => Settings.Id == id).FirstOrDefault();

        public void Update(string id, Setting settings) =>
            _settings.ReplaceOne(settings => settings.Id == id, settings);
    }

    public interface ISettingsService
    {
        List<Setting> Get();

        Setting GetByKey(string key);

        Setting Get(string id);

        void Update(string id, Setting SettingsIn);
    }
}
