using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Entites
{
    public class Knjiga : Dokument
    {
        public Knjiga()
        {
            tip = "Knjiga";
        }

        [BsonElement("autor")]
        public string autor { get; set; }



    }
}