using AutoMapper;
using E_Commerce.Data;
using E_Commerce.DTO;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {


        private readonly AppDbContext context;

        private readonly IMapper mapper;

        public ProductsController(AppDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(mapper.Map<IEnumerable<ProductDTO>>(context.Products.ToList()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpGet("{Code}")]
        public IActionResult GetOneByID(string Code)
        {
            Product product = context.Products.Find(Code);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    return Ok(mapper.Map<ProductDTO>(product));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
        }


        [HttpPost]
        public IActionResult Add(ProductDTO productDto)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    context.Products.Add(mapper.Map<Product>(productDto));
                    context.SaveChanges();

                    string URL = Url.Action(nameof(GetOneByID), new { ID = productDto.Code });

                    return Created(URL, productDto);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut("{Code}")]
        public IActionResult Update(ProductDTO productDto , string Code)
        {

            if (ModelState.IsValid)
            {

                Product oldProduct = context.Products.Find(Code);
                if (oldProduct != null)
                {
                    try
                    {
                        productDto.Code = Code;
                        mapper.Map(productDto, oldProduct);
                        context.Products.Update(oldProduct);
                        context.SaveChanges();

                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }

                }
                else
                {
                    return NotFound();
                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("{Code}")]
        public IActionResult Delete(string Code)
        {

            Product product = context.Products.Find(Code);
            if (product != null)
            {
                try
                {
                    context.Products.Remove(product);
                    context.SaveChanges();

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            }
            else
            {
                return NotFound();
            }


        }


    }
}
