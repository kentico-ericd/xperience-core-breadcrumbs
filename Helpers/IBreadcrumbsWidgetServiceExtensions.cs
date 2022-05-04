using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;

namespace Xperience.Core.Breadcrumbs
{
    public static class IBreadcrumbsWidgetServiceExtensions
    {
        public static IServiceCollection AddBreadcrumbs(
            this IServiceCollection services,
            Action<BreadcrumbsWidgetProperties> configure = null,
            IBreadcrumbsRenderer renderer = null)
        {
            services.AddSingleton<BreadcrumbHelper>();

            // Register renderer
            if (renderer is null)
            {
                renderer = new DefaultBreadcrumbsRenderer();
            }
            services.AddSingleton(renderer);

            // Register widget properties
            var props = new BreadcrumbsWidgetProperties().SetDefaults();
            if (configure is object)
            {
                configure(props);
            }
            services.AddSingleton(props);

            return services;
        }
    }
}
