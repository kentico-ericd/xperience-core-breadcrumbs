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
        /// Registers <see cref="BreadcrumbHelper"/> and <see cref="BreadcrumbsWidgetProperties"/> in the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Breadcrumb properties which override the default values.</param>
        public static IServiceCollection AddBreadcrumbs(
            this IServiceCollection services,
            Action<BreadcrumbsWidgetProperties>? configure = null)
        {
            services.AddSingleton<BreadcrumbHelper>();

            // Register widget properties
            var props = new BreadcrumbsWidgetProperties();
            if (configure is object)
            {
                configure(props);
            }
            services.AddSingleton(props);

            return services;
        }
    }
}
