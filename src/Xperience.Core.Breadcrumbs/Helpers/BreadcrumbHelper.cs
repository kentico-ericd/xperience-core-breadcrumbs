using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;

using Kentico.Content.Web.Mvc;

using Microsoft.AspNetCore.Html;

namespace Xperience.Core.Breadcrumbs
{
    /// <summary>
    /// Provides methods for retrieving breadcrumb content and HTML output.
    /// </summary>
    public class BreadcrumbHelper
    {
        private const string CACHE_KEY_FORMAT = "breadcrumbswidget|document|{0}|{1}|{2}";
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IBreadcrumbsRenderer breadcrumbsRenderer;
        private readonly IPageRetriever pageRetriever;
        private readonly BreadcrumbsWidgetProperties breadcrumbsWidgetProperties;


        public BreadcrumbHelper(
            IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IBreadcrumbsRenderer breadcrumbsRenderer,
            IPageRetriever pageRetriever,
            BreadcrumbsWidgetProperties breadcrumbsWidgetProperties)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.breadcrumbsRenderer = breadcrumbsRenderer;
            this.pageRetriever = pageRetriever;
            this.breadcrumbsWidgetProperties = breadcrumbsWidgetProperties;
        }


        /// <summary>
        /// Gets breadrumb HTML using the manually provided <paramref name="props"/>.
        /// </summary>
        /// <param name="props">The breadcrumb properties to use when retrieving and displaying the breadcrumbs.</param>
        public IHtmlContent GetBreadcrumbs(BreadcrumbsWidgetProperties props)
        {
            return GetBreadcrumbContent(props);
        }


        /// <summary>
        /// Gets breadrumb HTML using the <see cref="BreadcrumbsWidgetProperties"/> registered via DI.
        /// </summary>
        public IHtmlContent GetBreadcrumbs()
        {
            return GetBreadcrumbContent(breadcrumbsWidgetProperties);
        }


        /// <summary>
        /// Gets a list of <see cref="BreadcrumbItem"/>s in the order they should be rendered.
        /// </summary>
        /// <param name="props">The breadcrumb properties to use when retrieving the hierarchy.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public IReadOnlyList<BreadcrumbItem> GetHierarchy(BreadcrumbsWidgetProperties props)
        {
            if (props == null)
            {
                throw new ArgumentNullException(nameof(props));
            }

            return GetHierarchyInternal(props);
        }


        private IHtmlContent GetBreadcrumbContent(BreadcrumbsWidgetProperties? props)
        {
            if (props == null)
            {
                throw new ArgumentNullException(nameof(props));
            }

            var hierarchy = GetHierarchyInternal(props);
            var header = breadcrumbsRenderer.RenderOpeningTag(props.ContainerClass);
            var footer = breadcrumbsRenderer.RenderClosingTag();
            var sb = new StringBuilder();
            foreach (BreadcrumbItem bci in hierarchy)
            {
                sb.Append(breadcrumbsRenderer.RenderSeparator(props.Separator));
                if (bci.IsCurrentPage)
                {
                    sb.Append(breadcrumbsRenderer.RenderCurrentPage(bci, props.BreadcrumbItemClass, props.CurrentPageClass));
                }
                else if (bci.IsSiteLink)
                {
                    sb.Append(breadcrumbsRenderer.RenderSiteLink(bci, props.BreadcrumbItemClass));
                }
                else if (string.IsNullOrEmpty(bci.Url))
                {
                    sb.Append(breadcrumbsRenderer.RenderItemWithoutLink(bci, props.BreadcrumbItemClass));
                }
                else
                {
                    sb.Append(breadcrumbsRenderer.RenderItem(bci, props.BreadcrumbItemClass));
                }
            }

            // Remove first separator
            var body = TrimSeparator(sb.ToString().Trim(), props.Separator);
            return new HtmlString($"{header}{body}{footer}");
        }


        private string GetCacheKey(int docID, bool showSiteLink, bool showContainers)
        {
            return string.Format(CACHE_KEY_FORMAT, docID, showSiteLink, showContainers);
        }


        private IReadOnlyList<BreadcrumbItem> GetHierarchyInternal(BreadcrumbsWidgetProperties props)
        {
            if (!pageDataContextRetriever.TryRetrieve<TreeNode>(out var data))
            {
                return new List<BreadcrumbItem>();
            }

            var current = data.Page;
            var hierarchy = CacheHelper.Cache((cs) =>
            {
                ICollection<string> cacheDependencies = new List<string>();
                var list = BuildHierarchyInternal(current, props.ShowSiteLink, props.ShowContainers, ref cacheDependencies);
                cs.CacheDependency = CacheHelper.GetCacheDependency(cacheDependencies);
                return list;
            }, new CacheSettings(120, GetCacheKey(current.DocumentID, props.ShowSiteLink, props.ShowContainers)));

            return hierarchy.ToList().AsReadOnly();
        }


        private IEnumerable<BreadcrumbItem> BuildHierarchyInternal(TreeNode current, bool addSiteLink, bool showContainers, ref ICollection<string> cacheDependencies)
        {
            // Add current page
            var ret = new List<BreadcrumbItem>
            {
                new BreadcrumbItem
                {
                    Name = current.DocumentName,
                    Url = null,
                    IsCurrentPage = true
                }
            };
            cacheDependencies.Add($"documentid|{current.DocumentID}");

            // Get all pages on current page path
            var pages = pageRetriever.RetrieveMultiple(query =>
                query.Where(TreePathUtils.GetNodesOnPathWhereCondition(current.NodeAliasPath, false, false))
            );

            // Add current page's parents in loop
            var nodeLevel = current.NodeLevel - 1;
            while (nodeLevel > 0)
            {
                var parent = pages.Where(p => p.NodeLevel == nodeLevel).FirstOrDefault();
                var type = DataClassInfoProvider.GetDataClassInfo(parent.ClassName);
                if (type != null)
                {
                    if (type.ClassIsCoupledClass ||
                        !type.ClassIsCoupledClass && showContainers)
                    {
                        var url = pageUrlRetriever.Retrieve(parent).AbsoluteUrl;
                        ret.Add(new BreadcrumbItem
                        {
                            Name = parent.DocumentName,
                            Url = url
                        });
                    }
                }
                cacheDependencies.Add($"documentid|{current.DocumentID}");

                nodeLevel--;
            }

            // Add link to main domain if needed
            if (addSiteLink)
            {
                ret.Add(new BreadcrumbItem
                {
                    IsSiteLink = true,
                    Name = current.Site.DisplayName,
                    Url = current.Site.SitePresentationURL
                });
                cacheDependencies.Add($"cms.site|byid|{current.Site.SiteID}");
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
                result = result[trimString.Length..];
            }

            return result;
        }
    }
}
