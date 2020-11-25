using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using Xperience.Components.Widgets.BreadcrumbsWidget;

[assembly: RegisterWidget(BreadcrumbsWidgetViewComponent.IDENTIFIER, typeof(BreadcrumbsWidgetViewComponent), "Breadcrumbs", typeof(BreadcrumbsWidgetProperties), Description = "Displays the breadcrumbs of the current page", IconClass = "icon-l-list-article")]

namespace Xperience.Components.Widgets.BreadcrumbsWidget
{
    public class BreadcrumbsWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "Xperience.BreadcrumbsWidget";

        public HtmlContentViewComponentResult Invoke(ComponentViewModel<BreadcrumbsWidgetProperties> viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var content = BreadcrumbHtmlExtensions.GetBreadcrumbContent(viewModel.Page, viewModel.Properties);

            return new HtmlContentViewComponentResult(content);
        }
    }
}
