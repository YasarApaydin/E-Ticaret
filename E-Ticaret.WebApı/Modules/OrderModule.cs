using E_Ticaret.Application.Features.Order;
using E_Ticaret.Application.Features.Users;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TS.Result;

namespace E_Ticaret.WebApı.Modules;
    public static class OrderModule
    {
    public static void RegisterOrderRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/orders").WithTags("Orders").RequireAuthorization();
        group.MapPost("cancel", async (ISender sender, [FromQuery] Guid orderId, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new CancelOrderCommand(orderId), cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();




        group.MapGet("order", async(ISender sender, [FromQuery] Guid orderId, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetOrderDetailQuery(orderId), cancellationToken);
            return response is null
               ? Results.InternalServerError(response)
               : Results.Ok(response);


        }).Produces<GetOrderDetailResponse>();




        group.MapGet("user", async (ISender sender, HttpContext context, CancellationToken cancellationToken) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();


            var response = await sender.Send(new GetOrdersByUserQuery(Guid.Parse(userId)), cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<List<GetUserOrderResponse>>();





        group.MapPut("status", async (ISender sender, [FromBody] UpdateOrderStatusCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);

            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

        }).Produces<Result<string>>();


        group.MapPost("", async (ISender sender, [FromBody] CreateOrderCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<Guid>>();
    }


    }

