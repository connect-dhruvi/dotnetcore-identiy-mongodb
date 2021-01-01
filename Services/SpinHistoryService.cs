using MongoDB.Driver;
using slotmachine_api.Models;
using slotmachine_api.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace slotmachine_api.Services
{
    public class SpinHistoryService : ISpinHistoryService
    {
        private readonly IMongoCollection<SpinHistory> _spinHistory;

        public SpinHistoryService(ISlotMachineDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _spinHistory = database.GetCollection<SpinHistory>(settings.SpinHistoryCollection);
        }

        public List<SpinHistory> Get() => _spinHistory.Find(spinHistory => true).ToList();

        public SpinHistory Get(string id) => _spinHistory.Find(spinHistory => spinHistory.Id == id).FirstOrDefault();

        public SpinHistory Create(SpinHistory spinHistory)
        {
            _spinHistory.InsertOne(spinHistory);
            return spinHistory;
        }
    }

    public interface ISpinHistoryService
    {
        List<SpinHistory> Get();

        SpinHistory Get(string id);

        SpinHistory Create(SpinHistory spinHistoryIn);
    }
}
