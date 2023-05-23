using Microsoft.EntityFrameworkCore;
using SaritasaT.DTOs.Item;
using SaritasaT.Models;
using SaritasaT.Securities;

namespace SaritasaT.Services
{
    public interface IItemService
    {
        Task<IEnumerable<StorageItem>> GetAll(int storageId);
        Task<StorageItem?> GetById(int id);
        Task<StorageItem> Create(CreateItem item, Storage storage);
        Task<StorageItem> Update(int id,StorageItem item);
        Task Delete(int id);
    }
    public class ItemService:IItemService
    {
        private readonly AppDbContext _context; private readonly IDataProtection _protector;
        public ItemService(AppDbContext context,IDataProtection protector)
        {
            _context = context;
            _protector= protector;
        }
        public async Task<IEnumerable<StorageItem>> GetAll(int storageId)
        {
            return await _context.StorageItems.Where(i=>i.StorageId==storageId).ToListAsync();
        }
        public async Task<StorageItem?> GetById(int id)
        {
            return await GetItem(id);
        }
        public async Task<StorageItem> Create(CreateItem item,Storage storage)
        {
            StorageItem itemStorage = new()
            {
                IsAutoDelEnabled=item.IsAutoDelEnabled,
                StorageId=item.StorageId,
                Text=item.Text,
                Storage=storage,
                FileName=item?.File.FileName??"",
                ShareURL= DateTime.Now.ToString()
            };
            _context.StorageItems.Add(itemStorage);
            await _context.SaveChangesAsync();
            return itemStorage;
        }
        public async Task<StorageItem> Update(int id,StorageItem item)
        {
            var storageItem= await GetItem(id) ?? throw new KeyNotFoundException("File not found");
            storageItem.Text=item.Text;
            storageItem.FileName=item.FileName;
            _context.StorageItems.Update(storageItem);
            await _context.SaveChangesAsync();
            return storageItem;
        }
        public async Task Delete(int id)
        {
            var storageItem = await GetItem(id) ?? throw new KeyNotFoundException("File not found");
            _context.StorageItems.Remove(storageItem);
            await _context.SaveChangesAsync();
        }

        //helper
        private async Task<StorageItem> GetItem(int id)
        {
            var item = await _context.StorageItems.FindAsync(id) ?? throw new KeyNotFoundException("File not found");
            return item;
        }
    }
}
