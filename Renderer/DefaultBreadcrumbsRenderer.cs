using System;
using System.Collections.Generic;
using System.Text;

namespace Xperience.Core.Breadcrumbs
{
    public class DefaultBreadcrumbsRenderer : IBreadcrumbsRenderer
    {
        public string RenderClosingTag()
        {
            return "</div>";
        }

        public string RenderCurrentPage(BreadcrumbItem bci, string breadcrumbItemClass, string currentPageClass)
        {
            return $"<span class='{breadcrumbItemClass} {currentPageClass}'>{bci.Name}</span>";
        }

        public string RenderItem(BreadcrumbItem bci, string breadcrumbItemClass)
        {
            return $"<span class='{breadcrumbItemClass}'><a href='{bci.Url}'>{bci.Name}</a></span>";
        }

        public string RenderItemWithoutLink(BreadcrumbItem bci, string breadcrumbItemClass)
        {
            return $"<span class='{breadcrumbItemClass}'>{bci.Name}</span>";
        }

        public string RenderOpeningTag(string containerClass)
        {
            return $"<div class='{containerClass}'>";
        }

        public string RenderSeparator(string separator)
        {
            return $" {separator} ";
        }

        public string RenderSiteLink(BreadcrumbItem bci, string breadcrumbItemClass)
        {
            return RenderItem(bci, breadcrumbItemClass);
        }
    }
}
