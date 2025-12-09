using E_Ticaret.Application.Features.Categories;
using E_Ticaret.Application.Features.Order;
using E_Ticaret.Application.Features.Product;
using E_Ticaret.Application.Features.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace E_Ticaret.Presentation.Controllers
{
    [Route("odata")]
    [ApiController]
    [EnableQuery]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AppODataController(ISender sender):ODataController
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EnableLowerCamelCase();
            builder.EntitySet<GetAllCategoriesQueryResponse>("getCategories");
            builder.EntitySet<GetDeletedCategoryResponse>("getDeletedCategories");
            builder.EntitySet<GetAllOrdersQueryResponse>("getAllOrders");
            builder.EntitySet<GetDeletedProductResponse>("getDeletedProduct");
            builder.EntitySet<GetAllProductQueryResponse>("getProducts");
            return builder.GetEdmModel();
        }




        [HttpGet("getCategories")]
    
        public async Task<IQueryable<GetAllCategoriesQueryResponse>> GetAllCategories(CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetCategoriesQuery(), cancellationToken);
            return response;
        }
        [HttpGet("getDeletedCategories")]
        public async Task<IQueryable<GetDeletedCategoryResponse>> GetDeletedCategories(CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetDeletedCategoriesQuery(), cancellationToken);
            return response;
        }


        [HttpGet("getAllOrders")]
        public async Task<IQueryable<GetAllOrdersQueryResponse>> GetAllOrders(CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetAllOrdersQuery(), cancellationToken);
            return response;
        }


        [HttpGet("getDeletedProduct")]
        public async Task<IQueryable<GetDeletedProductResponse>> GetDeletedProduct(CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetDeletedProductQuery(),cancellationToken);

            return response;
        }

        [HttpGet("getProducts")]
        public async Task<IQueryable<GetAllProductQueryResponse>> GetProducts(CancellationToken cancellationToken){

            var response = await sender.Send(new GetProductsQuery(), cancellationToken);

            return response;

        }




    }
}
