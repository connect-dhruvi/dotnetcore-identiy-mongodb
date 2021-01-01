using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace slotmachine_api.Models
{
    public class SpinHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int[] SpinValue { get; set; }

        public string UserId { get; set; }

        public int Points { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}