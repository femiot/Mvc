// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// A default implementation of <see cref="IClientModelValidator"/>.
    /// </summary>
    public class DataAnnotationsClientModelValidator : IClientModelValidator
    {
        /// <summary>
        /// Creates a new instance of <see cref="DataAnnotationsClientModelValidator"/>.
        /// </summary>
        /// <param name="attribute">The <see cref="ValidationAttribute"/> which is used to get
        /// <see cref="ModelClientValidationRule"/>s.</param>
        public DataAnnotationsClientModelValidator([NotNull] ValidationAttribute attribute)
        {
            Attribute = attribute;
        }

        /// <summary>
        /// The <see cref="ValidationAttribute"/> associated with this instance.
        /// </summary>
        public ValidationAttribute Attribute { get; private set; }

        /// <inheritdoc />
        public virtual IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            [NotNull] ClientModelValidationContext context)
        {
            var customValidator = Attribute as IClientModelValidator;
            if (customValidator != null)
            {
                return customValidator.GetClientValidationRules(context);
            }

            return Enumerable.Empty<ModelClientValidationRule>();
        }

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
