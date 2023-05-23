using SaritasaT.Models;

namespace SaritasaT.Services
{
    public interface IStorageService
    {
        IEnumerable<Storage> GetAll();
        Storage GetByUserId(int id);
        Task<Storage> Create(User owner);
        Task<Storage> Update(Storage storage);
        Task Delete(int id);
        Task<Storage> GetById(int? id);
    }
    public class StorageService : IStorageService
    {
        private readonly AppDbContext _context;
        public StorageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Storage> Create(User owner)
        {
            Storage storage = new()
            {
                OwnerID = owner.Id,
                User=owner
            };
            _context.Storages.Add(storage);
            await _context.SaveChangesAsync();
            return storage;
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Storage> GetAll()
        {
            throw new NotImplementedException();
        }

        public Storage GetByUserId(int userId)
        {
            var storage = GetStorage(userId);
            return storage;
        }

        public async Task<Storage> Update(Storage storage)
        {
            throw new NotImplementedException();
        }

        public async Task<Storage> GetById(int? id)
        {
            var storage = await _context.Storages.FindAsync(id);
            return storage;
        }

        //helpers
        private Storage GetStorage(int userId)
        {
            var storage = _context.Storages.FirstOrDefault(s=>s.OwnerID== userId) ?? throw new KeyNotFoundException("Storage not found");
            return storage;
        }
    }
}
