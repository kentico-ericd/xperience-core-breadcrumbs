[![Nuget](https://img.shields.io/nuget/v/Xperience.Core.Breadcrumbs)](https://www.nuget.org/packages/Xperience.Core.Breadcrumbs)

# Xperience 13 .NET Core Breadcrumb Widget

This is a widget for Xperience .NET Core websites which use [Content Tree-Based routing](https://docs.xperience.io/developing-websites/implementing-routing/content-tree-based-routing).

![screenshot](screenshot.png)

## Installation

1. Install the [Xperience.Core.Breadcrumbs](https://www.nuget.org/packages/Xperience.Core.Breadcrumbs) NuGet package in your .NET Core application
2. Register the breadcrumbs in your application startup:

```cs
using Xperience.Core.Breadcrumbs;

public void ConfigureServices(IServiceCollection services) {
    // Use default properties for all breadcrumbs
    services.AddBreadcrumbs();

    // Or, use these properties for all breadcrumbs
    services.AddBreadcrumbs(options => {
        options.Separator = "|";
        options.ShowContainers = true;
        options.ShowSiteLink = true;
        options.ContainerClass = "breadcrumbs-widget";
        options.BreadcrumbItemClass = "breadcrumb-item";
        options.CurrentPageClass = "breadcrumbs-current";
    });
```

## Using the breadcrumbs

### Adding the pagebuilder widget

The breadcrumbs widget can be added to any page which uses the [page builder](https://docs.xperience.io/developing-websites/page-builder-development/creating-pages-with-editable-areas). It has 6 properties:

- **Show domain link first**: Displays the site name and a link to the root of the site as the first breadcrumb item
- **Show container page types**: If checked, pages that use container page types (e.g. a Folder) will appear in the breadcrumbs
- **Separator**: The text to add between each breadcrumb item
- **Container class**: The CSS class(es) to add the `div` that surrounds the breadcrumbs
- **Item class**: The CSS class(es) added to all breadcrumb items
- **Current page class**: The CSS class(es) add to the current page

### Adding breadcrumbs to views

You can also add the widget directly to any view, such as the main _Layout.cshtml_:

```cs
@using Xperience.Core.Breadcrumbs
@inject BreadcrumbHelper breadcrumbHelper
...
@breadcrumbHelper.GetBreadcrumbs()
```

To override the properties registered during application startup, pass a new `BreadcrumbsWidgetProperties` instance:

```cs
@breadcrumbHelper.GetBreadcrumbs(new BreadcrumbsWidgetProperties()
{
    Separator = "|",
    ShowContainers = true,
    ShowSiteLink = true,
    ContainerClass = "my-breadcrumbs",
    BreadcrumbItemClass = "breadcrumb-item",
    CurrentPageClass = "breadcrumbs-current"
})
```

## Customizations

### Breadcrumb rendering

The final HTML of your breadcrumbs is determined by the `IBreadcrumbsRenderer` interface, which you can find the default code for in [DefaultBreadcrumbsRenderer](/src/Xperience.Core.Breadcrumbs/Services/DefaultBreadcrumbsRenderer.cs). If you'd like to customize the HTML of the breadcrumbs, you can implement your own `IBreadcrumbsRenderer` and use the `RegisterImplementation` attribute to register your code with a higher priority:

```cs
[assembly: RegisterImplementation(typeof(IBreadcrumbsRenderer), typeof(CustomBreadcrumbsRenderer), Lifestyle = Lifestyle.Singleton, Priority = RegistrationPriority.Default)]
namespace MySite.Breadcrumbs {
    /// <summary>
    /// Custom implementation of <see cref="IBreadcrumbsRenderer"/>.
    /// </summary>
    public class CustomBreadcrumbsRenderer : IBreadcrumbsRenderer {
```

### Breadcrumb item generation

Breadcrumb items are provided by the [DefaultBreadcrumbItemMapper](/src/Xperience.Core.Breadcrumbs/Services/DefaultBreadcrumbItemMapper.cs). If you would like to modify how the breadcrumb names, URLs, etc. are generated, you can implement your own `IBreadcrumbItemMapper` and use the `RegisterImplementation` attribute to register your code with a higher priority:

```cs
[assembly: RegisterImplementation(typeof(IBreadcrumbItemMapper), typeof(CustomBreadcrumbItemMapper), Lifestyle = Lifestyle.Singleton, Priority = RegistrationPriority.Default)]
namespace MySite.Breadcrumbs
{
    /// <summary>
    /// Custom implementation of <see cref="IBreadcrumbItemMapper"/>.
    /// </summary>
    public class CustomBreadcrumbItemMapper : IBreadcrumbItemMapper {
```

## Compatibility

This code is only available for use on Kentico Xperience 13 websites using the [.NET Core development model](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core). The website must be using the [content tree-based routing](https://docs.xperience.io/developing-websites/implementing-routing/content-tree-based-routing) model for the breadcrumbs to display properly.

## Feedback & Contributing

Check out the [contributing](https://github.com/kentico-ericd/xperience-core-breadcrumbs/blob/master/CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.

## License

The repository is available as open source under the terms of the [MIT License](https://opensource.org/licenses/MIT).
