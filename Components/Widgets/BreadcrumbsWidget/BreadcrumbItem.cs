using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xperience.Components.Widgets.BreadcrumbsWidget
{
    public class BreadcrumbItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsCurrentPage { get; set; } = false;

        public BreadcrumbItem()
        {

        }
    }
}
