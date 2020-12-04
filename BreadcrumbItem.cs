namespace Xperience.Core.Breadcrumbs
{
    public class BreadcrumbItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsCurrentPage { get; set; } = false;

        public BreadcrumbItem()
        {

        }
    }
}
