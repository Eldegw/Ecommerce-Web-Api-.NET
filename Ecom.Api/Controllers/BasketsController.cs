using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class BasketsController : BaseController
    {
        public BasketsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {

        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var result = await work.castumerBasket.GetBasketAsync(id);
            if (result == null)
            {
                return Ok(new CustomerBasket());
            }

           return Ok(result);
        }

        [HttpPost("update-basket")]
        public async Task<IActionResult> add(CustomerBasket basket)
        {
            var _basket = await work.castumerBasket.UpdateBasketAsync(basket);
            return Ok(_basket);
        }

        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
          var result = await work.castumerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ResponseApi(200 , "Item has been Deleted"))
                : BadRequest(new ResponseApi(400));
        }


    }
}
