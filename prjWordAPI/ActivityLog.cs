using Microsoft.AspNetCore.Mvc.Filters;

namespace prjWordAPI
{
    public class ActivityLog : ActionFilterAttribute
    {
        private ILogUserActivityService _service;
    }
}
