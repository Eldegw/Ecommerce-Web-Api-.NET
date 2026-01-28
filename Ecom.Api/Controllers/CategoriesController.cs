using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Dto;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Repositries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Ecom.Api.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var category = await work.CategoryRepository.GetAllAsync();
                if (category == null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await work.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return BadRequest(new ResponseApi(400 , $"Not found category id = {id}"));
                }
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

         
        }
        [HttpPost("Add-Category")]
        public async Task<IActionResult> Add(CategoryDto categoryDto)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDto);
               
                await work.CategoryRepository.AddAsync(category);

                return Ok(new ResponseApi(200 , "Item has been added"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }



        }

        [HttpPut("Update-Category")]
        public async Task<IActionResult> Update(UpdateCategoryDto updateCategoryDto)
        {

            try
            {
                Category categoryFromDB = mapper.Map<Category>(updateCategoryDto);

                await work.CategoryRepository.UpdateAsync(categoryFromDB); 
                return Ok(new ResponseApi(200 , "Item has been Updated"));


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-category")]
        public async Task<IActionResult> Delete(int id )
        {
            try
            {
               await work.CategoryRepository.DeleteAsync(id);
                return Ok(new ResponseApi(200 , "Item has been Deleted"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
