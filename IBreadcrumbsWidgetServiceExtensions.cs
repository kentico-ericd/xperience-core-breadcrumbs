using Microsoft.Extensions.DependencyInjection;

namespace Xperience.Core.Breadcrumbs
{
    public static class IBreadcrumbsWidgetServiceExtensions
    {
        public static void AddBreadcrumbs(this IServiceCollection services, BreadcrumbsWidgetProperties props)
        {
            services.AddSingleton<BreadcrumbHelper>();
            services.AddSingleton(props);
        }
    }
}
