
namespace WEB_153503_DAINOVICH.Extensions
{
    public static class HttpRequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            var req = request.Headers["X-Requested-With"].ToString();
            return req.ToLower().Equals("xmlhttprequest");
        }
    }
}
