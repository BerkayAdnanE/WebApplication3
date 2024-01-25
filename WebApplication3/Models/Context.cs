using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Context : DbContext
    {
        public Context() : base("baglan")
        {
            Database.SetInitializer<Context>(new CreateDatabaseIfNotExists<Context>());
            Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
        }
        public DbSet<Uyeler> Uyeler { get; set; }
        public DbSet<Randevu> Randevu { get; set; }
        public DbSet<HizmetEkle> HizmetEkle { get; set; }
    }
}