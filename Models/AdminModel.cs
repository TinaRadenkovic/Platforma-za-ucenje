using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPlatforma.Entites;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
//using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace WebPlatforma.Models
{
    public class AdminModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Admin> korisnikCollection;

        public AdminModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
            korisnikCollection = db.GetCollection<Admin>("korisnik");
        }
        public void Update(Admin p)
        {
            korisnikCollection.UpdateOne(
                Builders<Admin>.Filter.Eq("_id", p.id),
                Builders<Admin>.Update
                .Set("email", p.email)
                .Set("sifra", p.sifra)
                .Set("ime", p.ime)
                .Set("prezime", p.prezime)
                .Set("telefon", p.telefon)
                );
        }

        public Admin Find(String email, String sifra)
        {
            return korisnikCollection.AsQueryable<Admin>().SingleOrDefault(a => a.email == email && a.sifra == sifra);
        }
    }
}