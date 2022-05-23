using CMS;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

using Xperience.Core.Breadcrumbs;

[assembly: AssemblyDiscoverable]
[assembly: RegisterWidget(BreadcrumbsWidgetViewComponent.IDENTIFIER, typeof(BreadcrumbsWidgetViewComponent), "Breadcrumbs", typeof(BreadcrumbsWidgetProperties), Description = "Displays the breadcrumbs of the current page", IconClass = "icon-l-list-article")]

namespace Xperience.Core.Breadcrumbs
{
    /// <summary>
    /// An Xperience pagebuilder widget which renders breadcrumbs.
    /// </summary>
    public class BreadcrumbsWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// The unique identifier of the widget.
        /// </summary>
        public const string IDENTIFIER = "Xperience.Core.Breadcrumbs";


        private readonly BreadcrumbHelper helper;


        public BreadcrumbsWidgetViewComponent(BreadcrumbHelper helper)
        {
            this.helper = helper;
        }


        public HtmlContentViewComponentResult Invoke(ComponentViewModel<BreadcrumbsWidgetProperties> viewModel)
        {
            if (viewModel == null || viewModel.Properties.Separator == null)
            {
                // Adding new widget, use default properties
                return new HtmlContentViewComponentResult(helper.GetBreadcrumbs());
            }
            
            // Properties were supplied by widget properties dialog in admin
            return new HtmlContentViewComponentResult(helper.GetBreadcrumbs(viewModel.Properties));
        }
    }
}
