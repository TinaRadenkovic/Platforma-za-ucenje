using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.Ajax.Utilities;

namespace WebPlatforma.Entites
{
    public class Komentar
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("tekstKomentara")]
        public string tekstKomentara { get; set; }

        [BsonElement("vreme")]
        public string vreme { get; set; }

        [BsonElement("idKorisnika")]
        public string idKorisnika { get; set; }

        [BsonElement("imeKorisnika")]
        public string imeKorisnika { get; set; }

        [BsonElement("dokument")]
        public string dokument { get; set; }

        [BsonElement("ocena")]
        public string ocena { get; set; }

        [BsonElement("negativnaOcena")]
        public string negativnaOcena { get; set; }

        [BsonElement("odgovori")]
        public List<Komentar> odgovori { get; set; }

        public Komentar()
        {
            odgovori = new List<Komentar>();
            ocena = 0.ToString();
            negativnaOcena = 0.ToString();
            vreme = System.DateTime.Now.ToString();
        }
    }
}