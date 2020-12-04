using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace Xperience.Core.Breadcrumbs
{
    public class BreadcrumbsWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// HTML to be inserted between breadcrumb items
        /// </summary>
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "Separator")]
        public string Separator { get; set; }

        /// <summary>
        /// If true, a link to the current site will be displayed as the first item in the breadcrumbs
        /// </summary>
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 1, Label = "Show domain link first")]
        public bool ShowSiteLink { get; set; }

        /// <summary>
        /// If true, pages which use a container page type (e.g. folders) will be displayed in the breadcrumbs
        /// </summary>
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 2, Label = "Show container page types")]
        public bool ShowContainers { get; set; }

        /// <summary>
        /// One or more CSS classes to be added to the DIV which surrounds the breadcrumbs
        /// </summary>
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "Container class")]
        public string ContainerClass { get; set; }

        public BreadcrumbsWidgetProperties()
        {

        }

        public BreadcrumbsWidgetProperties SetDefaults()
        {
            Separator = "|";
            ContainerClass = "breadcrumbs-widget";
            ShowContainers = true;
            ShowSiteLink = true;
            return this;
        }
    }
}