using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tutorial2TareasMVC.Entitys;

namespace Tutorial2TareasMVC.DBContext
{
    public interface IContextDB
    {
        DbSet<Tarea> Tareas { get; set; }
    }
    public class ContextDB : IdentityDbContext, IContextDB
    {
        public ContextDB(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tarea>().Property(t=>t.Titulo).HasMaxLength(250).IsRequired();
        }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Paso> Pasos { get; set; }
        public DbSet<ArchivoAdjunto> ArchivoAdjuntos { get; set; }
    }
}
