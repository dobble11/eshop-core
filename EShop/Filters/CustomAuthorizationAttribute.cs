using EShop.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace EShop.Filters
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var values = context.HttpContext.Request.Headers["Authorization"];
            if (values.Count == 0)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                var token = Base64Helper.Decode(values[0].Substring(6));
                context.HttpContext.User = new CustomPrincipal { CurrentIdentity = new CustomIdentity { Id = Guid.Parse(token) } };
            }
        }
    }
}
