using FilmsApi.Model;
using FilmsApi.Model.Advertising;
using FilmsApi.Model.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastestDeliveryApi.database
{
    public class EfModel: DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public EfModel(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<Advertising> Advertisings { get; set; }
        public virtual DbSet<Freekassa> Freekassas { get; set; }
    }
}
