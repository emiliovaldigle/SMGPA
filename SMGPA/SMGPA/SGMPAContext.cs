using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SGMPA.Models
{
    public partial class SGMPAContext : DbContext
    {
        public SGMPAContext()
            : base("SMGPAConnection")
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Functionary> Functionary { get; set; }
        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Process> Process { get; set; }
        public virtual DbSet<Operation> Operation { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}