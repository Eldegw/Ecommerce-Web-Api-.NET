using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IimageManagementSerives _imageManagementSerives;
        private readonly UserManager<AppUser> userManager;
        private readonly IConnectionMultiplexer redis;
        private readonly SignInManager<AppUser> signInManager;

        private readonly IEmailService emailService;
        private readonly IGenerateToken token;




        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public ICastumerBasketRepository castumerBasket { get; }

        public IAuth auth { get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, IimageManagementSerives imageManagementSerives
           , IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService
            , SignInManager<AppUser> signInManager, IGenerateToken token)

        {
            _context = context;
            _mapper = mapper;
            this.token = token;

            _imageManagementSerives = imageManagementSerives;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            CategoryRepository = new CategoryRepositry(context);
            PhotoRepository = new PhotoRepository(context);
            ProductRepository = new ProductRepository(context, mapper, imageManagementSerives);
            castumerBasket = new CastumerBasketRepository(redis);
            auth = new AuthRepository(userManager, signInManager, emailService , token);
        }

    }

}
