using System.Security.Claims;

namespace EShop.Filters
{
    public class CustomPrincipal : ClaimsPrincipal
    {
        public CustomIdentity CurrentIdentity { get; set; }
    }
}
