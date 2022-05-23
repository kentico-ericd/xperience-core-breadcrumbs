using CMS.DocumentEngine;
using CMS.SiteProvider;

namespace Xperience.Core.Breadcrumbs
{
    /// <summary>
    /// Converts Xperience objects into <see cref="BreadcrumbItem"/>s.
    /// </summary>
    public interface IBreadcrumbItemMapper
    {
        /// <summary>
        /// Generates a <see cref="BreadcrumbItem"/> for an Xperience content tree page.
        /// </summary>
        /// <param name="page">The page to generate the breadcrumb for.</param>
        /// <param name="isCurrent">If <c>true</c>, the <paramref name="page"/> is what the visitor is currently viewing.</param>
        public BreadcrumbItem MapPage(TreeNode page, bool isCurrent = false);


        /// <summary>
        /// Generates a <see cref="BreadcrumbItem"/> for the current site.
        /// </summary>
        /// <param name="site">The current Xperience site.</param>
        public BreadcrumbItem MapSite(SiteInfo site);
    }
}
