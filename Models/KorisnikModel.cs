using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPlatforma.Entites;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;

namespace WebPlatforma.Models
{
    public class KorisnikModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Blanket> blanketCollection;

        public KorisnikModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
            blanketCollection = db.GetCollection<Blanket>("dokument");
        }
        public Blanket FindAll()
        {
            return blanketCollection.AsQueryable<Blanket>().SingleOrDefault(b => b.rok == "februar");
        }
    }
}