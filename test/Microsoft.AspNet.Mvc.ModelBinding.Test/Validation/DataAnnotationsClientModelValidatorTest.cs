// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#if DNX451
using System.Linq;
#endif
using Microsoft.Framework.DependencyInjection;
#if DNX451
using Moq;
using Moq.Protected;
#endif
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    public class DataAnnotationsClientModelValidatorTest
    {
        private static IModelMetadataProvider _metadataProvider = TestModelMetadataProvider.CreateDefaultProvider();

        [Fact]
        public void GetClientValidationRules_ReturnsEmptyRuleSet()
        {
            // Arrange
            var attribute = new FileExtensionsAttribute();
            var validator = new DataAnnotationsClientModelValidator<FileExtensionsAttribute>(attribute);

            var metadata = _metadataProvider.GetMetadataForProperty(
                containerType: typeof(string),
                propertyName: nameof(string.Length));

            var serviceCollection = new ServiceCollection();
            var requestServices = serviceCollection.BuildServiceProvider();

            var context = new ClientModelValidationContext(metadata, _metadataProvider, requestServices);

            // Act
            var results = validator.GetClientValidationRules(context);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void GetClientValidationRules_WithIClientModelValidator_CallsAttribute()
        {
            // Arrange
            var attribute = new TestableAttribute();
            var validator = new DataAnnotationsClientModelValidator<TestableAttribute>(attribute);

            var metadata = _metadataProvider.GetMetadataForProperty(
                containerType: typeof(string),
                propertyName: nameof(string.Length));

            var serviceCollection = new ServiceCollection();
            var requestServices = serviceCollection.BuildServiceProvider();

            var context = new ClientModelValidationContext(metadata, _metadataProvider, requestServices);

            // Act
            var results = validator.GetClientValidationRules(context);

            // Assert
            var rule = Assert.Single(results);
            Assert.Equal("an error", rule.ErrorMessage);
            Assert.Empty(rule.ValidationParameters);
            Assert.Equal("testable", rule.ValidationType);
        }

        private class TestableAttribute : ValidationAttribute, IClientModelValidator
        {
            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
            {
                return new[] { new ModelClientValidationRule(validationType: "testable", errorMessage: "an error") };
            }
        }
    }
}
