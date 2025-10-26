using Api.GRRInnovations.Memorix.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.GRRInnovations.Memorix.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var result = Result<object>.Fail("Invalid input data.", context.HttpContext.TraceIdentifier);
                result.Data = errors;

                context.Result = new BadRequestObjectResult(result);
            }
        }
    }
}
