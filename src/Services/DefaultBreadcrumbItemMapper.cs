using CMS;
using CMS.Core;
using CMS.DocumentEngine;
using CMS.SiteProvider;

using Kentico.Content.Web.Mvc;

using System;

using Xperience.Core.Breadcrumbs;

[assembly: RegisterImplementation(typeof(IBreadcrumbItemMapper), typeof(DefaultBreadcrumbItemMapper), Lifestyle = Lifestyle.Singleton, Priority = RegistrationPriority.SystemDefault)]
namespace Xperience.Core.Breadcrumbs
{
    /// <summary>
    /// Default implementation of <see cref="IBreadcrumbItemMapper"/>.
    /// </summary>
    public class DefaultBreadcrumbItemMapper : IBreadcrumbItemMapper
    {
        private readonly IPageUrlRetriever pageUrlRetriever;


        public DefaultBreadcrumbItemMapper(IPageUrlRetriever pageUrlRetriever)
        {
            this.pageUrlRetriever = pageUrlRetriever;
        }


        public BreadcrumbItem MapPage(TreeNode page, bool isCurrent = false)
        {
            var url = String.Empty;
            if (!isCurrent)
            {
                url = pageUrlRetriever.Retrieve(page).AbsoluteUrl;
            }

            return new BreadcrumbItem
            {
                Name = page.DocumentName,
                Url = url,
                IsCurrentPage = isCurrent
            };
        }

        public BreadcrumbItem MapSite(SiteInfo site)
        {
            return new BreadcrumbItem
            {
                IsSiteLink = true,
                Name = site.DisplayName,
                Url = site.SitePresentationURL
            };
        }
    }
}
