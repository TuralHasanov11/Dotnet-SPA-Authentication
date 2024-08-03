using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Abstractions;
using WebApp.Identity;
using WebApp.Models;
using WebApp.Requests;
using WebApp.Responses;

namespace WebApp.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/authentication")
            .WithName("Authentication")
            .WithTags("Authentication")
            .WithOpenApi();

        group.MapPost("login", Login)
            .WithName(nameof(Login))
            .Accepts<LoginRequest>("application/json")
            .Produces<Ok<AccessTokenResponse>>(StatusCodes.Status200OK)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized);

        group.MapPost("register", Register)
            .Accepts<LoginRequest>("application/json")
            .WithName(nameof(Register))
            .Produces<Ok>(StatusCodes.Status200OK)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized);

        group.MapGet("logout", Logout)
            .WithName(nameof(Logout))
            .Produces<Ok>(StatusCodes.Status200OK)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized);

        group.MapPost("refresh", Refresh)
            .WithName(nameof(Refresh))
            .Accepts<RefreshTokenRequest>("application/json")
            .Produces<Ok<AccessTokenResponse>>(StatusCodes.Status200OK)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized);

        group.MapGet("user-info", UserInfo)
            .WithName(nameof(UserInfo))
            .Produces<Ok<IEnumerable<ClaimResponse>>>(StatusCodes.Status200OK)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }

    public static async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> Login(
        HttpContext context,
        LoginRequest request,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        var tokenResult = await jwtProvider.GenerateToken(context.User);

        if (tokenResult.IsFailure)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(tokenResult.Value);
    }

    public static async Task<Results<Ok, UnauthorizedHttpResult>> Register(
        RegisterRequest request,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {

        var user = ApplicationUser.Create();

        await userManager.SetUserNameAsync(user, request.Email);
        await userManager.SetEmailAsync(user, request.Email);
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        var visitorRole = await roleManager.FindByNameAsync(ApplicationRoles.Visitor);

        if (visitorRole is not null)
        {
            await userManager.AddToRoleAsync(user, visitorRole.Name!);
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, UnauthorizedHttpResult>> Logout(
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        ClaimsPrincipal principal)
    {
        await signInManager.SignOutAsync();

        await jwtProvider.RevokeToken(principal);

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> Refresh(
        RefreshTokenRequest request,
        IJwtProvider jwtProvider)
    {
        var result = await jwtProvider.RefreshToken(request.AccessToken, request.RefreshToken);

        if (result.IsFailure)
        {
            return TypedResults.Problem(result.Error?.Message, statusCode: StatusCodes.Status401Unauthorized);
        }

        return TypedResults.Ok(result.Value);
    }

    public static async Task<Results<Ok<UserInfo>, UnauthorizedHttpResult>> UserInfo(
        UserManager<ApplicationUser> userManager,
        ClaimsPrincipal claimsPrincipal)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);

        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var permissions = claimsPrincipal.GetPermissions();

        return TypedResults.Ok(user.ToUserInfoResponse(permissions));
    }

}