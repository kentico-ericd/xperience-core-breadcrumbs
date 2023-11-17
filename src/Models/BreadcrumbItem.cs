namespace Xperience.Core.Breadcrumbs
{
    /// <summary>
    /// Represents a single breadcrumb item to be rendered.
    /// </summary>
    public class BreadcrumbItem
    {
        /// <summary>
        /// The text to be displayed for the breadcrumb.
        /// </summary>
        public string? Name
        {
            get;
            set;
        }


        /// <summary>
        /// The absolute URL of the breadcrumb.
        /// </summary>
        public string? Url
        {
            get;
            set;
        }


        /// <summary>
        /// <c>True</c> if this breadcrumb item represents the current page.
        /// </summary>
        public bool IsCurrentPage
        {
            get;
            set;
        } = false;


        /// <summary>
        /// <c>True</c> if this breadcrumb item displays the current site name and URL.
        /// </summary>
        public bool IsSiteLink
        {
            get;
            set;
        } = false;
    }
}
