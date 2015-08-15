using System;
using System.Reflection;
using System.Web.Mvc;

namespace Lte.WebApp.Models
{
    public class OnlyIfPostedFromButtonAttribute : ActionMethodSelectorAttribute
    {
        public String SubmitButton { get; set; }
        public String ViewModelSubmitButton { get; set; }

        public override Boolean IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var buttonName = controllerContext.HttpContext.Request[SubmitButton];
            if (buttonName == null)
            {
                //This is neccessary to support the RemoteAttribute that appears to intercepted the form post 
                //and removes the submit button from the Request (normally detected in the code above) 
                var viewModelSubmitButton = controllerContext.HttpContext.Request[ViewModelSubmitButton];
                if ((viewModelSubmitButton == null) || (viewModelSubmitButton != SubmitButton))
                    return false;
            }

            // Modify the requested action to the name of the method the attribute is attached to 
            controllerContext.RouteData.Values["action"] = methodInfo.Name;
            return true;
        }
    }
}