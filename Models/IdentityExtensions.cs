using System.Security.Claims;
using System.Security.Principal;

namespace Trippin_Website.Models
{
    public static class IdentityExtensions
    {
        public static string GetUsernameCont(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UsernameCont");
            return (claim != null) ? claim.Value : string.Empty;
        }

    }
}