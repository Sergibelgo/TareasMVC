using Microsoft.EntityFrameworkCore;
using Tutorial2TareasMVC.Entitys;

namespace Tutorial2TareasMVC.DBContext
{
    public interface IContextDB
    {
        DbSet<Tarea> Tareas { get; set; }
    }
    public class ContextDB : DbContext, IContextDB
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
    }
}
