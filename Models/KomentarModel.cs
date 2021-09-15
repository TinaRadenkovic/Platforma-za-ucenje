using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPlatforma.Entites;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
using System.Web.UI.WebControls;
using MongoDB.Driver.Linq;

namespace WebPlatforma.Models
{
    public class KomentarModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Komentar> komentarCollection;

        public KomentarModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
            komentarCollection = db.GetCollection<Komentar>("komentar");
        }

        public void Create(Komentar komentar)
        {
            komentarCollection.InsertOne(komentar);
        }

        public void Delete(String id)
        {
            komentarCollection.DeleteOne(Builders<Komentar>.Filter.Eq("_id", new ObjectId(id)));
        }

        public Komentar Find(ObjectId id)
        {
            return komentarCollection.AsQueryable<Komentar>().SingleOrDefault(a => a.id == id);
        }

        public List<Komentar> FindByDocument(String dokument)
        {
            List<Komentar> lista = new List<Komentar>();
           
            var k = komentarCollection.AsQueryable<Komentar>()
                                 .Where(f => f.dokument == dokument).ToArray();
            foreach (Komentar b in k)
            {
                lista.Add(b);
            }

            return lista;
        }

        public List<Komentar> FindByDocumentSortNew(String dokument)
        {
            List<Komentar> lista = new List<Komentar>();

            var k = komentarCollection.AsQueryable<Komentar>()
                                 .Where(f => f.dokument == dokument).OrderByDescending(f => f.vreme);
            foreach (Komentar b in k)
            {
                lista.Add(b);
            }

            return lista;
        }

        public List<Komentar> FindByDocumentSortPopular(String dokument)
        {
            List<Komentar> lista = new List<Komentar>();

            var k = komentarCollection.AsQueryable<Komentar>()
                                 .Where(f => f.dokument == dokument).OrderByDescending(f => f.ocena);
                               
            foreach (Komentar b in k)
            {
                lista.Add(b);
            }

            return lista;
        }

        public void Update(Komentar p)
        {
            komentarCollection.UpdateOne(
                Builders<Komentar>.Filter.Eq("_id", p.id),
                Builders<Komentar>.Update
                .Set("odgovori", p.odgovori)
                .Set("ocena", p.ocena)
                );
        }

        public void FindUgnjezdeni(String ime)
        {
            //var rez = komentarCollection.AsQueryable<Komentar>()
            //    .Where(f => f.odgovori.Contains("ime"));

            // komentarCollection.Find();

            //var query = Query.And(
            //                Query.Not(Query.EQ("ime", "Jelena")),
            //                Query.LT("Plata", 40000)
            //                );

            var filter = Builders<Komentar>.Filter.AnyLte("odgovori.vreme", "10/08/2020 15:10");
            komentarCollection.Find(filter).FirstOrDefault();

        }
    }
}