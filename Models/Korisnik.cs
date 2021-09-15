using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPlatforma.Models
{
    public class Korisnik
    {
        public ObjectId id { get; set; }
        public string email { get; set; }
        public string sifra { get; set; }
    }
}