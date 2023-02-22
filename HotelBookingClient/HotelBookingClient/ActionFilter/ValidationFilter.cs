using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;


namespace HotelBookingClient.ActionFilter
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method == "POST"
                &&
                !context.ModelState.IsValid)
            {
                if (context.ActionDescriptor.DisplayName.Contains("Create"))
                {
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
            }                               
        }                        
    }
  
   
    public class ModelStateValidatorConvension : IApplicationModelConvention
    {

        public void Apply(ApplicationModel application)
        {

            foreach (var controllerModel in application.Controllers)
            {
                controllerModel.Filters.Add(new ValidationFilter());
            }
        }
    }



}

   
