namespace Identity.Platform.TagHelpers
{
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

        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (For == null)
            {
                return;
            }

            TagBuilder tbody = new TagBuilder("tbody");

            foreach (ModelMetadata property in For.Metadata.Properties)
            {
                string sentenceCasePropertyName = SentenceCaseRegex.Replace(property.Name, "$1 $2");

                TagBuilder header = new TagBuilder("th");
                header.Attributes["scope"] = "row";
                header.InnerHtml.SetContent(sentenceCasePropertyName);

                TagBuilder row = new TagBuilder("tr");
                row.InnerHtml.AppendHtml(header);
                row.InnerHtml.AppendHtml($"<td>{property.PropertyGetter(For.Model)}</td>");

                tbody.InnerHtml.AppendHtml(row);
            }

            TagBuilder headerRow = new TagBuilder("tr");

            foreach (string header in new[] { "Property", "Value" })
            {
                TagBuilder th = new TagBuilder("th");
                th.Attributes["scope"] = "col";
                th.InnerHtml.SetContent(header);

                headerRow.InnerHtml.AppendHtml(th);
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
    }
}