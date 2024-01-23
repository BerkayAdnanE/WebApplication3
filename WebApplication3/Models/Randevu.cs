using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public int UyeID { get; set; }  
        public DateTime RandevuBaslangicTarihi { get; set; }
        public int RandevuHizmetSuresi { get; set; } // Dakika cinsinden süre
        public string ChckBxSecimleri { get; set; }
        public string NotAciklama { get; set; }
        public int SysAktif { get; set; }
        public DateTime RandevuKayıtTarihi { get; set; }
        public string RandevuKaydedenPersonel { get; set; }
        public DateTime RandevuGuncellenmeTarihi { get; set; }
        public string RandevuGuncelleyenPersonel { get; set; }

    }
}