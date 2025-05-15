using System;

namespace AJAX.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext() { }
              public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        { }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
    }
}

   

