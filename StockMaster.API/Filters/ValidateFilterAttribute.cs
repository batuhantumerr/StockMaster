using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StockMaster.Core.DTOs;

namespace StockMaster.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Eğer validasyon hatası varsa (ModelState geçerli değilse)
            if (!context.ModelState.IsValid)
            {
                // Tüm hataları topla
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

                // Bizim standart formatımıza çevir (400 BadRequest)
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors));
            }
        }
    }
}