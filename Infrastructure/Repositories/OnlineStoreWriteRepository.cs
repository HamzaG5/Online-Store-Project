using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OnlineStoreWriteRepository<TEntity> : IOnlineStoreWriteRepository<TEntity> where TEntity : class, new()
    {

        protected readonly OnlineStoreContext _onlineStoreContext;

        public OnlineStoreWriteRepository(OnlineStoreContext onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            await _onlineStoreContext.AddAsync(entity);
            await _onlineStoreContext.SaveChangesAsync();

            return entity;
        }
    }
}
