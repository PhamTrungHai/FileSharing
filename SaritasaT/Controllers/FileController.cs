using Microsoft.AspNetCore.Mvc;
using SaritasaT.DTOs.Item;
using SaritasaT.Models;
using SaritasaT.Securities;
using SaritasaT.Services;

namespace SaritasaT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IBlobService _blobService;
        private readonly IStorageService _storageService;
        private readonly IDataProtection _protector;
        public FileController(IItemService itemService,IDataProtection protector, IBlobService blobService, IStorageService storageService)
        {
            _itemService = itemService;
            _protector = protector;
            _blobService = blobService;
            _storageService = storageService;
        }

        // GET: api/<ItemController>/GetFiles
        [HttpGet("GetFiles")]
        public async Task<IEnumerable<StorageItem>> GetAll(int userId)
        {
            Storage storage=_storageService.GetByUserId(userId);
            var files= await _itemService.GetAll(storage.Id);
            foreach (var file in files)
            {
                file.ShareURL = _protector.Encode(file.Id.ToString());
            }
            return files;
        }

        // GET api/<ItemController>/encodedid
        [HttpGet("{encodedId}")]
        public async Task<IActionResult> Get(string encodedId)
        {
            int id = Int32.Parse(_protector.Decode(encodedId));
            var result=await _itemService.GetById(id);
            if(result.IsAutoDelEnabled)
            {
                await _itemService.Delete(id);
            }
            return Ok(result);
        }

        [HttpGet("Download/{encodedId}")]
        public async Task<IActionResult> Download(string encodedId)
        {
            int id = Int32.Parse(_protector.Decode(encodedId));
            var item=await _itemService.GetById(id);
            var result=await _blobService.DownloadFile(item.FileName);
            if (item.IsAutoDelEnabled)
            {
                var update = item;
                update.FileName = "";
                await _itemService.Update(id, update);
                await _blobService.DeleteFile(item.FileName);
            }
            return File(result.Content,result.ContentType,result.Name);
        }

        // POST api/<ItemController>
        [HttpPost("Upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm]CreateItem item)
        {
            Storage storage = await _storageService.GetById(item.StorageId);
            var newItem=await _itemService.Create(item,storage);
            string encodedId = _protector.Encode(Convert.ToString(newItem.Id));
            BlobResponse blobResponse;
            if (item.File != null)
            {
                item.FileName = newItem.FileName;
                blobResponse = await _blobService.UploadFile(item.File);
            }
            return CreatedAtAction(nameof(Get), new { encodedId = encodedId }, newItem);
        }

        // PUT api/<ItemController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ItemController>/5
        [HttpDelete("{encodedId}")]
        public async Task Delete(string encodedId)
        {
            int id = Int32.Parse(_protector.Decode(encodedId));
            await _itemService.Delete(id);
        }
    }
}
