using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Dto;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Ecom.Api.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
               var product =  await work.ProductRepository
                    .GetAllAsync(x=>x.Category , x=>x.Photos);
                var result = mapper.Map<List<ProductDto>>(product);

                if (product == null)
                {
                    return BadRequest(new ResponseApi(400, "Item had been add"));

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await work.ProductRepository
                    .GetByIdAsync(id,x=>x.Category , x=>x.Photos);

                var result = mapper.Map<ProductDto>(product);


                if (product == null)
                {
                    return BadRequest(new ResponseApi(400, $"Item product id = {id} Not found"));
                }
                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }



        [HttpPost("add-product")]
        public async Task<IActionResult> Add(AddProductDto productDto)
        {
            try
            {
                await work.ProductRepository.AddAsync(productDto);
                return Ok(new ResponseApi(200 , "Item has been Added"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(400 ,ex.Message));
            }
        }


        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            try
            {
                await work.ProductRepository.UpdateAsync(updateProductDto);
                return Ok(new ResponseApi(200));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(400,ex.Message));

            }
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

              var product = await work.ProductRepository.
                  GetByIdAsync(id , x => x.Category , XmlConfigurationExtensions=>XmlConfigurationExtensions.Photos);

                await work.ProductRepository.DeleteAsync(product);

                return Ok(new ResponseApi(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(400, ex.Message));
             
            }

        }





    }
}
