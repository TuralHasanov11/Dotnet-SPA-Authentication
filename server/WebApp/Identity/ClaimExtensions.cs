using System.Security.Claims;
using WebApp.Responses;

namespace WebApp.Identity;

public static class ClaimExtensions
{
    public static ClaimResponse ToClaimResponse(this Claim claim)
    {
        return new ClaimResponse(claim.Type, claim.Value);
    }
}