using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Collections;
using System.Reflection;

namespace AngularJSWebApplication.Helpers
{
   
    public static class ModelStateHelper
    {

        /// <summary>
        /// Return Error Messages
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static List<String> ReturnErrorMessages(IEnumerable errors)
        {

            var r = new System.Text.RegularExpressions.Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);

            List<String> errorMessages = new List<String>();

            foreach (System.Web.Http.ModelBinding.ModelState state in errors)
            {                
                foreach (var error in state.Errors)
                {
                    if (error.Exception != null)
                    {
                        string[] objectProperty = error.Exception.ToString().Split(Convert.ToChar("'"));
                        if (objectProperty.Length > 2)
                        {
                            string errorMessage = r.Replace(objectProperty[1], " ");
                            errorMessages.Add(errorMessage + " is invalid.");
                        }
                        else
                        {
                            errorMessages.Add(error.Exception.ToString());
                        }
                    }
                }

            }
          
            return errorMessages;

        }
        /// <summary>
        /// Return Validation Errors
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Hashtable ReturnValidationErrors(IEnumerable errors)
        {
            Hashtable validationErrors = new Hashtable();

            foreach (System.Web.Http.ModelBinding.ModelState state in errors)
            {
                foreach (var error in state.Errors)
                {
                    if (error.Exception != null)
                    {
                        string[] objectProperty = error.Exception.ToString().Split(Convert.ToChar("'"));
                        if (objectProperty.Length > 2)
                        {
                            if (validationErrors.ContainsKey(objectProperty[1]) == false)
                                validationErrors.Add(objectProperty[1], objectProperty[1]);
                        }
                    }
                }

            }          
            return validationErrors;
        }
        /// <summary>
        /// Errors
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Count() > 0);
            }
            return null;
        }

        /// <summary>
        /// Update View Model
        /// </summary>
        /// <param name="dataTransformationObject"></param>
        /// <param name="businessObject"></param>
        public static void UpdateViewModel(object dataTransformationObject, object businessObject)
        {
            Type targetType = businessObject.GetType();
            Type sourceType = dataTransformationObject.GetType();

            PropertyInfo[] sourceProps = sourceType.GetProperties();
            foreach (var propInfo in sourceProps)
            {              
                //Get the matching property from the target
                PropertyInfo toProp =  (targetType == sourceType) ? propInfo : targetType.GetProperty(propInfo.Name);

                //If it exists and it's writeable
                if (toProp != null && toProp.CanWrite)
                {
                    //Copy the value from the source to the target
                    Object value = propInfo.GetValue(dataTransformationObject, null);
                    toProp.SetValue(businessObject, value, null);
                }
            }


        }

    }

}