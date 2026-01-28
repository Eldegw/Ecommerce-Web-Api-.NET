using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositries
{
    public class CastumerBasketRepository : ICastumerBasketRepository
    {
        private readonly StackExchange.Redis.IDatabase _database;

        public CastumerBasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
       
        
        
        
      
        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
           var result = await _database.StringGetAsync(id);

            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerBasket>(result);
            }
            return null;

        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var _basket = await _database.StringSetAsync(basket.Id , JsonSerializer.Serialize(basket) , TimeSpan.FromDays(3));
            if (_basket)
            {
                return await GetBasketAsync(basket.Id);
            }
            return null;

        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
          return await _database.KeyDeleteAsync(id);
        }

    }
}
