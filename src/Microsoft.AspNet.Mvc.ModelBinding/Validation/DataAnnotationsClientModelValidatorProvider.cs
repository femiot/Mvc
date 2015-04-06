// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// An implementation of <see cref="IClientModelValidatorProvider"/> which providers client validators
    /// for attributes which derive from <see cref="ValidationAttribute"/>. It also provides
    /// a validator for types which implement <see cref="IClientModelValidator"/>.
    /// The logic to support <see cref="IClientModelValidator"/>
    /// is implemented in <see cref="DataAnnotationsClientModelValidator"/>.
    /// </summary>
    public class DataAnnotationsClientModelValidatorProvider : IClientModelValidatorProvider
    {
        // A factory for validators based on ValidationAttribute.
        internal delegate IClientModelValidator
            DataAnnotationsClientModelValidationFactory(ValidationAttribute attribute);

        // Factories for validation attributes
        private static readonly DataAnnotationsClientModelValidationFactory _defaultAttributeFactory =
            (attribute) => new DataAnnotationsClientModelValidator(attribute);

        private readonly Dictionary<Type, DataAnnotationsClientModelValidationFactory> _attributeFactories =
            BuildAttributeFactoriesDictionary();

        internal Dictionary<Type, DataAnnotationsClientModelValidationFactory> AttributeFactories
        {
            get { return _attributeFactories; }
        }

        /// <inheritdoc />
        public void GetValidators(ClientValidatorProviderContext context)
        {
            foreach (var attribute in context.ValidatorMetadata.OfType<ValidationAttribute>())
            {
                DataAnnotationsClientModelValidationFactory factory;
                if (!_attributeFactories.TryGetValue(attribute.GetType(), out factory))
                {
                    factory = _defaultAttributeFactory;
                }

                context.Validators.Add(factory(attribute));
            }
        }

        private static Dictionary<Type, DataAnnotationsClientModelValidationFactory> BuildAttributeFactoriesDictionary()
        {
            var dict = new Dictionary<Type, DataAnnotationsClientModelValidationFactory>();
            AddValidationAttributeAdapter(dict, typeof(RegularExpressionAttribute),
                (attribute) => new RegularExpressionAttributeAdapter((RegularExpressionAttribute)attribute));

            AddValidationAttributeAdapter(dict, typeof(MaxLengthAttribute),
                (attribute) => new MaxLengthAttributeAdapter((MaxLengthAttribute)attribute));

            AddValidationAttributeAdapter(dict, typeof(MinLengthAttribute),
                (attribute) => new MinLengthAttributeAdapter((MinLengthAttribute)attribute));

            AddValidationAttributeAdapter(dict, typeof(CompareAttribute),
                (attribute) => new CompareAttributeAdapter((CompareAttribute)attribute));

            AddValidationAttributeAdapter(dict, typeof(RequiredAttribute),
                (attribute) => new RequiredAttributeAdapter((RequiredAttribute)attribute));

            AddValidationAttributeAdapter(dict, typeof(RangeAttribute),
                (attribute) => new RangeAttributeAdapter((RangeAttribute)attribute));

            AddValidationAttributeAdapter(dict, typeof(StringLengthAttribute),
                (attribute) => new StringLengthAttributeAdapter((StringLengthAttribute)attribute));

            AddDataTypeAttributeAdapter(dict, typeof(CreditCardAttribute), "creditcard");
            AddDataTypeAttributeAdapter(dict, typeof(EmailAddressAttribute), "email");
            AddDataTypeAttributeAdapter(dict, typeof(PhoneAttribute), "phone");
            AddDataTypeAttributeAdapter(dict, typeof(UrlAttribute), "url");

            return dict;
        }

        private static void AddValidationAttributeAdapter(
            Dictionary<Type, DataAnnotationsClientModelValidationFactory> dictionary,
            Type validationAttributeType,
            DataAnnotationsClientModelValidationFactory factory)
        {
            if (validationAttributeType != null)
            {
                dictionary.Add(validationAttributeType, factory);
            }
        }

        private static void AddDataTypeAttributeAdapter(
            Dictionary<Type, DataAnnotationsClientModelValidationFactory> dictionary,
            Type attributeType,
            string ruleName)
        {
            AddValidationAttributeAdapter(
                dictionary,
                attributeType,
                (attribute) => new DataTypeAttributeAdapter((DataTypeAttribute)attribute, ruleName));
        }
    }
}
