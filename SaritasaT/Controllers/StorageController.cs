using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaritasaT.Models;
using SaritasaT.Services;


namespace SaritasaT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;
        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }
        // GET: api/<StorageController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<StorageController>/5
        [HttpGet("{userId}")]
        public Storage Get(int userId)
        {
            var storage=_storageService.GetByUserId(userId);
            return storage;
        }

        //// POST api/<StorageController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<StorageController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<StorageController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
