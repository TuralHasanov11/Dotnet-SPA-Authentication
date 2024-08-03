using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Identity;
using WebApp.Models;
using WebApp.Requests;
using WebApp.Responses;

namespace WebApp.Endpoints;

public static class AuthorizationEndpoints
{
    public static void MapAuthorizationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/authorization")
            .WithName("Authorization")
            .WithTags("Authorization")
            .WithOpenApi();

        group.MapGet("roles", RoleList)
            .WithName(nameof(RoleList))
            .Produces<Ok<IEnumerable<ApplicationRoleResponse>>>(StatusCodes.Status200OK)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)
            .Produces<ForbidHttpResult>(StatusCodes.Status403Forbidden)
            .RequireAuthorization(Permissions.RoleView);

        group.MapPost("roles", RoleCreate)
            .WithName(nameof(RoleCreate))
            .Produces<Created>(StatusCodes.Status201Created)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)
            .Produces<ForbidHttpResult>(StatusCodes.Status403Forbidden)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest)
            .RequireAuthorization(Permissions.RoleCreate);
    }

    public static async Task<Results<Ok<IEnumerable<ApplicationRoleResponse>>, UnauthorizedHttpResult, ForbidHttpResult>>RoleList(
        RoleManager<ApplicationRole> roleManager)
    {
        IEnumerable<ApplicationRoleResponse> roles = await roleManager.Roles
            .Select(r => r.ToApplicationRoleResponse())
            .ToListAsync();

        return TypedResults.Ok(roles);
    }

    public static async Task<Results<Created, UnauthorizedHttpResult, ForbidHttpResult, BadRequest>> RoleCreate(
        CreateRoleRequest request,
        RoleManager<ApplicationRole> roleManager)
    {
        var role = new ApplicationRole
        {
            Name = request.Name
        };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Created();
    }
}