using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Entites
{
    public class Admin
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("sifra")]
        public string sifra { get; set; }

        [BsonElement("ime")]
        public string ime { get; set; }

        [BsonElement("prezime")]
        public string prezime { get; set; }

        [BsonElement("telefon")]
        public string telefon { get; set; }
    }
}