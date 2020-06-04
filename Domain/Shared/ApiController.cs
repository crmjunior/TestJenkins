using System.Linq;
using System.Text;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Shared
{
    public class ApiController : ControllerBase
    {
        public ApiController()
        {
            
        }

        protected IActionResult CreateResponse(object result)
        {
            return Response(result, true, null);
        }

        protected IActionResult CreateResponse(object result, bool isValid, string message = null)
        {
            return Response(result, isValid, message);
        }

        protected IActionResult CreateResponse(object result, ValidationResult validation)
        {
            var message = new StringBuilder();
            foreach(var error in validation.Errors)
            {
                message.AppendLine(error.ErrorMessage);
            }
            return Response(result, validation.IsValid, message.ToString());
        }

        protected new IActionResult Response(object result, bool isValid, string message)
        {
            if (isValid)
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = message. ToString()
            });
        }
    }
}