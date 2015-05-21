using System.Data.Entity;
using LComplete.Framework.EF;

namespace FileServer.Code
{
    public class FileDbContext:BaseDbContext
    {
        static FileDbContext()
        {
            Database.SetInitializer<FileDbContext>(null);
        }

        public FileDbContext() :base("File")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UploadFile> UploadFiles { get; set; } 
    }
}