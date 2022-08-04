using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Api.Database;
using Orbit.Application.Api.Dto;
using Orbit.Application.Api.Models;

namespace Orbit.Application.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly OrbitDbContext _orbitDbContext;
        private readonly IMapper _mapper;

        public ProductController(OrbitDbContext orbitDbContext,
            IMapper mapper)
        {
            this._orbitDbContext = orbitDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var products = await _orbitDbContext.Products.AsNoTracking().ToListAsync();

            var productDtos = products.Select(p => _mapper.Map<ProductDto>(p));

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var product = await _orbitDbContext.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productDto);

            await _orbitDbContext.AddAsync(product, cancellationToken);
            await _orbitDbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, _mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDto productDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productDto);

            _orbitDbContext.Products.Update(product);
            await _orbitDbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var product = await _orbitDbContext.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            _orbitDbContext.Products.Remove(product);

            await _orbitDbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}