using AdminApplication.Models;

namespace Project_C.Models.StoreModels
{
    public class SQLStoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;
        public SQLStoreRepository(ApplicationDbContext context)
        {
            this._context = context;
        }


        public Store AddStore(Store store)
        {
            _context.Stores.Add(store);
            _context.SaveChanges();
            return store;
        }

        public void DeleteStore(Guid id)
        {
            Store store = _context.Stores.Find(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores;
        }

        public Store GetStore(Guid id)
        {
            return _context.Stores.Find(id);
        }

        public Store UpdateStore(Store storeChanges)
        {
            var store = _context.Stores.Attach(storeChanges);
            store.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return storeChanges;
        }
    }
}

