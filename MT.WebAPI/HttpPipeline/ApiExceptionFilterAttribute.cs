﻿using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using log4net;

namespace MT.WebAPI.HttpPipeline
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiExceptionFilterAttribute));

        public override void OnException(HttpActionExecutedContext context)
        {
            ApiException ae = context.Exception as ApiException ?? new ApiException(HttpStatusCode.InternalServerError, context.Exception.Message);
            context.Response = context.Request.CreateResponse(ae.StatusCode, CreateErrorMessage(ae, context.Exception));
            Log.Error(context.Exception);
        }

        protected virtual ErrorMessage CreateErrorMessage(ApiException ae, Exception e)
        {
            HttpContext context = HttpContext.Current;
            return new ErrorMessage
            {
                Message = ae.Message,
                ErrorCode = (int)ae.StatusCode,
                Exception = (context != null && context.IsDebuggingEnabled) ? CreateErrorException(e) : null
            };
        }

        protected virtual ErrorException CreateErrorException(Exception e)
        {
            if (e == null)
            {
                return null;
            }

            return new ErrorException
            {
                ClassName = e.GetType().Name,
                InnerException = CreateErrorException(e.InnerException),
                Message = e.Message,
                StackTrace = e.StackTrace
            };
        }
    }
}