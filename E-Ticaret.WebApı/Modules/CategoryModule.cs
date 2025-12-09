using E_Ticaret.Application.Features.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace E_Ticaret.WebApı.Modules;
    public static class CategoryModule
    {

    public static void RegisterCategoryRoutes(this  IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/categories").WithTags("Categories").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, [FromBody] CreateCategoryCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
                 }).Produces<Result<string>>();

        group.MapPost("/remove", async(ISender sender, [FromBody] RemoveCategoryCommand request,CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

        }).Produces<Result<string>>();
        group.MapPost("/restoreCategory", async (ISender sender, [FromBody] RestoreCategoryCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();


        group.MapPost("/softDeleteCategory", async (ISender sender, [FromBody] SoftDeleteCategoryCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();


        group.MapPost("/updateCategory", async (ISender sender, [FromBody] UpdateCategoryCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

        }).Produces<Result<string>>();



    }


    }
