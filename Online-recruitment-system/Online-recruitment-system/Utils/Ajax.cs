using Microsoft.AspNetCore.Http;
using System;

namespace Online_recruitment_system
{
    static public class Ajax
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.Headers["X-Requested-With"] == "XMLHttpRequest") return true;

            if (request.Headers != null) return request.Headers["X-Requested-With"] == "XMLRequest";
            return false;
        }
    }
}
