using LComplete.Framework.EF;

namespace DataAccess
{
    public class Repository<T> : BaseRepository<MainDbContext, T> where T : class, new()
    {
    }
}
