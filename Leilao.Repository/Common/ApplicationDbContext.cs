using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leilao.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.Leilao> Leilaos { get; set; }
        public DbSet<Models.Item> Items { get; set; }
        public DbSet<Models.Usuario> Usuarios { get; set; }
        public DbSet<Models.ItemUsuario> ItemUsuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
        }
    }
}
