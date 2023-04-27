using Microsoft.EntityFrameworkCore;

namespace Tutorial2TareasMVC.DBContext
{
    public interface IContextDB
    {

    }
    public class ContextDB : DbContext, IContextDB
    {
        public ContextDB(DbContextOptions options) : base(options)
        {
        }
    }
}
