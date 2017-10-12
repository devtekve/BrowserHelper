using System.Windows.Forms;

namespace ConsoleApp1
{
    public partial class Form1 : Form
    {
        private BrowserHelper.BrowserHelper Browser;

        public Form1()
        {
            InitializeComponent();
            Browser = new BrowserHelper.BrowserHelper(webBrowser1);
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            await Browser.Navigate("https://www.jcplanet.com/Account/Login");            
            Browser.SetElementByID("Email", BrowserHelper.ElementProperty.InnerText, textBox1.Text);
            
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Browser.ClickButton("LOG IN");
        }
    }
}