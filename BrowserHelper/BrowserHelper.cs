using System.Windows.Forms;

namespace BrowserHelper
{
    public class BrowserHelper
    {
        private WebBrowser _navegador;

        public BrowserHelper(WebBrowser navegador)
        {
            _navegador = navegador;
        }

        public void GoTo(string url)
        {
            _navegador.Navigate(url);
        }

        public HtmlElement GetElementByID(string element)
        {
            return _navegador.Document.GetElementById(element);
        }

        public HtmlElementCollection GetElementsByTag(string tag)
        {
            return _navegador.Document.GetElementsByTagName(tag);
        }

        public void SetElementByID(string element, string data)
        {
            HtmlElement elemento = GetElementByID(element);
            elemento.InnerText = data;
        }

        public void ClickButton(string name)
        {
            foreach (HtmlElement elemento in _navegador.Document.GetElementsByTagName("input"))
            {
                if (elemento.Name.ToLower() == name)
                {
                    elemento.InvokeMember("click");
                }
            }
        }

        public void ClickLink(string linkText)
        {
            HtmlElementCollection links = _navegador.Document.GetElementsByTagName("A");

            foreach (HtmlElement link in links)
            {
                if (link.InnerText == linkText)
                {
                    link.InvokeMember("Click");
                }
            }
        }

        public void ClickImage(string imgName)
        {
            HtmlElementCollection links = _navegador.Document.GetElementsByTagName("img");

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
            _navegador.Document.InvokeScript(scriptName);
        }
    }
}
