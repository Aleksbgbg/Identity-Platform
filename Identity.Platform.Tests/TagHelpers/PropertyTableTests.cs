namespace Identity.Platform.Tests.TagHelpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Identity.Platform.TagHelpers;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    using Moq;

    using Xunit;

    public class PropertyTableTests
    {
        [Fact]
        public void GeneratesCorrectTableForModel()
        {
            // Arrange
            PropertyTable propertyTable = new PropertyTable();

            ModelExpressionProvider modelExpressionProvider = new ModelExpressionProvider
            (
                new EmptyModelMetadataProvider(),
                new ExpressionTextCache()
            );

            Model model = new Model("X", "Y");

            propertyTable.For = modelExpressionProvider.CreateModelExpression
            (
                new ViewDataDictionary<Model>
                (
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary()
                ),
                _ => model
            );

            TagHelperContext tagHelperContext = new TagHelperContext
            (
                "propertytable",
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                string.Empty
            );

            Mock<TagHelperContent> tagHelperContentMock = new Mock<TagHelperContent>();
            TagHelperOutput tagHelperOutput = new TagHelperOutput
            (
                string.Empty,
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(tagHelperContentMock.Object)
            );

            // Act
            propertyTable.Process(tagHelperContext, tagHelperOutput);

            // Assert
            Assert.Equal
            (
                $"<table class=\"table\"><thead class=\"thead-dark\"><tr><th scope=\"col\">Property</th><th scope=\"col\">Value</th></tr></thead><tbody><tr><th scope=\"row\">{nameof(model.Key)}</th><td>{model.Key}</td></tr><tr><th scope=\"row\">{nameof(model.Value)}</th><td>{model.Value}</td></tr></tbody></table>",
                tagHelperOutput.PostContent.GetContent()
            );
        }

        private class Model
        {
            public Model(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; }

            public string Value { get; }
        }
    }
}