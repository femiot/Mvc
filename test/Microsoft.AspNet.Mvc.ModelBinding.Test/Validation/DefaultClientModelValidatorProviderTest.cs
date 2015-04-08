// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    public class DefaultClientModelValidatorProviderTest
    {
        private readonly IModelMetadataProvider _metadataProvider = TestModelMetadataProvider.CreateDefaultProvider();

        [Fact]
        public void ReturnsAClientModelValidator()
        {
            // Arrange
            var provider = new DefaultClientModelValidatorProvider();
            var metadata = _metadataProvider.GetMetadataForType(typeof(DummyClassWithDummyClientValidationAttribute));

            var providerContext = new ClientValidatorProviderContext(metadata);

            // Act
            provider.GetValidators(providerContext);

            // Assert
            var validator = providerContext.Validators.Single();
            Assert.IsType<DummyClientValidation>(validator);
        }

        [Fact]
        public void DoesNotReturnANonClientModelValidator()
        {
            // Arrange
            var provider = new DefaultClientModelValidatorProvider();
            var metadata = _metadataProvider.GetMetadataForType(typeof(DummyClass));

            var providerContext = new ClientValidatorProviderContext(metadata);

            // Act
            provider.GetValidators(providerContext);

            // Assert
            Assert.Empty(providerContext.Validators);
        }

        private class DummyClientValidation : Attribute, IClientModelValidator
        {
            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
            {
                throw new NotImplementedException();
            }
        }

        private class DummyAttribute : Attribute
        {
        }

        [DummyClientValidation]
        private class DummyClassWithDummyClientValidationAttribute
        {
        }

        [Dummy]
        private class DummyClass
        {
        }
    }
}
