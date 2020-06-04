using System.Linq;
using MedCore_API.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MedCoreAPI.Util
{
    public class RoutePrefixConvention : IApplicationModelConvention
    {
        #region internals
        const string ApiVersionTemplate = "V{v:ApiVersion}";
        private readonly AttributeRouteModel _routePrefix;
        private readonly AttributeRouteModel _routePrefixWithVersion;

        private readonly AttributeRouteModel _routePrefixWithXml;

        #endregion
        public RoutePrefixConvention(IRouteTemplateProvider route)
        {
            _routePrefix = new AttributeRouteModel(route);
            _routePrefixWithVersion = new AttributeRouteModel(route);
            _routePrefixWithXml = new AttributeRouteModel(route);
            

            _routePrefixWithVersion.Template = _routePrefix.Template.Replace("json", ApiVersionTemplate);
            _routePrefixWithXml.Template = _routePrefix.Template.Replace("json", "xml");
        }
        public void Apply(ApplicationModel application)
        {
            foreach (ControllerModel Controller in application.Controllers)
            {
                foreach (ActionModel action in Controller.Actions)
                {
                    if (action.Attributes.Any(att => att.GetType() == typeof(MapToApiVersionAttribute)))
                    {
                        SetRoutePrefix(action, _routePrefixWithVersion);
                    }
                    else
                    {
                        if (action.Attributes.Any(att => att.GetType() == typeof(MapToXmlAttribute)))
                        {
                            SetRoutePrefix(action, _routePrefixWithXml);
                        }
                        else
                        SetRoutePrefix(action, _routePrefix);
                    }
                }
            }
        }
        private void SetRoutePrefix(ActionModel action, AttributeRouteModel attributeRouteModelPrefix)
        {
            foreach (SelectorModel selector in action.Selectors)
            {
                selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(attributeRouteModelPrefix, selector.AttributeRouteModel);
            }
        }
    }
}