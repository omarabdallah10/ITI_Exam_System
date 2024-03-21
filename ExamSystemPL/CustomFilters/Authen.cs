using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Numerics;

namespace ExamSystemPL.CustomFilters
{
    public class Authen : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated == false)
            {
                context.Result = new RedirectToActionResult("Create", "Login", null);
            }
        }
    }
}
