using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMSTEK.Data;
using GMSTEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GMSTEK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private IRepository<Item> _itemRepository;

        public ItemController(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET: api/Item
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return _itemRepository.FindAll();
        }

        
    }
}
