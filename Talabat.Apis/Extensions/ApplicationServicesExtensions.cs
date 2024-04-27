using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Talabat.Apis.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository.Data.Contexts;
using Talabat.Repository;
using System.Linq;
using Talabat.Apis.Errors;
using Talabat.Core.Services;
using Talabat.Service;

namespace Talabat.Apis.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices( this IServiceCollection services)
        {

            services.AddSingleton(typeof(IResponseCacheService),typeof(ResponseCacheService));

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));  
            services .AddScoped(typeof(IUnitOfWork ),typeof(UnitOfWork));   

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count() > 0)
                    .SelectMany(M => M.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();

                    var ErrorResponse = new ApiValidationErorrResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ErrorResponse);
                };
            });

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddScoped(typeof(ITokenServices),typeof(TokenServices));

            services.AddScoped(typeof(IOrderService),typeof(OrderService));

            return services;
        }
    }
}
