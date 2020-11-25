using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Text;

namespace Xperience.Components.Widgets.BreadcrumbsWidget
{
    public static class BreadcrumbHtmlExtensions
    {
        public static IHtmlContent GetBreadcrumbContent(this IHtmlHelper html, BreadcrumbsWidgetProperties props = null)
        {
            var pageDataContextRetriever = Service.Resolve<IPageDataContextRetriever>();
            var current = pageDataContextRetriever.Retrieve<TreeNode>().Page;

            if (props == null)
            {
                props = new BreadcrumbsWidgetProperties();
                props.SetDefaults();
            }

            return GetBreadcrumbContent(current, props);
        }

        public static IHtmlContent GetBreadcrumbContent(TreeNode current, BreadcrumbsWidgetProperties model)
        {
            var hierarchy = GetHierarchy(current, model.ShowSiteLink, model.ShowContainers);

            var sb = new StringBuilder();
            foreach (BreadcrumbItem bci in hierarchy)
            {
                sb.Append($" {model.Separator} ");
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
            var str = TrimSeparator(sb.ToString().Trim(), model.Separator);
            return new HtmlString($"<div class='{model.ClassName}'>{str}</div>");
        }

        public static IEnumerable<BreadcrumbItem> GetHierarchy(TreeNode current, bool addSiteLink, bool showContainers)
        {
            var pageUrlRetriever = Service.Resolve<IPageUrlRetriever>();

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

        public static string TrimSeparator(string target, string trimString)
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
