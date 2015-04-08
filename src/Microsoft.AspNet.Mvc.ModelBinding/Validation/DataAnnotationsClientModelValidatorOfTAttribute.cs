// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// A generic implementation of <see cref="DataAnnotationsClientModelValidator"/>.
    /// </summary>
    /// <typeparam name="TAttribute">An attribute which inherits <see cref="ValidationAttribute"/>.</typeparam>
    public abstract class DataAnnotationsClientModelValidator<TAttribute> : IClientModelValidator
        where TAttribute : ValidationAttribute
    {
        /// <summary>
        /// Create a new instance of <see cref="DataAnnotationsClientModelValidator"/>.
        /// </summary>
        /// <param name="attribute">An attribute which inherits <see cref="ValidationAttribute"/>.</param>
        public DataAnnotationsClientModelValidator(TAttribute attribute)
        {
            Attribute = attribute;
        }

        /// <summary>
        /// Gets the <see cref="TAttribute"/> associated with this instance.
        /// </summary>
        public TAttribute Attribute
        {
            get;
        }

        /// <inheritdoc />
        public abstract IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ClientModelValidationContext context);

        /// <summary>
        /// Gets the error message formatted using the <see cref="Attribute"/>.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> associated with the model annotated with
        /// <see cref="Attribute"/>.</param>
        /// <returns>Formatted error string.</returns>
        protected virtual string GetErrorMessage([NotNull] ModelMetadata modelMetadata)
        {
            return Attribute.FormatErrorMessage(modelMetadata.GetDisplayName());
        }
    }
}
