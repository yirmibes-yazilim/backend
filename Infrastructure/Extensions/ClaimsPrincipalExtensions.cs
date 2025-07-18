using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user == null || !(user.Identity?.IsAuthenticated ?? false))
                throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanmamış.");

            var subClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(subClaim))
                throw new UnauthorizedAccessException("sub claim bulunamadı.");

            return int.Parse(subClaim);
        }
    }
}
