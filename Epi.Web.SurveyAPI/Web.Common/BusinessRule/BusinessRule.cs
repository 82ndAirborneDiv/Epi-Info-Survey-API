﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.SurveyAPI.Web.Common.BusinessObject;

namespace Epi.Web.SurveyAPI.Web.Common.BusinessRule
{
    /// <summary>
    /// Abstract base class for business rules. 
    /// Maintains property name to which rule applies and validation error message.
    /// </summary>
    public abstract class BusinessRule
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName">The property name to which rule applies.</param>
        public BusinessRule(string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = propertyName + " is not valid";
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="propertyName">The property name to which rule applies.</param>
        /// <param name="errorMessage">The error message.</param>
        public BusinessRule(string propertyName, string errorMessage)
            : this(propertyName)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Validation method. To be implemented in derived classes.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        public abstract bool Validate(BusinessObject.BusinessObject businessObject);

        /// <summary>
        /// Gets value for given business object's property using reflection.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected object GetPropertyValue(BusinessObject.BusinessObject businessObject)
        {
            return businessObject.GetType().GetProperty(PropertyName).GetValue(businessObject, null);
        }
    }
}

