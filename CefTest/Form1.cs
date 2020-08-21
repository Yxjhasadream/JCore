using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace Bats
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
            InitBrowser();
        }

        void test()
        {
            SystemSounds.Asterisk.Play();
        }

        public ChromiumWebBrowser browser;
        public void InitBrowser()
        {
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            Cef.Initialize(new CefSettings());
            Cef.EnableHighDPISupport();
            browser = new ChromiumWebBrowser("https://web.batchat.com");
            Controls.Add(browser);
            browser.Dock = DockStyle.Fill;


            Dictionary<string, string> FilterUrls = new Dictionary<string, string>()
            {
                {"mainPage","https://web.batchat.com" },
            };

            var MyRequestHandler = new MyRequestHandler(FilterUrls);
            browser.RequestHandler = MyRequestHandler;
        }

        #region 事件集合。

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            //任务栏区显示图标
            ShowInTaskbar = true;
            Activate();
        }

        private void 显示ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Show();
            Activate();
        }

        private void 退出ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                browser.Dispose();
                Dispose();
                Close();
            }
        }
        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, System.EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                ShowInTaskbar = true;
                //图标显示在托盘区
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            Hide();

            //图标显示在托盘区
            notifyIcon1.Visible = true;
            //取消"关闭窗口"事件
        }

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        #endregion
    }

    public enum HttpMethod
    {
        None,
        GET,
        POST,
        CONNECT,
        OPTIONS,
    }
    public class FilterData : IDisposable
    {
        public FilterData(string name, string url) { Name = name; Url = url; }
        public FilterData(string name, string url, HttpMethod _Method) { Name = name; Url = url; Method = _Method; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Name { get; set; }
        public string Url { get; set; }
        public HttpMethod Method { get; set; } = HttpMethod.GET;
        public bool HasData { get { return Filter?.Data?.Length > 0; } }
        public UInt64 Identifier { get; set; }
        public MyResponseFilter Filter { get; set; }
        public void AddFilter(UInt64 _Identifier, MyResponseFilter _Filter)
        {
            Filter?.Dispose(true);
            Identifier = _Identifier;
            Filter = _Filter;
        }

        bool Disposed = false;
        public void Dispose()
        {
            if (Disposed)
                return;
            Filter?.Dispose(true);
            Disposed = true;
        }
        public override string ToString()
        {
            return $"Name={Name},DataStr={Filter?.DataStr}";
        }
    }

    public class MyRequestHandler : IRequestHandler, IDisposable
    {
        public MyRequestHandler(string name, string _url, HttpMethod _Method = HttpMethod.GET, bool _filter = false)
        {
            Filters.Add(new FilterData(name, _url, _Method));
            filter = _filter;
        }

        public MyRequestHandler(Dictionary<string, string> _urls, HttpMethod _Method = HttpMethod.GET,
            bool _filter = false)
        {
            foreach (var u in _urls)
            {
                Filters.Add(new FilterData(u.Key, u.Value, _Method));
            }

            filter = _filter;
        }

        public MyRequestHandler(FilterData Filter, bool _filter = false)
        {
            Filters.Add(Filter);
            filter = _filter;
        }

        public MyRequestHandler(List<FilterData> _Filters, bool _filter = false)
        {
            Filters.AddRange(_Filters);
            filter = _filter;
        }

        bool Disposed = false;

        /// <summary>
        /// 是否过滤，如果不过滤就截获
        /// </summary>
        bool filter = false;

        /// <summary>
        /// 过滤、截获 url 列表 和 捕获过滤数据
        /// </summary>
        public List<FilterData> Filters = new List<FilterData>();
         

        /// <summary>
        /// Called on the CEF IO thread before a resource request is initiated.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control.</param>
        /// <param name="browser">represent the source browser of the request.</param>
        /// <param name="frame">represent the source frame of the request.</param>
        /// <param name="request">represents the request contents and cannot be modified in this callback.</param>
        /// <param name="isNavigation">will be true if the resource request is a navigation.</param>
        /// <param name="isDownload">will be true if the resource request is a download.</param>
        /// <param name="requestInitiator">is the origin (scheme + domain) of the page that initiated the request.</param>
        /// <param name="disableDefaultHandling">[in,out] to true to disable default handling of the request, in which case it will need
        /// to be handled via <see cref="IResourceRequestHandler.GetResourceHandler"/> or it will be canceled.</param>
        /// <returns>
        /// To allow the resource load to proceed with default handling return null. To specify a handler for the resource return a
        /// <see cref="IResourceRequestHandler"/> object. If this callback returns null the same method will be called on the associated
        /// <see cref="IRequestContextHandler"/>, if any.
        /// </returns>
        IResourceRequestHandler IRequestHandler.GetResourceRequestHandler(IWebBrowser chromiumWebBrowser,
            IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload,
            string requestInitiator, ref bool disableDefaultHandling)
        {

            return null;
        }

        /// <summary>
        /// Called before browser navigation. If the navigation is allowed <see cref="IWebBrowser.FrameLoadStart"/> and
        /// <see cref="IWebBrowser.FrameLoadEnd"/>
        /// will be called. If the navigation is canceled <see cref="IWebBrowser.LoadError"/> will be called with an ErrorCode value of
        /// <see cref="CefErrorCode.Aborted"/>.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        /// <param name="frame">The frame the request is coming from.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="userGesture">The value will be true if the browser navigated via explicit user gesture (e.g. clicking a link) or
        /// false if it navigated automatically (e.g. via the DomContentLoaded event).</param>
        /// <param name="isRedirect">has the request been redirected.</param>
        /// <returns>
        /// Return true to cancel the navigation or false to allow the navigation to proceed.
        /// </returns>
        bool IRequestHandler.OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
            IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }

        /// <summary>
        /// Called to handle requests for URLs with an invalid SSL certificate. Return true and call
        /// <see cref="IRequestCallback.Continue"/> either in this method or at a later time to continue or cancel the request.  
        /// If CefSettings.IgnoreCertificateErrors is set all invalid certificates will be accepted without calling this method.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        /// <param name="errorCode">the error code for this invalid certificate.</param>
        /// <param name="requestUrl">the url of the request for the invalid certificate.</param>
        /// <param name="sslInfo">ssl certificate information.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests. If empty the error cannot be
        /// recovered from and the request will be canceled automatically.</param>
        /// <returns>
        /// Return false to cancel the request immediately. Return true and use <see cref="IRequestCallback"/> to execute in an async
        /// fashion.
        /// </returns>
        bool IRequestHandler.OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser,
            CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            //NOTE: We also suggest you wrap callback in a using statement or explicitly execute callback.Dispose as callback wraps an unmanaged resource.

            //Example #1
            //Return true and call IRequestCallback.Continue() at a later time to continue or cancel the request.
            //In this instance we'll use a Task, typically you'd invoke a call to the UI Thread and display a Dialog to the user
            //You can cast the IWebBrowser param to ChromiumWebBrowser to easily access
            //control, from there you can invoke onto the UI thread, should be in an async fashion

            //Task.Run(() =>
            //{
            //    //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            //    if (!callback.IsDisposed)
            //        {
            //        using (callback)
            //            {
            //            //We'll allow the expired certificate from badssl.com
            //            if (requestUrl.ToLower().Contains("https://expired.badssl.com/"))
            //                {
            //                callback.Continue(true);
            //                }
            //            else
            //                {
            //                callback.Continue(false);
            //                }
            //            }
            //        }
            //});

            return true;

            //Example #2
            //Execute the callback and return true to immediately allow the invalid certificate
            //callback.Continue(true); //Callback will Dispose it's self once exeucted
            //return true;

            //Example #3
            //Return false for the default behaviour (cancel request immediately)
            //callback.Dispose(); //Dispose of callback
            //return false;
        }

        /// <summary>
        /// Called on the CEF UI thread when the window.document object of the main frame has been created.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        void IRequestHandler.OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
        }

        /// <summary>
        /// Called on the UI thread before OnBeforeBrowse in certain limited cases where navigating a new or different browser might be
        /// desirable. This includes user-initiated navigation that might open in a special way (e.g. links clicked via middle-click or
        /// ctrl + left-click) and certain types of cross-origin navigation initiated from the renderer process (e.g. navigating the top-
        /// level frame to/from a file URL).
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        /// <param name="frame">The frame object.</param>
        /// <param name="targetUrl">target url.</param>
        /// <param name="targetDisposition">The value indicates where the user intended to navigate the browser based on standard
        /// Chromium behaviors (e.g. current tab, new tab, etc).</param>
        /// <param name="userGesture">The value will be true if the browser navigated via explicit user gesture (e.g. clicking a link) or
        /// false if it navigated automatically (e.g. via the DomContentLoaded event).</param>
        /// <returns>
        /// Return true to cancel the navigation or false to allow the navigation to proceed in the source browser's top-level frame.
        /// </returns>
        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
            string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        /// <summary>
        /// Called when a plugin has crashed.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        /// <param name="pluginPath">path of the plugin that crashed.</param>
        void IRequestHandler.OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        /// <summary>
        /// Called when JavaScript requests a specific storage quota size via the webkitStorageInfo.requestQuota function. For async
        /// processing return true and execute <see cref="IRequestCallback.Continue"/> at a later time to grant or deny the request or
        /// <see cref="IRequestCallback.Cancel"/> to cancel.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        /// <param name="originUrl">the origin of the page making the request.</param>
        /// <param name="newSize">is the requested quota size in bytes.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>
        /// Return false to cancel the request immediately. Return true to continue the request and call
        /// <see cref="IRequestCallback.Continue"/> either in this method or at a later time to grant or deny the request.
        /// </returns>
        bool IRequestHandler.OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl,
            long newSize, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            //if (!callback.IsDisposed)
            //    {
            //    using (callback)
            //        {
            //        //Accept Request to raise Quota
            //        //callback.Continue(true);
            //        //return true;
            //        }
            //    }

            return false;
        }

        /// <summary>
        /// Called when the render process terminates unexpectedly.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        /// <param name="status">indicates how the process terminated.</param>
        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser,
            CefTerminationStatus status)
        {
            // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
            //chromiumWebBrowser.Load(CefExample.RenderProcessCrashedUrl);
        }

        /// <summary>
        /// Called on the CEF UI thread when the render view associated with browser is ready to receive/handle IPC messages in the
        /// render process.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        void IRequestHandler.OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
        }

        /// <summary>
        /// Called when the browser needs user to select Client Certificate for authentication requests (eg. PKI authentication).
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object.</param>
        /// <param name="isProxy">indicates whether the host is a proxy server.</param>
        /// <param name="host">hostname.</param>
        /// <param name="port">port number.</param>
        /// <param name="certificates">List of Client certificates for selection.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of client certificate selection for
        /// authentication requests.</param>
        /// <returns>
        /// Return true to continue the request and call ISelectClientCertificateCallback.Select() with the selected certificate for
        /// authentication. Return false to use the default behavior where the browser selects the first certificate from the list.
        /// 
        /// </returns>
        bool IRequestHandler.OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy,
            string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            //callback.Dispose();
            return false;
        }

        public override string ToString()
        {
            if (!Filters?.Any() ?? true)
                return string.Empty;
            return string.Join("\r\n", Filters.Select(f => f.ToString()));
        }

        public void Dispose()
        {
            if (Disposed)
                return;
            Filters?.ForEach(f => f.Dispose());
            Filters?.Clear();
            Filters = null;
            Disposed = true;
        }

        public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            throw new NotImplementedException();
        }
    }

    class MyResourceRequestHandler : IResourceRequestHandler
    {
        public MyResourceRequestHandler(FilterData Filter, bool _filter)
        {
            filterData = Filter;
            filter = _filter;
        }

        bool Disposed = false;
        /// <summary>
        /// 是否过滤，如果不过滤就截获
        /// </summary>
        bool filter = false;
        /// <summary>
        /// 过滤、截获 url 列表 和 捕获过滤数据
        /// </summary>
        FilterData filterData { get; set; }


        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded. To optionally filter cookies for the request return a
        /// <see cref="ICookieAccessFilter"/> object.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <returns>To optionally filter cookies for the request return a ICookieAccessFilter instance otherwise return null.</returns>
        ICookieAccessFilter IResourceRequestHandler.GetCookieAccessFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return null;
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource is loaded. To specify a handler for the resource return a
        /// <see cref="IResourceHandler"/> object.
        /// </summary>
        /// <param name="chromiumWebBrowser">The browser UI control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <returns>
        /// To allow the resource to load using the default network loader return null otherwise return an instance of
        /// <see cref="IResourceHandler"/> with a valid stream.
        /// </returns>
        IResourceHandler IResourceRequestHandler.GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return null;
        }

        /// <summary>Called on the CEF IO thread to optionally filter resource response content.</summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <returns>Return an IResponseFilter to intercept this response, otherwise return null.</returns>
        IResponseFilter IResourceRequestHandler.GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var fil = new MyResponseFilter(filter);
            filterData.AddFilter(request.Identifier, fil);
            return fil;
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded. To redirect or change the resource load optionally modify
        /// <paramref name="request"/>. Modification of the request URL will be treated as a redirect.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>
        /// Return <see cref="CefReturnValue.Continue"/> to continue the request immediately. Return
        /// <see cref="CefReturnValue.ContinueAsync"/> and call <see cref="IRequestCallback.Continue"/> or
        /// <see cref="IRequestCallback.Cancel"/> at a later time to continue or the cancel the request asynchronously. Return
        /// <see cref="CefReturnValue.Cancel"/> to cancel the request immediately.
        /// </returns>
        CefReturnValue IResourceRequestHandler.OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            //Uri url;
            //if (Uri.TryCreate(request.Url, UriKind.Absolute, out url) == false)
            //    {
            //    //If we're unable to parse the Uri then cancel the request
            //    // avoid throwing any exceptions here as we're being called by unmanaged code
            //    return CefReturnValue.Cancel;
            //    }

            //Example of how to set Referer
            // Same should work when setting any header

            // For this example only set Referer when using our custom scheme
            //if (url.Scheme == CefSharpSchemeHandlerFactory.SchemeName)
            //    {
            //    //Referrer is now set using it's own method (was previously set in headers before)
            //    request.SetReferrer("http://google.com", ReferrerPolicy.Default);
            //    }

            //Example of setting User-Agent in every request.
            //var headers = request.Headers;

            //var userAgent = headers["User-Agent"];
            //headers["User-Agent"] = userAgent + " CefSharp";

            //request.Headers = headers;

            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            //if (!callback.IsDisposed)
            //    {
            //    using (callback)
            //        {
            //        if (request.Method == "POST")
            //            {
            //            using (var postData = request.PostData)
            //                {
            //                if (postData != null)
            //                    {
            //                    var elements = postData.Elements;

            //                    var charSet = request.GetCharSet();

            //                    foreach (var element in elements)
            //                        {
            //                        if (element.Type == PostDataElementType.Bytes)
            //                            {
            //                            var body = element.GetBody(charSet);
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //        //Note to Redirect simply set the request Url
            //        //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase))
            //        //{
            //        //    request.Url = "https://github.com/";
            //        //}

            //        //Callback in async fashion
            //        //callback.Continue(true);
            //        //return CefReturnValue.ContinueAsync;
            //        }
            //    }

            return CefReturnValue.Continue;
        }

        /// <summary>
        /// Called on the CEF UI thread to handle requests for URLs with an unknown protocol component. SECURITY WARNING: YOU SHOULD USE
        /// THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <returns>
        /// return to true to attempt execution via the registered OS protocol handler, if any. Otherwise return false.
        /// </returns>
        bool IResourceRequestHandler.OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {

            //return request.Url.StartsWith("mailto");
            return false;
        }

        /// <summary>
        /// Called on the CEF IO thread when a resource load has completed. This method will be called for all requests, including
        /// requests that are aborted due to CEF shutdown or destruction of the associated browser. In cases where the associated browser
        /// is destroyed this callback may arrive after the <see cref="ILifeSpanHandler.OnBeforeClose"/> callback for that browser. The
        /// <see cref="IFrame.IsValid"/> method can be used to test for this situation, and care
        /// should be taken not to call <paramref name="browser"/> or <paramref name="frame"/> methods that modify state (like LoadURL,
        /// SendProcessMessage, etc.) if the frame is invalid.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <param name="status">indicates the load completion status.</param>
        /// <param name="receivedContentLength">is the number of response bytes actually read.</param>
        void IResourceRequestHandler.OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            //var url = new Uri(request.Url);
            //if (url.Scheme == CefSharpSchemeHandlerFactory.SchemeName && memoryStream != null)
            //    {
            //    //TODO: Do something with the data here
            //    var data = memoryStream.ToArray();
            //    var dataLength = data.Length;
            //    //NOTE: You may need to use a different encoding depending on the request
            //    var dataAsUtf8String = Encoding.UTF8.GetString(data);
            //    }
        }

        /// <summary>
        /// Called on the CEF IO thread when a resource load is redirected. The <paramref name="request"/> parameter will contain the old
        /// URL and other request-related information. The <paramref name="response"/> parameter will contain the response that resulted
        /// in the redirect. The <paramref name="newUrl"/> parameter will contain the new URL and can be changed if desired.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <param name="newUrl">[in,out] the new URL and can be changed if desired.</param>
        void IResourceRequestHandler.OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            //Example of how to redirect - need to check `newUrl` in the second pass
            //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase) && !newUrl.Contains("github"))
            //{
            //    newUrl = "https://github.com";
            //}
        }

        /// <summary>
        /// Called on the CEF IO thread when a resource response is received. To allow the resource load to proceed without modification
        /// return false. To redirect or retry the resource load optionally modify <paramref name="request"/> and return true.
        /// Modification of the request URL will be treated as a redirect. Requests handled using the default network loader cannot be
        /// redirected in this callback.
        /// 
        /// WARNING: Redirecting using this method is deprecated. Use OnBeforeResourceLoad or GetResourceHandler to perform redirects.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <returns>
        /// To allow the resource load to proceed without modification return false. To redirect or retry the resource load optionally
        /// modify <paramref name="request"/> and return true. Modification of the request URL will be treated as a redirect. Requests
        /// handled using the default network loader cannot be redirected in this callback.
        /// </returns>
        bool IResourceRequestHandler.OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            //NOTE: You cannot modify the response, only the request
            // You can now access the headers
            //var headers = response.Headers;

            return false;
        }

        public void Dispose()
        {
            if (Disposed)
                return;
            Disposed = true;
        }
    }
    
    public class MyResponseFilter : IResponseFilter
    {
        public MyResponseFilter(bool _filter) { filter = _filter; }

        bool Disposed = false;
        /// <summary>
        /// 是否过滤，如果不过滤就截获
        /// </summary>
        bool filter = false;
        private MemoryStream memoryStream;
        public void Dispose()
        {
            //memoryStream?.Dispose();
            //memoryStream = null;
        }
        public void Dispose(bool r)
        {
            if (Disposed)
                return;
            if (r)
            {
                memoryStream?.Dispose();
                memoryStream = null;
            }
            Disposed = true;
        }

        public FilterStatus Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            if (dataIn == null)
            {
                dataInRead = 0;
                dataOutWritten = 0;

                return FilterStatus.Done;
            }

            dataInRead = dataIn.Length;
            dataOutWritten = Math.Min(dataInRead, dataOut.Length);

            //Important we copy dataIn to dataOut
            dataIn.CopyTo(dataOut);

            //Copy data to stream
            dataIn.Position = 0;
            dataIn.CopyTo(memoryStream);

            return FilterStatus.Done;
        }

        public bool InitFilter()
        {
            memoryStream = new MemoryStream();

            return true;
        }
        public byte[] Data
        {
            get { return memoryStream.ToArray(); }
        }

        public string DataStr
        {
            get { return Encoding.UTF8.GetString(memoryStream.ToArray()); }
        }
    }

}
