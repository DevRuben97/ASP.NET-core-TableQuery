using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TableQuery.Entities;
using TableQuery.Extensions;

namespace Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        AppDbContext _dbContext;

        ILogger<ProductController> _logger;
        public ProductController(AppDbContext dbContext, ILogger<ProductController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }
        [HttpPost]
        [Route("[action]")]
       public async Task<IActionResult> GetValues(PaginationQuery query)
        {
            try
            {
               var result=  await  _dbContext.Products.ToListPagedAsync(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(500);
            }
        }
    }
}