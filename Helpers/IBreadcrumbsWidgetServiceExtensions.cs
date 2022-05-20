using System;

using Microsoft.Extensions.DependencyInjection;

namespace Xperience.Core.Breadcrumbs
{
    /// <summary>
    /// Startup extensions for registering breadcrumb configuration.
    /// </summary>
    public static class IBreadcrumbsWidgetServiceExtensions
    {
        /// <summary>
        /// Registers <see cref="BreadcrumbHelper"/>, <see cref="IBreadcrumbsRenderer"/>, and <see cref="BreadcrumbsWidgetProperties"/>
        /// in the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Breadcrumb properties which override the default values.</param>
        /// <param name="renderer">An optional <see cref="IBreadcrumbsRenderer"/> implementation.</param>
        public static IServiceCollection AddBreadcrumbs(
            this IServiceCollection services,
            Action<BreadcrumbsWidgetProperties>? configure = null,
            IBreadcrumbsRenderer? renderer = null)
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
