using AdminApplication.Models;

namespace Project_C.Models.StoreModels
{
    public interface IStoreRepository
    {
        Store GetStore(Guid id);
        IEnumerable<Store> GetAllStores();

        Store AddStore(Store store);
        Store UpdateStore(Store store);
        void DeleteStore(Guid id);

    }
}
