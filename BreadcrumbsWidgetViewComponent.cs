using CMS;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Html;
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


        private readonly BreadcrumbHelper? helper;


        public BreadcrumbsWidgetViewComponent(BreadcrumbHelper? helper = null)
        {
            this.helper = helper;
        }


        public HtmlContentViewComponentResult Invoke(ComponentViewModel<BreadcrumbsWidgetProperties> viewModel)
        {
            IHtmlContent content;
            if (viewModel != null)
            {
                if (viewModel.Properties.Separator is null)
                {
                    // Adding new widget, try to get properties from DI
                    if (helper is null)
                    {
                        // DI not available and properties were not initialized with defaults, set them now
                        content = new BreadcrumbHelper().GetBreadcrumbs(new BreadcrumbsWidgetProperties().SetDefaults());
                    }
                    else
                    {
                        // DI is available, get properties from registered BreadcrumbsWidgetProperties
                        content = helper.GetBreadcrumbs();
                    }
                }
                else
                {
                    // Properties were supplied by widget properties dialog in admin
                    content = new BreadcrumbHelper().GetBreadcrumbs(viewModel.Properties);
                }
            }
            else
            {
                if (helper != null)
                {
                    // Breadcrumbs are being rendered in a view, get properties from DI
                    content = helper.GetBreadcrumbs();
                }
                else
                {
                    content = new HtmlString("Widget properties are null and no BreadcrumbHelper was injected.");
                }
            }

            return new HtmlContentViewComponentResult(content);
        }
    }
}
