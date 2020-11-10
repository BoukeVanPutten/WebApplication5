using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Models;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        // Omdat ik mijn database niet ingericht heb, zijn mijn methods niet voorzien van await keywords
        // het zou netjes asynchronous moeten maar ivm tijd doe ik dus alleen even snel async in de method declaratie...


        private IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public async Task<IEnumerable<Product>> GetAll(SortMethod? method, int? page, int? pageSize)
        {
            SanitisePaginationValues(ref page, ref pageSize);

            var result = _repository.GetAllProducts(method, page.Value, pageSize.Value);
            return result;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Product>> GetByColour([Required] Colour colour, SortMethod? method, int? page, int? pageSize)
        {
            SanitisePaginationValues(ref page, ref pageSize);

            var result = _repository.GetProductsByColour(colour, method, page.Value, pageSize.Value);
            return result;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Product>> GetUsingFilters(/*IEnumerable<filters> filters,*/ SortMethod? sortMethod)
        {
            // Zou een object meegestuuurd kunnen worden in de params
            // hier staan dan de verschillende filters in 
            // deze zullen doorgegeven kunnen worden naar de repository, die dit dan afhandeld

            // var result = _repository.GetWithFilters(filterObject, sortMethod);

            return new List<Product>();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Product>> GetByCode(string productCode)
        {
            var result = _repository.GetProduct(productCode);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _repository.AddProduct(product);

            // 201, met url voor de Get van het gemaakte object, en het object zelf
            return CreatedAtAction(nameof(GetByCode), product.ProductCode, product);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _repository.EditProduct(newProduct);
            // eigenlijk moet dit nog een resultaat krijgen of het geslaagd is of niet
            // Dat zou ik dan moeten doorgeven aan de consumer van de API

            return Ok();
        }

        [HttpDelete("{productCode}")]
        public async Task<IActionResult> Delete(string productCode)
        {
            _repository.DeleteProduct(productCode);

            return Ok();
        }

        private void SanitisePaginationValues(ref int? page, ref int? pageSize)
        {
            if (page == null || page < 0)
            {
                page = 0;
            }

            if (pageSize == null || pageSize < 1)
            {
                pageSize = 3;
            }
        }
    }
}
