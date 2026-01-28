using Ecom.Core.Dto;
using Ecom.Core.Entities.Product;
using Ecom.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
       Task<IEnumerable<ProductDto>> GetAllAsync(ProductParams parameters);
        Task<bool> AddAsync(AddProductDto productDto);
        Task<bool> UpdateAsync(UpdateProductDto updateProductDto);

        Task DeleteAsync(Product product);

       
    }
}
