using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace slotmachine_api.Models
{
    public class Setting
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
     //   [JsonConverter(typeof(ObjectIdConverter))]
        // [BsonSerializer(typeof(TestingObjectTypeSerializer))]

        public string Id { get; set; }
       // [BsonSerializer(typeof(TestingObjectTypeSerializer))]

        public string Key { get; set; }
       // [BsonSerializer(typeof(TestingObjectTypeSerializer))]

        public string Value { get; set; } = "5";

    }
}
