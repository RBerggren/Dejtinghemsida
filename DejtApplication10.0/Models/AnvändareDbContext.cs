using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DejtApplication10._0.Models
{
    public class AnvändareDbContext : DbContext
    {
        public DbSet<AnvändareModel> användare { get; set; }

        public AnvändareDbContext() : base("Alla-Användare")
        {


        }
    }
}