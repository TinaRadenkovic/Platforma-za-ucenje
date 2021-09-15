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
    public class PorukaModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Poruka> porukaCollection;

        public PorukaModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
            porukaCollection = db.GetCollection<Poruka>("poruka");
        }
        public List<Poruka> FindAll(string pretraga)
        {
            List<Poruka> lista = new List<Poruka>();

            var k = porukaCollection.AsQueryable<Poruka>().ToArray();

            foreach (Poruka b in k)
            {
                lista.Add(b);
            }

            if (!String.IsNullOrEmpty(pretraga))
            {
                lista = lista.Where(s => s.emailKorisnika.ToLower().Contains(pretraga.ToLower()) || s.naslov.ToLower().Contains(pretraga.ToLower()) || s.sadrzajPoruke.ToLower().Contains(pretraga.ToLower())).ToList();
            }

            return lista;
        }

        public void Create(Poruka poruka)
        {
            porukaCollection.InsertOne(poruka);
        }

        public List<Poruka> FindZahtevBlanketi()
        {
            List<Poruka> lista = new List<Poruka>();

            var k = porukaCollection.AsQueryable<Poruka>()
                                 .Where(f => f.zahtev == "zahtevBlanket").ToArray();
            foreach (Poruka b in k)
            {
                lista.Add(b);
            }

            return lista;
        }
		
		public List<Poruka> FindZahtevPredavanja()
        {
            List<Poruka> lista = new List<Poruka>();

            var k = porukaCollection.AsQueryable<Poruka>()
                                 .Where(f => f.zahtev == "zahtevPredavanje").ToArray();
            foreach (Poruka b in k)
            {
                lista.Add(b);
            }

            return lista;
        }
		
		public List<Poruka> FindZahtevKnjige()
        {
            List<Poruka> lista = new List<Poruka>();

            var k = porukaCollection.AsQueryable<Poruka>()
                                 .Where(f => f.zahtev == "zahtevKnjiga").ToArray();
            foreach (Poruka b in k)
            {
                lista.Add(b);
            }

            return lista;
        }

        public List<Poruka> FindPoruke()
        {
            List<Poruka> lista = new List<Poruka>();

            var k = porukaCollection.AsQueryable<Poruka>().Where(f => f.zahtev == "false").ToArray();
            foreach (Poruka b in k)
            {
                lista.Add(b);
            }

            return lista;
        }

        public void Delete(String id)
        {
            porukaCollection.DeleteOne(Builders<Poruka>.Filter.Eq("_id", new ObjectId(id)));
        }

    }
}
    