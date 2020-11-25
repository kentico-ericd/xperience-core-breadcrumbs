# Xperience 13 .NET Core Breadcrumb Widget

This is a widget for Xperience .NET Core websites which use [Content Tree-Based routing](https://docs.xperience.io/developing-websites/implementing-routing/content-tree-based-routing).

![screenshot](/Components/Widgets/BreadcrumbsWidget/screenshot.png)

## Adding the widget to a page

The widget can be added to any page which uses the [page builder](https://docs.xperience.io/developing-websites/page-builder-development/creating-pages-with-editable-areas). It has 4 properties:

- **Show domain link first**: Displays the site name and a link to the root of the site as the first breadcrumb item
- **Show container page types**: If checked, pages that use container page types (e.g. a Folder) will appear in the breadcrumbs
- **Separator**: The text to add between each breadcrumb item
- **Container class**: The CSS class(es) to add the `div` that surrounds the breadcrumbs

## Adding breadcrumbs to views

You can also add breadcrumbs directly to any view, such as the main **_Layout.cshtml**. Add the following to the view:

```
@using Xperience.Components.Widgets.BreadcrumbsWidget
@Html.GetBreadcrumbContent()
```
The breadcrumbs will be initialized with default properties. To specify your own properties, pass an instance of `BreadcrumbsWidgetProperties`:

```
@Html.GetBreadcrumbContent(new BreadcrumbsWidgetProperties() {
    ShowContainers = true,
    ShowSiteLink = true,
    ClassName = "breadcrumb-container",
    Separator = ">"
})
```