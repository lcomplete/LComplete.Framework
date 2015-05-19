using System.Data.Entity;
using DataAccess.Mapping;
using Domain.Model;
using LComplete.Framework.EF;

namespace DataAccess
{
    public class MainDbContext : BaseDbContext
    {
        public MainDbContext() : base("Main") { }

        static MainDbContext()
        {
            Database.SetInitializer<MainDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new Auth_User_GroupsMap());
            modelBuilder.Configurations.Add(new Auth_Group_PermissionsMap());
        }

        public DbSet<Auth_User> Auth_Users { get; set; }
        public DbSet<Auth_User_Groups> Auth_User_Groups { get; set; }
        public DbSet<Auth_Group> Auth_Groups { get; set; }
        public DbSet<Auth_Group_Permissions> Auth_Group_Permissions { get; set; }


    }
}
