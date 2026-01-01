using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositries
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IimageManagementSerives imageManagementSerives;

        public ProductRepository(AppDbContext context, IMapper mapper, IimageManagementSerives imageManagementSerives) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementSerives = imageManagementSerives;
        }

        public async Task<bool> AddAsync(AddProductDto productDto)
        {
            if (productDto == null) return false;

            var product = mapper.Map<Product>(productDto);

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var imagePath = await imageManagementSerives.AddImageAsync(productDto.photo, productDto.Name);
            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,
            }).ToList();

            await context.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;


        }

      

        public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null) return false;

            var findproduct = await context.Products
                .Include(x => x.Category)
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == updateProductDto.Id);

            if (findproduct == null) return false;

            // احفظ الاسم القديم قبل ما تعمل mapping
            var oldProductName = findproduct.Name;

            var findPhoto = await context.Photos
                .Where(x => x.ProductId == updateProductDto.Id)
                .ToListAsync();

            // احذف الصور من المجلد باستخدام الاسم القديم
            foreach (var item in findPhoto)
            {
                imageManagementSerives.DeleteImageAsync(item.ImageName, oldProductName);
            }

            context.Photos.RemoveRange(findPhoto);
            await context.SaveChangesAsync();

            // دلوقتي عدل البيانات
            mapper.Map(updateProductDto, findproduct);

            var imagePath = await imageManagementSerives.AddImageAsync(updateProductDto.photo, updateProductDto.Name);

            var photo = imagePath.Select(Path => new Photo
            {
                ImageName = Path,
                ProductId = updateProductDto.Id,
            }).ToList();

            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();

            return true;
        }




        public async Task DeleteAsync(Product product)
        {
          var photo = await context.Photos.Where(x=>x.ProductId == product.Id).ToListAsync(); 
            foreach (var item in photo)
            {
              imageManagementSerives.DeleteImageAsync( item.ImageName ,product.Name );
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();   


        }








    }
}





#region Old Update Method
//public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
//{
//    if (updateProductDto == null) return false;

//    var findproduct = await context.Products.Include(x => x.Category).Include(x => x.Photos)
//        .FirstOrDefaultAsync(x => x.Id == updateProductDto.Id);

//    if (findproduct == null) return false;

//    mapper.Map(updateProductDto, findproduct);


//    var findPhoto = await context.Photos.Where(x => x.ProductId == updateProductDto.Id).ToListAsync();

//    foreach (var item in findPhoto)
//    {
//        imageManagementSerives.DeleteImageAsync(item.ImageName);

//    }

//    context.Photos.RemoveRange(findPhoto);
//    await context.SaveChangesAsync();

//    var imagePath = await imageManagementSerives.AddImageAsync(updateProductDto.photo, updateProductDto.Name);

//    var photo = imagePath.Select(Path => new Photo
//    {
//        ImageName = Path,
//        ProductId = updateProductDto.Id,

//    }).ToList();

//    await context.Photos.AddRangeAsync(photo);
//    await context.SaveChangesAsync();
//    return true;


//}

#endregion
