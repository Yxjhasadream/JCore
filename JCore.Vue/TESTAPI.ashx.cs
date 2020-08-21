using System.Web;

namespace JCore.Vue
{
    /// <summary>
    /// Summary description for TESTAPI
    /// </summary>
    public class TESTAPI : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}