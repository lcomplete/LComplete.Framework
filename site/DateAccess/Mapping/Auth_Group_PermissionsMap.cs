using System.Data.Entity.ModelConfiguration;
using Domain.Model;

namespace DataAccess.Mapping
{
    public class Auth_Group_PermissionsMap : EntityTypeConfiguration<Auth_Group_Permissions>
    {
        public Auth_Group_PermissionsMap()
        {
           // Relationships
            this.HasRequired(t => t.Auth_Group)
                .WithMany(t => t.Auth_Group_Permissions)
                .HasForeignKey(d => d.GroupId);

        }
    }
}
