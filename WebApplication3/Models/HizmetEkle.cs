using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class HizmetEkle
    {
        public int Id { get; set; }
        public string HizmetAdi { get; set; }
        public double HizmetBedeli { get; set; }
        public int SysAktif { get; set; }
        public DateTime HizmetEklenmeTarihi { get; set; }
        public string HizmetEkleyenPersonel { get; set; }
        public DateTime HizmetGuncellenmeTarihi { get; set; }
        public string HizmetGuncelleyenPersonel { get; set; }
    }
}