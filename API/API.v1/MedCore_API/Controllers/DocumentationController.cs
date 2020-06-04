using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MedCoreAPI.Controllers
{
    public class DocumentationController : ControllerBase
    {
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        public DocumentationController(IApiDescriptionGroupCollectionProvider apiExplorer) 
        {
            _apiExplorer = apiExplorer;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[controller]/GetTypeActionReturn")]
        public string GetTypeActionReturn(string url)
        {
            IReadOnlyList<ApiDescription> apiDescriptionsGroup = _apiExplorer.ApiDescriptionGroups.Items.First().Items;
            ApiDescription apiDescription = SearchForApiDescription(apiDescriptionsGroup, url);

            return apiDescription.SupportedResponseTypes.FirstOrDefault().Type.AssemblyQualifiedName;
        }

        private ApiDescription SearchForApiDescription(IReadOnlyList<ApiDescription> apiDescriptionsGroup, string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("Não existe endpoint nesse endereço");

            ApiDescription apiDescription = apiDescriptionsGroup.FirstOrDefault(a => a.RelativePath.Contains(url));

            if(apiDescription == null) 
            {
                url = FormatUrlToFindAction(url);
                return SearchForApiDescription(apiDescriptionsGroup, url);
            }
            else return apiDescription;
        }

        private string FormatUrlToFindAction(string url)
        {
            url = url.Replace("V2", "V{v}");

            return GetMinimalFormatUrl(url);
        }

        private string GetMinimalFormatUrl(string url)
        {
            var splitedUrl = url.Split("/",StringSplitOptions.RemoveEmptyEntries).SkipLast(1);

            if (splitedUrl.Count() == 1) return string.Empty;

            string newUrl = "";

            foreach (var item in splitedUrl)
            { 
                newUrl += item;
                newUrl += "/";
            }

            return newUrl;

        }
    }
}
