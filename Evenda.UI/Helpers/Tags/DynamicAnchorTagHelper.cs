using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Evenda.UI.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "controller,action,route-params,page")]
    public class DynamicAnchorTagHelper : AnchorTagHelper
    {
        public DynamicAnchorTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public string Controller { get; set; }
        public string Action { get; set; }
        public int Page { get; set; }
        public IDictionary<string, string?> RouteParams { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            RouteValues["controller"] = Controller;
            RouteValues["action"] = Action;
            RouteValues["pg"] = Page.ToString();

            if (RouteParams != null)
            {
                foreach (var param in RouteParams)
                {
                    if (!string.IsNullOrEmpty(param.Value))
                    {
                        RouteValues[param.Key] = param.Value;
                    }
                }
            }

            base.Process(context, output);
        }
    }
}