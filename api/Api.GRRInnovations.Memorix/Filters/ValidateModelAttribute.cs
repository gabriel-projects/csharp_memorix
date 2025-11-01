using Api.GRRInnovations.Memorix.Application.Wrappers;
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

                var errorMessage = string.Join("; ", errors);
                var result = Result<object>.Fail(Error.Validation(errorMessage), context.HttpContext.TraceIdentifier);

                context.Result = new BadRequestObjectResult(result);
            }
        }
    }
}
