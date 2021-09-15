using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Entites
{
    public class Predavanje
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("predmet")]
        public string predmet { get; set; }

        [BsonElement("vreme")]
        public string vreme { get; set; }

        [BsonElement("mesto")]
        public string mesto { get; set; }

        [BsonElement("ocena")]
        public string ocena { get; set; }

        [BsonElement("prijavljeniKorisnici")]
        public String prijavljeniKorisnici { get; set; }

        [BsonElement("predavac")]
        public String predavac { get; set; }

        public Predavanje()
        {
            //prijavljeniKorisnici = new List<String>();
        }
    }
}