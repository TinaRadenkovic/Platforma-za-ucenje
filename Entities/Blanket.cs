using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Entites
{
    public class Blanket : Dokument
    {
        public Blanket()
        {
            tip = "Blanket";
            ocena = 0.ToString();
        }

        [BsonElement("rok")]
        public string rok { get; set; }

        [BsonElement("godina")]
        public string godina { get; set; }

        [BsonElement("tipBlanketa")]
        public string tipBlanketa { get; set; } //pismeni, usmeni
    }
}