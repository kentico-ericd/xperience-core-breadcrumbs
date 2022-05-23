namespace Xperience.Core.Breadcrumbs
{
    /// <summary>
    /// Provides the HTML output of the breadcrumbs.
    /// </summary>
    public interface IBreadcrumbsRenderer
    {
        /// <summary>
        /// Renders the opening tag which surrounds the entire breadcrumb output.
        /// </summary>
        public string RenderOpeningTag(string containerClass);


        /// <summary>
        /// Renders the closing tag which surrounds the entire breadcrumb output.
        /// </summary>
        public string RenderClosingTag();


        /// <summary>
        /// Renders the breadcrumb item of the current page.
        /// </summary>
        public string RenderCurrentPage(BreadcrumbItem bci, string breadcrumbItemClass, string currentPageClass);


        /// <summary>
        /// Renders the breadcrumb separator.
        /// </summary>
        public string RenderSeparator(string separator);


        /// <summary>
        /// Renders the current site with a link to main domain.
        /// </summary>
        public string RenderSiteLink(BreadcrumbItem bci, string breadcrumbItemClass);


        /// <summary>
        /// Renders a standard breadcrumb item which contains a link.
        /// </summary>
        public string RenderItem(BreadcrumbItem bci, string breadcrumbItemClass);


        /// <summary>
        /// Renders a breadcrumb item that doesn't contain a link.
        /// </summary>
        public string RenderItemWithoutLink(BreadcrumbItem bci, string breadcrumbItemClass);
    }
}
