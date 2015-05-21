using System.Data.Entity.ModelConfiguration;
using Domain.Model;

namespace DataAccess.Mapping
{
    public class Auth_User_GroupsMap : EntityTypeConfiguration<Auth_User_Groups>
    {
        public Auth_User_GroupsMap()
        {
            // Relationships
            this.HasRequired(t => t.Auth_Group)
                .WithMany(t => t.Auth_User_Groups)
                .HasForeignKey(d => d.GroupId);
            this.HasRequired(t => t.Auth_User)
                .WithMany(t => t.Auth_User_Groups)
                .HasForeignKey(d => d.UserId);

        }
    }
}
