using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using Newtonsoft.Json;


namespace DXWPFApplication
{
    /// <summary>
    /// Interaction logic for DXWindowLogin.xaml
    /// </summary>
    public partial class DXWinLogin : DXWindow
    {
        public DXWinLogin()
        {
            InitializeComponent();
        }

        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            //public bool RememberMe { get; set; }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.MainWindow as MainWindow;

            //LoginModel model = new LoginModel()
            //{
            //    UserName = this.UserName.Text,
            //    Password = this.Password.Password,
            //};
            //string content = JsonConvert.SerializeObject(model);
            string content = String.Format(@"UserName={0}&Password={1}", this.UserName.Text, this.Password.Password);
            string uri = String.Format("http://{0}/Authentication/Login", mw.Host);
            HttpWebRequest req = mw.ReqMan.GenerateGETRequest(uri, null, null, false, mw.proxy, null); // need to GET first before POST to get sessionid required for login
            HttpWebResponse resp = mw.ReqMan.GetResponse(req);
            req = mw.ReqMan.GeneratePOSTRequest(uri, content, null, null, false, mw.proxy, null);
            resp = mw.ReqMan.GetResponse(req);
            string respcont = mw.ReqMan.GetResponseContent(resp);
            if (resp != null)
            {
                if (resp.StatusCode == HttpStatusCode.Found)
                {
                    this.Label1.Content = "";
                    this.DialogResult = true;
                    //this.Close();
                }
                else
                {
                    this.Label1.Content = "Invalid credential";
                }
            }
            else
            {
                this.Label1.Content = "No Response";
            }
        }

        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Label1.Content = "";
            this.UserName.SelectAll();
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Label1.Content = "";
            this.Password.SelectAll();
        }

        private void DXWindow_Activated(object sender, EventArgs e)
        {
            this.UserName.Focus();
        }
        
    }
}
