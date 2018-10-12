using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalGeobits.ChatApp.web.ErrorsLog
{
    public class  LogFilterExceptions : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null) return;

            File.AppendAllText(actionExecutedContext.HttpContext.Server.MapPath("~/Errors/logfilter.text"), "<EXP>" + Environment.NewLine + actionExecutedContext.Exception.ToString() + Environment.NewLine + "</EXP>" + Environment.NewLine);
        }
    }


}