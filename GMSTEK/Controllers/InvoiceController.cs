﻿using System;
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
        private IRepository<Invoice> _invoiceRepository;
        private IRepository<Item> _itemRepository;
        public InvoiceController(IRepository<Invoice> invoiceRepository,
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
            var invoice = _invoiceRepository.FindByCondition(x => x.InvoiceId == id);

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
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/invoice/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
