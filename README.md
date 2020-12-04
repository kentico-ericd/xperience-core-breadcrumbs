[![Nuget](https://img.shields.io/nuget/v/Xperience.Core.Breadcrumbs)](https://www.nuget.org/packages/Xperience.Core.Breadcrumbs)

# Xperience 13 .NET Core Breadcrumb Widget

This is a widget for Xperience .NET Core websites which use [Content Tree-Based routing](https://docs.xperience.io/developing-websites/implementing-routing/content-tree-based-routing).

![screenshot](screenshot.png)

## Installation

1. Install the [Xperience.Core.Breadcrumbs](https://www.nuget.org/packages/Xperience.Core.Breadcrumbs) NuGet package in your .NET Core application
1. Register the breadcrumbs with [dependency injection](#registering-the-widget-via-dependency-injection) (optional)
1. Build and [deploy](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core/deploying-and-hosting-asp-net-core-applications) the website.

## Registering the widget via dependency injection

You can configure a set of default widget properties using dependency injection. First, call `AddBreadcrumbs` in your [startup code](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core/starting-with-asp-net-core-development#StartingwithASP.NETCoredevelopment-Configuringapplicationstartup) and provide the default properties:

```
using Xperience.Core.Breadcrumbs;
...
services.AddBreadcrumbs(new BreadcrumbsWidgetProperties()
{
    ClassName = "my-breadcrumbs",
    Separator = "|",
    ShowContainers = true,
    ShowSiteLink = true
});
```

This registers `BreadcrumbHelper` and `BreadcrumbsWidgetProperties` with dependency injection, so these will be available in your views and code files. All new breadcrumb widgets added in the interface or rendered directly to views will use these properties, unless overridden.


## Adding the widget to a page

The widget can be added to any page which uses the [page builder](https://docs.xperience.io/developing-websites/page-builder-development/creating-pages-with-editable-areas). It has 4 properties:

- **Show domain link first**: Displays the site name and a link to the root of the site as the first breadcrumb item
- **Show container page types**: If checked, pages that use container page types (e.g. a Folder) will appear in the breadcrumbs
- **Separator**: The text to add between each breadcrumb item
- **Container class**: The CSS class(es) to add the `div` that surrounds the breadcrumbs

## Adding breadcrumbs to views

You can also [add the widget directly to any view](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core/page-builder-development-in-asp-net-core/rendering-widgets-directly-in-asp-net-core), such as the main **_Layout.cshtml**. The code varies depending on whether you used the optional [dependency injection](#registering-the-widget-via-dependency-injection) .

### If you are using the DI approach

You can render the breadcrumbs with the default properties using:

```
@using Xperience.Core.Breadcrumbs
@inject BreadcrumbHelper breadcrumbHelper
...
@breadcrumbHelper.GetBreadcrumbs()
```

To override the default properties, pass a new `BreadcrumbsWidgetProperties` instance:

```
@using Xperience.Core.Breadcrumbs
@inject BreadcrumbHelper breadcrumbHelper
...
@breadcrumbHelper.GetBreadcrumbs(new BreadcrumbsWidgetProperties()
{
    ClassName = "my-breadcrumbs",
    Separator = ">",
    ShowContainers = true,
    ShowSiteLink = true
})
```

### If you aren't using DI

The code is very similar to the DI approach, but since `BreadcrumbHelper` cannot be injected, you need to create a new instance:

```
@using Xperience.Core.Breadcrumbs
...
@Html.Raw(new BreadcrumbHelper().GetBreadcrumbs(new BreadcrumbsWidgetProperties()
{
    ClassName = "my-breadcrumbs",
    Separator = ">",
    ShowContainers = true,
    ShowSiteLink = true
}))
```

## Compatibility

This code is only available for use on Kentico Xperience 13 websites using the [.NET Core development model](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core). The website must be using the [content tree-based routing](https://docs.xperience.io/developing-websites/implementing-routing/content-tree-based-routing) model for the breadcrumbs to display properly.

## Feedback & Contributing

Check out the [contributing](https://github.com/kentico-ericd/xperience-core-breadcrumbs/blob/master/CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.

## License

The repository is available as open source under the terms of the [MIT License](https://opensource.org/licenses/MIT).
