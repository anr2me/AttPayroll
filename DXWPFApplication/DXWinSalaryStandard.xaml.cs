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
using Core.DomainModel;
using DevExpress.Xpf.Core;
using Newtonsoft.Json;


namespace DXWPFApplication
{
    /// <summary>
    /// Interaction logic for DXWindow1.xaml
    /// </summary>
    public partial class DXWinSalaryStandard : DXWindow
    {
        public DXWinSalaryStandard()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = App.Current.MainWindow as MainWindow;

            InsertUpdateData obj = new InsertUpdateData()
            {
                Id = this.ID.Text,
                TitleInfoId = ((int)this.Title.Tag).ToString(),
                Description = this.Description.Text,
                EffectiveDate = this.EffectiveDate.SelectedDate.GetValueOrDefault().ToShortDateString(),
            };
            string content = JsonConvert.SerializeObject(obj);
            //string content = String.Format(@"Id={0}&TitleInfoId={1}&Description={2}&EffectiveDate={3}", int.Parse(this.ID.Text), (int)this.Title.Tag, this.Description.Text, "\\/Date(" +this.EffectiveDate.SelectedDate.GetValueOrDefault().ToBinary()+")\\/");
            string uri = String.Format("http://{0}/payroll/SalaryStandard/{1}", mw.Host, (int.Parse(this.ID.Text) > 0) ? "Update" : "Insert");
            HttpWebRequest req = mw.ReqMan.GeneratePOSTRequest(uri, content, null, null, false, mw.proxy);
            HttpWebResponse resp = mw.ReqMan.GetResponse(req);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string respcont = mw.ReqMan.GetResponseContent(resp);
                dynamic model = JsonConvert.DeserializeObject(respcont);
                if (model != null)
                {
                    if (model.Errors.Count == 0)
                    {
                        this.DialogResult = true;
                    }
                    else
                    {
                        var error = model.Errors.First;

                    }
                }
            }
            else if (resp.StatusCode == HttpStatusCode.Found)
            {
                mw.Login();
            }
        }


    }
}
