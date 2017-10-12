using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrowserHelper
{
    public class BrowserHelper
    {
        #region static

        private static Dictionary<ElementProperty, PropertyInfo> _propertyCache;

        static BrowserHelper()
        {
            _propertyCache = new Dictionary<ElementProperty, PropertyInfo>();

            var htmlElementType = typeof(HtmlElement);
            var enumNames = Enum.GetNames(typeof(ElementProperty));
            foreach (var name in enumNames)
            {
                var propertyInfo = htmlElementType.GetProperty(name);
                if (Enum.TryParse(name, out ElementProperty result))
                    _propertyCache.Add(result, propertyInfo);
            }
        }

        #endregion static

        private WebBrowser _browser;

        /// <summary>
        /// Instantiates the BrowserHelper class.
        /// </summary>
        /// <param name="browser">Requires a WebBrowser element already created.</param>
        public BrowserHelper(WebBrowser browser)
        {
            _browser = browser;
        }

        private async Task PageLoaded(int TimeOut)
        {
            TaskCompletionSource<bool> PageLoaded = null;
            PageLoaded = new TaskCompletionSource<bool>();
            int TimeElapsed = 0;
            _browser.DocumentCompleted += (s, e) =>
            {
                if (_browser.ReadyState != WebBrowserReadyState.Complete) return;
                if (PageLoaded.Task.IsCompleted) return; PageLoaded.SetResult(true);
            };
            //
            while (PageLoaded.Task.Status != TaskStatus.RanToCompletion)
            {
                await Task.Delay(10);//interval of 10 ms worked good for me
                TimeElapsed++;
                if (TimeElapsed >= TimeOut * 100) PageLoaded.TrySetResult(true);
            }
        }

        /// <summary>
        /// Makes the browser go the the specified link
        /// </summary>
        /// <param name="url">Link to go</param>
        public async Task Navigate(string url, int timeout = 10)
        {         
            _browser.Navigate(url);
            await PageLoaded(timeout);
        }

        /// <summary>
        /// Gets an element based on the ID of an HTML tag (&lt;h1 ID = 'ThisID'&gt;XXX&lt;/h1&gt;)
        /// </summary>
        /// <param name="element">The ID to search for</param>
        /// <returns>An HtmlElement object</returns>
        public HtmlElement GetElementByID(string element)
        {
            return _browser.Document.GetElementById(element);
        }

        /// <summary>
        /// Gets collection of elements based on the specified HTML tag (&lt;this&gt;&lt;/this&gt;)
        /// </summary>
        /// <param name="tag">The HTML tag to search for</param>
        /// <returns>A collection of HtmlElement</returns>
        public HtmlElementCollection GetElementsByTag(string tag)
        {
            return _browser.Document.GetElementsByTagName(tag);
        }

        /// <summary>
        /// Sets any HTML element property in the HTML code by the ID of the tag
        /// </summary>
        /// <param name="ID">The ID of the HTML tag (&lt;h1 ID = 'ThisID'&gt;XXX&lt;/h1&gt;)</param>
        /// <param name="property">The property that will be set</param>
        /// <param name="data">The data that will be set on the property specified</param>
        /// <returns>True if the property was found and set</returns>
        public bool SetElementByID(string ID, ElementProperty property, string data)
        {
            var element = GetElementByID(ID);

            if (_propertyCache.TryGetValue(property, out PropertyInfo propertyInfo))
            {
                propertyInfo.SetValue(element, data);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clicks a button by its text
        /// </summary>
        /// <param name="buttonText">Text in the button to click</param>
        public void ClickButton(string buttonText)
        {
            foreach (HtmlElement element in GetElementsByTag("button"))
            {
                if (element.InnerText == buttonText)
                {
                    element.InvokeMember("click");
                }
            }
        }

        /// <summary>
        /// Clicks a link by its text
        /// </summary>
        /// <param name="linkText">Text inside the &lt;A&gt;&lt;/A&gt; tag (&lt;A HREF = "http://domain.com"&gt;THIS TEXT&lt;/A&gt;</param>
        public void ClickLink(string linkText)
        {
            HtmlElementCollection links = _browser.Document.GetElementsByTagName("A");

            foreach (HtmlElement link in links)
            {
                if (link.InnerText == linkText)
                {
                    link.InvokeMember("Click");
                }
            }
        }

        /// <summary>
        /// Clicks an image by the image name
        /// </summary>
        /// <param name="imgName">Image name to click</param>
        public void ClickImage(string imgName)
        {
            HtmlElementCollection links = _browser.Document.GetElementsByTagName("img");

            foreach (HtmlElement link in links)
            {
                if (link.OuterHtml.Contains(imgName))
                {
                    link.InvokeMember("Click");
                }
            }
        }

        public void InvokeScript(string scriptName)
        {
            _browser.Document.InvokeScript(scriptName);
        }
    }
}