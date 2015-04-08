// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// A default implementation of <see cref="IClientModelValidatorProvider"/> which providers client validators
    /// for attributes which derive from <see cref="IClientModelValidator"/>.
    /// </summary>
    public class DefaultClientModelValidatorProvider : IClientModelValidatorProvider
    {
        /// <inheritdoc />
        public void GetValidators(ClientValidatorProviderContext context)
        {
            foreach (var attribute in context.ValidatorMetadata.OfType<IClientModelValidator>())
            {
                context.Validators.Add(attribute);
            }
        }
    }
}
