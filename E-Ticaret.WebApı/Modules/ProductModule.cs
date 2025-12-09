using E_Ticaret.Application.Features.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace E_Ticaret.WebApı.Modules;
    public static class ProductModule
    {
   public static void RegisterProductRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/products").WithTags("Products").RequireAuthorization();


        group.MapPost(string.Empty, async (ISender sender, [FromBody] CreateProductCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();


        group.MapPut(string.Empty, async (ISender sender, [FromBody] UpdateProductCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();


        group.MapDelete(string.Empty, async ( ISender sender, [FromQuery] Guid id, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new RemoveProductCommand(id), cancellationToken);

            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

        }).Produces<Result<string>>();


        group.MapPost("restore", async (ISender sender, [FromQuery] Guid id, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new RestoreProductCommand(id), cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();

    
    }




}
