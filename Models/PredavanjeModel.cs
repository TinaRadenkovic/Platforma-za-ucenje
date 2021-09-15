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
    public class PredavanjeModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Predavanje> predavanjeCollection;

        public PredavanjeModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
            predavanjeCollection = db.GetCollection<Predavanje>("predavanje");
        }
        public List<Predavanje> FindAll(string pretraga)
        {
            List<Predavanje> lista = new List<Predavanje>();
           
            var k = predavanjeCollection.AsQueryable<Predavanje>();
                                 
            foreach (Predavanje b in k)
            {
                lista.Add(b);
            }

            if (!String.IsNullOrEmpty(pretraga))
            {
                lista = lista.Where(s => s.predmet.ToLower().Contains(pretraga.ToLower()) || s.mesto.ToLower().Contains(pretraga.ToLower())).ToList();
            }

            return lista;
        }

        public void Create(Predavanje predavanje)
        {
            predavanjeCollection.InsertOne(predavanje);
        }

        public void Update(Predavanje p)
        {
            predavanjeCollection.UpdateOne(
                Builders<Predavanje>.Filter.Eq("_id", p.id),
                Builders<Predavanje>.Update
                .Set("predmet", p.predmet)
                .Set("vreme",p.vreme)
                .Set("mesto",p.mesto)
                .Set("predavac",p.predavac)
                .Set("prijavljeniKorisnici", p.prijavljeniKorisnici)
                .Set("ocena",p.ocena)
                );
        }

        public void Delete(String id)
        {
            predavanjeCollection.DeleteOne(Builders<Predavanje>.Filter.Eq("_id", new ObjectId(id)));
        }

        public Predavanje Find(ObjectId id)
        {
            return predavanjeCollection.AsQueryable<Predavanje>().SingleOrDefault(a => a.id == id);
        }
    }
}