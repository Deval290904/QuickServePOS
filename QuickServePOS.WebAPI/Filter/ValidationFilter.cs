using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuickServePOS.WebAPI.Filter
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                context.Result = new BadRequestObjectResult(new
                {
                    message = errors
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}