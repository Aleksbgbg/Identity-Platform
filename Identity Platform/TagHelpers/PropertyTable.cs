namespace Identity.Platform.TagHelpers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("propertytable")]
    public class PropertyTable : TagHelper
    {
        private static readonly Regex SentenceCaseRegex = new Regex("(.+?)([A-Z])");

        public bool IsCollection { get; set; }

        public ModelExpression For { get; set; }

        public string[] Properties { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (For == null)
            {
                return;
            }

            TagBuilder headerRow = new TagBuilder("tr");
            TagBuilder tbody = new TagBuilder("tbody");

            if (IsCollection)
            {
                ModelPropertyCollection propertyMetadata = For.Metadata.ElementMetadata.Properties;

                foreach (string propertyName in Properties ?? propertyMetadata.Select(property => property.Name))
                {
                    string sentenceCasePropertyName = ToSentenceCase(propertyName);

                    TagBuilder header = new TagBuilder("th");
                    header.Attributes["scope"] = "col";
                    header.InnerHtml.SetContent(sentenceCasePropertyName);

                    headerRow.InnerHtml.AppendHtml(header);
                }

                foreach (object item in (IEnumerable)For.Model)
                {
                    TagBuilder row = new TagBuilder("tr");

                    foreach (ModelMetadata property in propertyMetadata)
                    {
                        object propertyValue = property.PropertyGetter(item);

                        string propertyString;

                        switch (propertyValue)
                        {
                            case string value:
                                propertyString = value;
                                break;
                            case IEnumerable<object> enumerable:
                                propertyString = string.Join(", ", enumerable);
                                break;
                            default:
                                propertyString = propertyValue?.ToString();
                                break;
                        }

                        row.InnerHtml.AppendHtml($"<td>{propertyString}</td>");
                    }

                    tbody.InnerHtml.AppendHtml(row);
                }
            }
            else
            {
                foreach (ModelMetadata property in For.Metadata.Properties)
                {
                    string sentenceCasePropertyName = ToSentenceCase(property.Name);

                    TagBuilder header = new TagBuilder("th");
                    header.Attributes["scope"] = "row";
                    header.InnerHtml.SetContent(sentenceCasePropertyName);

                    TagBuilder row = new TagBuilder("tr");
                    row.InnerHtml.AppendHtml(header);
                    row.InnerHtml.AppendHtml($"<td>{property.PropertyGetter(For.Model)}</td>");

                    tbody.InnerHtml.AppendHtml(row);
                }

                foreach (string header in new[] { "Property", "Value" })
                {
                    TagBuilder th = new TagBuilder("th");
                    th.Attributes["scope"] = "col";
                    th.InnerHtml.SetContent(header);

                    headerRow.InnerHtml.AppendHtml(th);
                }
            }

            TagBuilder thead = new TagBuilder("thead");
            thead.AddCssClass("thead-dark");
            thead.InnerHtml.AppendHtml(headerRow);

            TagBuilder table = new TagBuilder("table");
            table.AddCssClass("table");
            table.InnerHtml.AppendHtml(thead);
            table.InnerHtml.AppendHtml(tbody);

            output.PostContent.SetHtmlContent(table);
        }

        private static string ToSentenceCase(string value)
        {
            return SentenceCaseRegex.Replace(value, "$1 $2");
        }
    }
}