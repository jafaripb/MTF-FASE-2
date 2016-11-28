using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MTF_x.Models;

namespace MTF_x
{
    public class PengadaanContext:DbContext
    {
        public PengadaanContext() : base("PengadaanContext")
        {
            
        }
        public DbSet<Pengadaan> Pengadaan { get; set; }
        public DbSet<JadwalPengadaan> JadwalPengadaan { get; set; }
    }
}