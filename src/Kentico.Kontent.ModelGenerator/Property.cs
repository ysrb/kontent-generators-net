﻿using System;
using System.Collections.Generic;

namespace Kentico.Kontent.ModelGenerator
{
    public class Property
    {
        public const string STRUCTURED_SUFFIX = "(structured)";

        public string Identifier => TextHelpers.GetValidPascalCaseIdentifierName(Codename);

        public string Codename { get; }

        /// <summary>
        /// Returns return type of the property in a string format (e.g.: "string").
        /// </summary>
        public string TypeName { get; private set; }

        private static readonly Dictionary<string, string> DeliverTypes = new Dictionary<string, string>
        {
            { "text", "string" },
            { "rich_text", "string" },
            { "rich_text" + STRUCTURED_SUFFIX, "IRichTextContent" },
            { "number", "decimal?" },
            { "multiple_choice", "IEnumerable<MultipleChoiceOption>" },
            { "date_time", "DateTime?" },
            { "asset", "IEnumerable<Asset>" },
            { "modular_content", "IEnumerable<object>" },
            { "taxonomy", "IEnumerable<TaxonomyTerm>" },
            { "url_slug", "string" },
            { "custom", "string" }
        };

        private static readonly Dictionary<string, string> ContentManagementTypes = new Dictionary<string, string>
        {
            { "text", "string" },
            { "rich_text", "string" },
            { "number", "decimal?" },
            { "multiple_choice", "IEnumerable<MultipleChoiceOptionIdentifier>" },
            { "date_time", "DateTime?" },
            { "asset", "IEnumerable<AssetIdentifier>" },
            { "modular_content", "IEnumerable<ContentItemIdentifier>" },
            { "taxonomy", "IEnumerable<TaxonomyTermIdentifier>" },
            { "url_slug", "string" },
            { "custom", "string" }
        };

        private static Dictionary<string, string> contentTypeToTypeName(bool cmApi) 
            => cmApi ? ContentManagementTypes : DeliverTypes;

        public Property(string codename, string typeName)
        {
            Codename = codename;
            TypeName = typeName;
        }

        public static bool IsContentTypeSupported(string contentType, bool cmApi = false)
        {
            return contentTypeToTypeName(cmApi).ContainsKey(contentType);
        }

        public static Property FromContentType(string codename, string contentType, bool cmApi = false)
        {
            if (!IsContentTypeSupported(contentType, cmApi))
            {
                throw new ArgumentException($"Unknown Content Type {contentType}", nameof(contentType));
            }

            return new Property(codename, contentTypeToTypeName(cmApi)[contentType]);
        }
    }
}
