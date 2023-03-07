using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Validators
{
    public class PermissionsActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

          var br=  context.HttpContext.RequestServices.GetService<BooksRepository>();

            //i need to check whether the isbn being input is in the table permissions
            //related with the logged in user

            string id = context.HttpContext.User.Claims.ElementAt(0).Value;
            string isbn = context.HttpContext.Request.Form.ElementAt(0).Value;

            if (br.CanUserReserveBook(isbn, id) == false)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
