using System.Web.Mvc;

namespace MVCProjeKampi.Filters
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                filterContext.Result = new RedirectResult("/Login/Index");
            else
                filterContext.Result = new RedirectResult("/Panel/Index");
        }
    }
}