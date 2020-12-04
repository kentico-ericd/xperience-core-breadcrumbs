using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xperience.Core.Breadcrumbs
{
    public class BreadcrumbHelper
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        
        public BreadcrumbsWidgetProperties Properties { get; set; }

        public BreadcrumbHelper() {
            pageDataContextRetriever = Service.Resolve<IPageDataContextRetriever>();
            pageUrlRetriever = Service.Resolve<IPageUrlRetriever>();
        }

        public BreadcrumbHelper(
            IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public BreadcrumbHelper(
            IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            BreadcrumbsWidgetProperties breadcrumbsWidgetProperties)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            Properties = breadcrumbsWidgetProperties;
        }

        /// <summary>
        /// Returns breadrumb HTML using the manually provided BreadcrumbsWidgetProperties
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public IHtmlContent GetBreadcrumbs(BreadcrumbsWidgetProperties props)
        {
            return GetBreadcrumbContent(props);
        }

        /// <summary>
        /// Returns breadrumb HTML using the BreadcrumbsWidgetProperties registered via DI
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public IHtmlContent GetBreadcrumbs()
        {
            return GetBreadcrumbContent(Properties);
        }

        private IHtmlContent GetBreadcrumbContent(BreadcrumbsWidgetProperties props)
        {
            if (props is null)
            {
                throw new ArgumentNullException(nameof(props));
            }

            var current = pageDataContextRetriever.Retrieve<TreeNode>().Page;
            var hierarchy = GetHierarchy(current, props.ShowSiteLink, props.ShowContainers);

            var sb = new StringBuilder();
            foreach (BreadcrumbItem bci in hierarchy)
            {
                sb.Append($" {props.Separator} ");
                if (bci.IsCurrentPage)
                {
                    sb.Append($"<span class='breadcrumb-item breadcrumb-current-page'>{bci.Name}</span>");
                }
                else if (bci.Url == null)
                {
                    sb.Append($"<span class='breadcrumb-item'>{bci.Name}</span>");
                }
                else
                {
                    sb.Append($"<span class='breadcrumb-item'><a href='{bci.Url}'>{bci.Name}</a></span>");
                }
            }

            // Remove first separator
            var str = TrimSeparator(sb.ToString().Trim(), props.Separator);
            return new HtmlString($"<div class='{props.ClassName}'>{str}</div>");
        }

        private IEnumerable<BreadcrumbItem> GetHierarchy(TreeNode current, bool addSiteLink, bool showContainers)
        {
            // Add current page
            var ret = new List<BreadcrumbItem>();
            ret.Add(new BreadcrumbItem()
            {
                Name = current.DocumentName,
                Url = null,
                IsCurrentPage = true
            });

            // Add current page's parents in loop
            var parent = current.Parent;
            while (parent != null)
            {
                if (parent.IsRoot()) break;

                var type = DataClassInfoProvider.GetDataClassInfo(parent.ClassName);
                if (type != null)
                {
                    if (type.ClassIsCoupledClass ||
                        !type.ClassIsCoupledClass && showContainers)
                    {
                        var url = pageUrlRetriever.Retrieve(parent).AbsoluteUrl;
                        ret.Add(new BreadcrumbItem()
                        {
                            Name = parent.DocumentName,
                            Url = url
                        });
                    }

                    parent = parent.Parent;
                }
            }

            // Add link to main domain if needed
            if (addSiteLink)
            {
                ret.Add(new BreadcrumbItem()
                {
                    Name = current.Site.DisplayName,
                    Url = current.Site.SitePresentationURL
                });
            }

            ret.Reverse();
            return ret;
        }

        private string TrimSeparator(string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString)) return target;

            string result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }
    }
}
