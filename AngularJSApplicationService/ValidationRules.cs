using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using AngularJSDataModels;

namespace AngularJSApplicationService
{
    /// <summary>
    /// Validation Rules
    /// </summary>
    public class ValidationRules
    {
        private Object _businessObject;
        private Boolean _validationStatus { get; set; }
        private List<String> _validationMessage { get; set; }
        private Hashtable _validationErrors;

        public Boolean ValidationStatus { get { return _validationStatus; } }
        public List<String> ValidationMessage { get { return _validationMessage; } }
        public Hashtable ValidationErrors { get { return _validationErrors; } }

        public Object BusinessObject { set { _businessObject = value; } }

        public ValidationRules()
        {
            _validationStatus = true;
            _validationMessage = new List<string>();
            _validationErrors = new Hashtable();
        }

        /// <summary>
        /// Update Messages
        /// </summary>
        /// <param name="validationStatus"></param>
        /// <param name="validationMessages"></param>
        /// <param name="validationErrors"></param>
        public void UpdateMessages(Boolean validationStatus, List<String> validationMessages, Hashtable validationErrors)
        {
            if (validationStatus == false) _validationStatus = false;

            foreach (string validationMessage in validationMessages)
            {
                _validationMessage.Add(validationMessage);
            }

            foreach (DictionaryEntry validationError in validationErrors)
            {
                if (_validationErrors.ContainsKey(validationError.Key) == false)
                {
                    _validationErrors.Add(validationError.Key, validationError.Value);
                }
            }

        }
        
        /// <summary>
        /// Initialize Validation Rules
        /// </summary>
        /// <param name="businessObject"></param>
        public void InitializeValidationRules(Object businessObject)
        {
            _businessObject = businessObject;

            _validationStatus = true;
            _validationMessage = new List<string>();
            _validationErrors = new Hashtable();

        }

        /// <summary>
        /// Validate Required
        /// </summary>
        /// <param name="propertyName"></param>
        public Boolean ValidateRequired(string propertyName)
        {
            return ValidateRequired(propertyName, propertyName);
        }

        /// <summary>
        /// Validate Required
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="friendlyName"></param>
        public Boolean ValidateRequired(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.ValidateRequired(valueOf) == false)
            {
                string errorMessage = friendlyName + " is a required field.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;

        }

        /// <summary>
        /// Validate Guid Required
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="friendlyName"></param>
        public Boolean ValidateGuidRequired(string propertyName, string friendlyName, string displayPropertyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.ValidateRequiredGuid(valueOf) == false)
            {
                string errorMessage = friendlyName + " is a required field.";
                if (displayPropertyName == string.Empty)
                {
                    AddValidationError(propertyName, errorMessage);
                }
                else
                {
                    AddValidationError(displayPropertyName, errorMessage);
                }
                return false;
            }

            return true;

        }


        public void ValidationError(string propertyName, string errorMessage)
        {
            AddValidationError(propertyName, errorMessage);
        }

        /// <summary>
        /// Validate Length
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateLength(string propertyName, int maxLength)
        {
            return ValidateLength(propertyName, propertyName, maxLength);
        }

        /// <summary>
        /// Validate Length
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateLength(string propertyName, string friendlyName, int maxLength)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.ValidateLength(valueOf, maxLength) == false)
            {
                string errorMessage = friendlyName + " exceeds the maximum of " + maxLength + " characters long.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Numeric
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateNumeric(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.IsInteger(valueOf) == false)
            {
                string errorMessage = friendlyName + " is not a valid number.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Greater Than Zero
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateGreaterThanZero(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.ValidateGreaterThanZero(valueOf) == false)
            {
                string errorMessage = friendlyName + " must be greater than zero.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Decimal Greater Than Zero
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateDecimalGreaterThanZero(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.ValidateDecimalGreaterThanZero(valueOf) == false)
            {
                string errorMessage = friendlyName + " must be greater than zero.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }


        public Boolean ValidateDecimalIsNotZero(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.ValidateDecimalIsNotZero(valueOf) == false)
            {
                string errorMessage = friendlyName + " must not equal zero.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Item has a selected value
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateSelectedValue(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.ValidateGreaterThanZero(valueOf) == false)
            {
                string errorMessage = friendlyName + " not selected.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Validate Is Date
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateIsDate(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.IsDate(valueOf) == false)
            {
                string errorMessage = friendlyName + " is not a valid date.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Is Date or Null Date
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateIsDateOrNullDate(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.IsDateOrNullDate(valueOf) == false)
            {
                string errorMessage = friendlyName + " is not a valid date.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Required Date
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateRequiredDate(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.IsDateGreaterThanDefaultDate(valueOf) == false)
            {
                string errorMessage = friendlyName + " is a required field.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Date Greater Than or Equal to Today
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="maxLength"></param>
        public Boolean ValidateDateGreaterThanOrEqualToToday(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            if (Validations.IsDateGreaterThanOrEqualToToday(valueOf) == false)
            {
                string errorMessage = friendlyName + " must be greater than or equal to today.";
                AddValidationError(propertyName, errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Email Address
        /// </summary>
        /// <param name="propertyName"></param>
        public Boolean ValidateEmailAddress(string propertyName)
        {
            return ValidateEmailAddress(propertyName, propertyName);
        }

        /// <summary>
        /// Validate Email Address
        /// </summary>
        /// <param name="propertyName"></param>
        public Boolean ValidateEmailAddress(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            string stringValue;

            if (valueOf == null) return true;

            stringValue = valueOf.ToString();
            if (stringValue == string.Empty) return true;

            if (Validations.ValidateEmailAddress(valueOf.ToString()) == false)
            {
                string emailAddressErrorMessage = friendlyName + " is not a valid email address";
                AddValidationError(propertyName, emailAddressErrorMessage);
                return false;
            }

            return true;
        }

  

        /// <summary>
        /// Validatie URL
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="friendlyName"></param>
        /// <returns></returns>
        public Boolean ValidateURL(string propertyName, string friendlyName)
        {
            object valueOf = GetPropertyValue(propertyName);
            string stringValue;

            if (valueOf == null) return true;

            stringValue = valueOf.ToString();
            if (stringValue == string.Empty) return true;

            if (Validations.ValidateURL(valueOf.ToString()) == false)
            {
                string urlErrorMessage = friendlyName + " is not a valid URL address";
                AddValidationError(propertyName, urlErrorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets value for given business object's property using reflection.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected object GetPropertyValue(string propertyName)
        {
            return _businessObject.GetType().GetProperty(propertyName).GetValue(_businessObject, null);
        }

        /// <summary>
        /// Add Validation Error
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="friendlyName"></param>
        /// <param name="errorMessage"></param>
        public void AddValidationError(string propertyName, string errorMessage)
        {

            if (_validationErrors.Contains(propertyName) == false)
            {
                _validationErrors.Add(propertyName, errorMessage);
                _validationMessage.Add(errorMessage);
            }

            _validationStatus = false;
        }

    }
}
