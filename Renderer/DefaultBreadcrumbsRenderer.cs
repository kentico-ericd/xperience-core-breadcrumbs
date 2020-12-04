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

        public string RenderCurrentPage(BreadcrumbItem bci)
        {
            return $"<span class='breadcrumb-item breadcrumb-current-page'>{bci.Name}</span>";
        }

        public string RenderItem(BreadcrumbItem bci)
        {
            return $"<span class='breadcrumb-item'><a href='{bci.Url}'>{bci.Name}</a></span>";
        }

        public string RenderItemWithoutLink(BreadcrumbItem bci)
        {
            return $"<span class='breadcrumb-item'>{bci.Name}</span>";
        }

        public string RenderOpeningTag(string containerClass)
        {
            return $"<div class='{containerClass}'>";
        }

        public string RenderSeparator(string separator)
        {
            return $" {separator} ";
        }

        public string RenderSiteLink(BreadcrumbItem bci)
        {
            return RenderItem(bci);
        }
    }
}
