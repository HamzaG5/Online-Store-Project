using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OnlineStoreReadRepository<TEntity> : IOnlineStoreReadRepository<TEntity> where TEntity : class, new()
    {
        protected readonly OnlineStoreContext _onlineStoreContext;

        public OnlineStoreReadRepository(OnlineStoreContext onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            var result = _onlineStoreContext.Set<TEntity>();

            if (result == null)
            {
                throw new Exception($"Couldn't retrieve entities of type: {typeof(TEntity)}.");
            }

            return result;
        }
    }
}
