namespace Identity.Platform.Attributes.Constraints
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ActionConstraints;
    using Microsoft.AspNetCore.Routing;

    public class RouteParameterConstraintAttribute : ActionMethodSelectorAttribute
    {
        private readonly string[] _routeParameters;

        public RouteParameterConstraintAttribute(params string[] routeParameters)
        {
            _routeParameters = routeParameters;
        }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            return _routeParameters.All(routeContext.RouteData.Values.ContainsKey);
        }
    }
}