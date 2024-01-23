using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Uyeler
    {
        [Key]
        public int UyeID { get; set; }
        public string UyeAdi { get; set; }
        public string UyeSoyadi { get; set; }

        public string UyeTelNo { get; set; }
        public string UyeTcNo { get; set; }  
        public string UyeMaili { get; set; }
        public string UyeOzelNotu { get; set; }
        public int SysAktif { get; set; }
        public DateTime UyeEklenmeTarihi { get; set; }
        public string UyeEkleyenPersonel { get; set; }
        public DateTime UyeGuncellenmeTarihi { get; set; }
        public string UyeGuncelleyenPersonel { get; set; }
    }
}