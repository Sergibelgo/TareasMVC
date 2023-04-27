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
        public DbSet<Tarea> Tareas { get; set; }
    }
}
