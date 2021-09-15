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
    public class KnjigaModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Knjiga> knjigaCollection;

        public KnjigaModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
            knjigaCollection = db.GetCollection<Knjiga>("dokument");
        }
        public List<Knjiga> FindAll(string pretraga)
        {
            List<Knjiga> lista = new List<Knjiga>();
            //return blanketCollection.AsQueryable().SingleOrDefault(b => b.tip == "Blanket");
            var k = knjigaCollection.AsQueryable<Knjiga>()
                                 .Where(f => f.tip == "Knjiga").ToArray();
            foreach (Knjiga b in k)
            {
                lista.Add(b);
            }

            if (!String.IsNullOrEmpty(pretraga))
            {
                lista = lista.Where(s => s.predmet.ToLower().Contains(pretraga.ToLower()) || s.autor.ToLower().Contains(pretraga.ToLower()) || s.opis.ToLower().Contains(pretraga.ToLower())).ToList();
            }
            
            return lista;
        }

        public void Create(Knjiga knjiga)
        {
            knjigaCollection.InsertOne(knjiga);
        }

        public void Delete(String id)
        {
            knjigaCollection.DeleteOne(Builders<Knjiga>.Filter.Eq("_id", new ObjectId(id)));
        }

        public Knjiga Find(ObjectId id)
        {
            return knjigaCollection.AsQueryable<Knjiga>().SingleOrDefault(a => a.id == id);
        }

        public void Update(Knjiga p)
        {
            knjigaCollection.UpdateOne(
                Builders<Knjiga>.Filter.Eq("_id", p.id),
                Builders<Knjiga>.Update
                .Set("predmet", p.predmet)
                .Set("autor", p.autor)
                .Set("nazivFajla", p.nazivFajla)
                .Set("opis", p.opis)
                .Set("ocena", p.ocena)
                );
        }
    }
}