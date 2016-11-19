using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SMGPA.Models
{
    public partial class SMGPAContext : DbContext
    {
        public SMGPAContext()
            : base("SMGPAConnection")
        {

        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Functionary> Functionary { get; set; }
        public virtual DbSet<Career> Career { get; set; }
        public virtual DbSet<Entities> Entity { get; set; }
        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Process> Process { get; set; }
        public virtual DbSet<Operation> Operation { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Tasks> Task { get; set; }
        public virtual DbSet<Observation> Observation { get; set; }
        public virtual DbSet<Document> Document { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();     
        }
     }
}