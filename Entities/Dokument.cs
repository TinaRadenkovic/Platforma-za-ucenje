using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Entites
{
    public class Dokument
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("predmet")]
        public string predmet { get; set; }

        [BsonElement("ocena")]
        public string ocena { get; set; }


        [BsonElement("_t")]
        public string tip { get; set; }

        [BsonElement("opis")]
        public string opis { get; set; }
		
		[BsonElement("nazivFajla")]
        public string nazivFajla { get; set; }


   
    }
}