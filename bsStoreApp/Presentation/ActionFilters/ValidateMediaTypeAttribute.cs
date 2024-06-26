﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Presentation.ActionFilters
{
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent = context.HttpContext
            .Request
            .Headers
            .ContainsKey("Accept");

            if (!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult($"Accept header is missng!");
                return;
            }
            var mediaType = context.HttpContext
            .Request
            .Headers["Accept"]
            .FirstOrDefault();

            //out : outMediaType değişkeninin değeri ancak ve ancak metot içerisinde belirlenir.
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult($"Media type not present. " + $"Please add Accept header with rewuired media type.");
                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }
    }
}
