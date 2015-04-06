// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// A generic implementation of <see cref="DataAnnotationsClientModelValidator"/>.
    /// </summary>
    /// <typeparam name="TAttribute">An attribute which implements <see cref="ValidationAttribute"/>.</typeparam>
    public class DataAnnotationsClientModelValidator<TAttribute> : DataAnnotationsClientModelValidator
        where TAttribute : ValidationAttribute
    {

        /// <summary>
        /// Create a new instance of <see cref="DataAnnotationsClientModelValidator"/>.
        /// </summary>
        /// <param name="attribute">An attribute which implements <see cref="ValidationAttribute"/>.</param>
        public DataAnnotationsClientModelValidator(TAttribute attribute)
            : base(attribute)
        {
        }

        /// <summary>
        /// Gets the <see cref="TAttribute"/> associated with this instance.
        /// </summary>
        protected new TAttribute Attribute
        {
            get { return (TAttribute)base.Attribute; }
        }
    }
}
