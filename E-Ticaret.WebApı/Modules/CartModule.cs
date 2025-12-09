using E_Ticaret.Application.Features.Cart;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TS.Result;

namespace E_Ticaret.WebApı.Modules;
    public static class CartModule
    {
    public static void RegisterCartRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/baskets").WithTags("Baskets").RequireAuthorization();

        group.MapPost("add", async (ISender sender, [FromBody] AddToCartCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);

            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

        }).Produces<Result<string>>();



        group.MapDelete(string.Empty, async(ISender sender, [FromBody] ClearCartCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();


        group.MapPost("create", async (ISender sender, [FromBody] CreateCartCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

        }).Produces<Result<string>>();

        group.MapPost("remove", async (ISender sender, [FromBody] RemoveCartItemCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();

        group.MapPatch(string.Empty, async (ISender sender, [FromBody] UpdateCartQuantityCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();



        group.MapGet(string.Empty, async(ISender sender,HttpContext context, CancellationToken cancellationToken) =>
        {

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();




            var response = await sender.Send(new CartSummaryQuery(Guid.Parse(userId)), cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<CartSummaryResponse>>();


        group.MapGet("getBasketQuery", async (ISender sender, HttpContext context, CancellationToken cancellationToken) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();



            var response = await sender.Send(new GetCartQueryCommand(Guid.Parse(userId)), cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<GetCartQueryResponse>>();
    }


    }

