using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Html;

namespace Xperience.Core.Breadcrumbs
{
    public class BreadcrumbHelper
    {
        private const string CACHE_KEY_FORMAT = "breadcrumbswidget|document|{0}|{1}|{2}";
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IBreadcrumbsRenderer breadcrumbsRenderer;
        private readonly IPageRetriever pageRetriever;

        public BreadcrumbsWidgetProperties? Properties { get; set; }

        public BreadcrumbHelper() : this(
            Service.Resolve<IPageDataContextRetriever>(),
            Service.Resolve<IPageUrlRetriever>(),
            new DefaultBreadcrumbsRenderer(),
            Service.Resolve<IPageRetriever>(),
            null)
        {

        }

        public BreadcrumbHelper(
            IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IPageRetriever pageRetriever) : this(
                pageDataContextRetriever,
                pageUrlRetriever,
                new DefaultBreadcrumbsRenderer(),
                pageRetriever,
                null)
        {

        }

        public BreadcrumbHelper(
            IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IBreadcrumbsRenderer breadcrumbsRenderer,
            IPageRetriever pageRetriever,
            BreadcrumbsWidgetProperties? breadcrumbsWidgetProperties)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.breadcrumbsRenderer = breadcrumbsRenderer;
            this.pageRetriever = pageRetriever;
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

        public IReadOnlyList<BreadcrumbItem> GetHierarchy(BreadcrumbsWidgetProperties? props)
        {
            props = props ?? Properties;

            if (props is null)
            {
                throw new ArgumentNullException(nameof(props));
            }

            var current = pageDataContextRetriever.Retrieve<TreeNode>().Page;
            var hierarchy = CacheHelper.Cache((cs) =>
            {
                ICollection<string> cacheDependencies = new List<string>();
                var list = GetHierarchyInternal(current, props.ShowSiteLink, props.ShowContainers, ref cacheDependencies);
                cs.CacheDependency = CacheHelper.GetCacheDependency(cacheDependencies);
                return list;
            }, new CacheSettings(120, GetCacheKey(current.DocumentID, props.ShowSiteLink, props.ShowContainers)));

            return hierarchy.ToList().AsReadOnly();
        }

        private IHtmlContent GetBreadcrumbContent(BreadcrumbsWidgetProperties? props)
        {
            if (props is null)
            {
                throw new ArgumentNullException(nameof(props));
            }

            var current = pageDataContextRetriever.Retrieve<TreeNode>().Page;
            var hierarchy = CacheHelper.Cache((cs) =>
            {

                ICollection<string> cacheDependencies = new List<string>();
                var list = GetHierarchyInternal(current, props.ShowSiteLink, props.ShowContainers, ref cacheDependencies);
                cs.CacheDependency = CacheHelper.GetCacheDependency(cacheDependencies);
                return list;
            }, new CacheSettings(120, GetCacheKey(current.DocumentID, props.ShowSiteLink, props.ShowContainers)));

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

        private IEnumerable<BreadcrumbItem> GetHierarchyInternal(TreeNode current, bool addSiteLink, bool showContainers, ref ICollection<string> cacheDependencies)
        {
            // Add current page
            var ret = new List<BreadcrumbItem>();
            ret.Add(new BreadcrumbItem()
            {
                Name = current.DocumentName,
                Url = null,
                IsCurrentPage = true
            });
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
                        ret.Add(new BreadcrumbItem()
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
                ret.Add(new BreadcrumbItem()
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
                result = result.Substring(trimString.Length);
            }

            return result;
        }
    }
}
