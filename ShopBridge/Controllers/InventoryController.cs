using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.DTO;
using ShopBridge.Interfaces;


namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
           if (id == 0)
            {
                return BadRequest("Invalid");
            }
            var response = await _inventoryService.GetProductAsync(id);
            if(response!=null)
                return Ok(response);
            return NotFound("Not Found");
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response =await _inventoryService.GetProductsAsync();
            if (response != null)
                return Ok(response);
            return NotFound("Not Found");
        }
        [HttpPost]
        public async Task<IActionResult> AddProducts([FromForm]ProductDto product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");

            }
            var response = await _inventoryService.AddProductAsync(product);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(id==0)
            {
                return BadRequest("Invalid");
            }
            var response =await _inventoryService.DeleteProductAsync(id);
            if(response)
            {
                return Ok(response);
            }
            return NotFound("Not Found");
        }
    }
}
