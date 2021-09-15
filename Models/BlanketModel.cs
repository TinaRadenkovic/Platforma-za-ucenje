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
    public class BlanketModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Blanket> blanketCollection;

        public BlanketModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
            blanketCollection = db.GetCollection<Blanket>("dokument");
        }


        public List<Blanket> FindAll(string pretraga)
        {
            List<Blanket> lista = new List<Blanket>();
            //return blanketCollection.AsQueryable().SingleOrDefault(b => b.tip == "Blanket");
            var k = blanketCollection.AsQueryable<Blanket>()
                                 .Where(f => f.tip == "Blanket").ToArray();


            foreach (Blanket b in k)
            {
                lista.Add(b);
            }

            if (!String.IsNullOrEmpty(pretraga))
            {
                lista = lista.Where(s => s.predmet.ToLower().Contains(pretraga.ToLower()) || s.rok.ToLower().Contains(pretraga.ToLower()) || s.tipBlanketa.ToLower().Contains(pretraga.ToLower())).ToList();
            }

            return lista;
        }

        public void Create(Blanket blanket)
        {
            blanketCollection.InsertOne(blanket);
        }

        public void Delete(String id)
        {
            blanketCollection.DeleteOne(Builders<Blanket>.Filter.Eq("_id", new ObjectId(id)));
        }

        public Blanket Find(ObjectId id)
        {
            return blanketCollection.AsQueryable<Blanket>().SingleOrDefault(a => a.id == id);
        }

        public void Update(Blanket p)
        {
            blanketCollection.UpdateOne(
                Builders<Blanket>.Filter.Eq("_id", p.id),
                Builders<Blanket>.Update
                .Set("predmet", p.predmet)
                .Set("godina", p.godina)
                .Set("rok", p.rok)
                .Set("nazivFajla", p.nazivFajla)
                .Set("opis", p.opis)
                .Set("ocena", p.ocena)
                .Set("tipBlanketa", p.tipBlanketa)
                );
        }
    }
}