using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Entites
{
    public class Poruka
    {
        public Poruka()
        {
            vreme = System.DateTime.Now.ToString();
            procitano = "false";
            zahtev = "false";
        }
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("naslov")]
        public string naslov { get; set; }

        [BsonElement("emailKorisnika")]
        public string emailKorisnika { get; set; }

        [BsonElement("sadrzajPoruke")]
        public string sadrzajPoruke { get; set; }

        [BsonElement("vreme")]
        public string vreme { get; set; }

        [BsonElement("procitano")]
        public string procitano { get; set; }

        [BsonElement("ime")]
        public string ime { get; set; }

        [BsonElement("zahtev")]
        public string zahtev { get; set; }

    }
}