using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMSTEK.Data;
using GMSTEK.DTO;
using GMSTEK.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GMSTEK.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InvoiceController : Controller
    {
        private IInvoiceRepository _invoiceRepository;
        private IRepository<Item> _itemRepository;
        
        public InvoiceController(IInvoiceRepository invoiceRepository,
                                 IRepository<Item> itemRepository)
        {
            _invoiceRepository = invoiceRepository;
            _itemRepository = itemRepository;
        }
        // GET: api/invoice
        [HttpGet]
        public IEnumerable<Invoice> Get()
        {
            return _invoiceRepository.FindAll();
        }

        // GET api/invoice/5
        [HttpGet("{id}", Name = "invoiceCreated")]
        public IActionResult Get(int id)
        {
            var invoice = _invoiceRepository.GetById(id).FirstOrDefault();

            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }

        // POST api/invoice
        [HttpPost]
        public IActionResult Post([FromBody]InvoiceDTO invoiceDTO)
        {
            if(ModelState.IsValid)
            {
                var invoice = new Invoice
                {
                    Client = invoiceDTO.ClientName,
                    InvoiceDate = DateTime.Now,
                    InvoiceItems = new List<InvoiceItem>()
                };
                
                foreach (var item in invoiceDTO.Items)
                {
                    var itemFind = _itemRepository.FindByCondition(q => q.Code == item.Code).FirstOrDefault();
                    invoice.InvoiceItems.Add(new InvoiceItem
                    {
                        Item = itemFind,
                        Invoice = invoice,
                        Quantity = item.Quantity,
                        UnitValue = item.UnitValue
                    });
                }
                _invoiceRepository.Create(invoice);
                return new CreatedAtRouteResult("invoiceCreated", new { id = invoice.InvoiceId }, invoice);
            }
            return BadRequest(ModelState);
        }

        // PUT api/invoice/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ItemDTO[] items)
        {
            var invoiceItem = _invoiceRepository.GetById(id).FirstOrDefault();

            if(invoiceItem == null)
            {
                return NotFound();
            }

            foreach (var item in items)
            {
                var itemTmp = invoiceItem.InvoiceItems.FirstOrDefault(x => x.Item.Code == item.Code);
                itemTmp.Quantity = item.Quantity;
                itemTmp.UnitValue = item.UnitValue;
            }

            _invoiceRepository.Update(invoiceItem);
            return new CreatedAtRouteResult("invoiceCreated", new { id = invoiceItem.InvoiceId }, invoiceItem);

        }

        // DELETE api/invoice/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var invoice = _invoiceRepository.FindByCondition(x => x.InvoiceId == id).FirstOrDefault();
            if(invoice == null)
            {
                return NotFound();
            }

            _invoiceRepository.Delete(invoice);
            return Ok(invoice);
        }
    }
}
