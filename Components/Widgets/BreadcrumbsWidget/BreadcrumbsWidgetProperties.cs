using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace Xperience.Components.Widgets.BreadcrumbsWidget
{
    public class BreadcrumbsWidgetProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "Separator", DefaultValue = ">")]
        public string Separator { get; set; }

        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 1, Label = "Show domain link first", DefaultValue = true)]
        public bool ShowSiteLink { get; set; }

        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 2, Label = "Show container page types", DefaultValue = false)]
        public bool ShowContainers { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "Container class", DefaultValue = "breadcrumb-container")]
        public string ClassName { get; set; }

        public void SetDefaults()
        {
            Separator = ">";
            ShowSiteLink = true;
            ShowContainers = true;
            ClassName = "breadcrumb-container";
        }
    }
}