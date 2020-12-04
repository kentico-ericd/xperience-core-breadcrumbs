namespace Xperience.Core.Breadcrumbs
{
    public interface IBreadcrumbsRenderer
    {
        /// <summary>
        /// Renders the opening tag which surrounds the entire breadcrumb output
        /// </summary>
        public abstract string RenderOpeningTag(string containerClass);

        /// <summary>
        /// Renders the closing tag which surrounds the entire breadcrumb output
        /// </summary>
        public abstract string RenderClosingTag();

        /// <summary>
        /// Renders the breadcrumb item of the current page
        /// </summary>
        public abstract string RenderCurrentPage(BreadcrumbItem bci, string breadcrumbItemClass, string currentPageClass);

        /// <summary>
        /// Renders the breadcrumb separator
        /// </summary>
        public abstract string RenderSeparator(string separator);

        /// <summary>
        /// Renders the current site with a link to main domain
        /// </summary>
        public abstract string RenderSiteLink(BreadcrumbItem bci, string breadcrumbItemClass);

        /// <summary>
        /// Renders a standard breadcrumb item which contains a link
        /// </summary>
        public abstract string RenderItem(BreadcrumbItem bci, string breadcrumbItemClass);

        /// <summary>
        /// Renders a breadcrumb item that doesn't contain a link
        /// </summary>
        public abstract string RenderItemWithoutLink(BreadcrumbItem bci, string breadcrumbItemClass);
    }
}
