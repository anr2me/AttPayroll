using System;
using System.Collections.Generic;
using System.Linq;
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
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Layout.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Charts;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System.Net;
using Core.DomainModel;

namespace DXWPFApplication
{
    public partial class MainWindow : DXRibbonWindow
    {
        public string Host = "localhost:44350"; // use localhost instead of 127.0.0.1 to prevent error
        public WebProxy proxy = null; //new WebProxy("127.0.0.1", 8888);
        public WebFunctions.RequestManager ReqMan = new WebFunctions.RequestManager();
        //public DXWinLogin WinLogin = new DXWinLogin();
        //public DXWinSalaryStandard WinSalaryStandard = new DXWinSalaryStandard();
        //public DXWinLookUpTitleInfo WinLookUpTitleInfo = new DXWinLookUpTitleInfo();

        public bool? Login()
        {
            DXWinLogin WinLogin = new DXWinLogin();
            var result = WinLogin.ShowDialog();
            if (!result.GetValueOrDefault())
            {
                XtraMessageBox.Show("Login Failed", "Warning");
                this.Close();
            };
            return result;
        }

        public bool ReloadGrid()
        {
            //string content = String.Format(@"UserName={0}&Password={1}", WindowLogin.UserName.Text, WindowLogin.Password.Password);
            string uri = String.Format("http://{0}/payroll/SalaryStandard/GetListDynamic?ParentId={1}&_search={2}&nd={3}&rows={4}&page={5}&sidx={6}&sord={7}&filters={8}", Host, 0, false, DateTime.Now.ToBinary(), 20, 1, "id", "desc", "");
            HttpWebRequest req = ReqMan.GenerateGETRequest(uri, null, null, false, proxy);
           
            HttpWebResponse resp = ReqMan.GetResponse(req);
            //string respcont = ReqMan.GetResponseContent(resp);
            if (resp != null)
            {
                if (resp.StatusCode == HttpStatusCode.OK && resp.ContentType.ToLower().Contains("json"))
                {
                    string respcont = ReqMan.GetResponseContent(resp);
                    dynamic model = JsonConvert.DeserializeObject(respcont);
                    gridControl1.ItemsSource = null;
                    gridControl1.ItemsSource = model.rows; //jtoken.Last.Last.ToList();
                }
                else if (resp.StatusCode == HttpStatusCode.Found)
                {
                    Login();
                }
            }
            return true;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataSource();
            App.Current.MainWindow = this;
            //WinLogin.SetParent(Window.GetWindow(this));

            Login();
        }

        private void DXRibbonWindow_Activated(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        private void TableView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

        }

        private void TableView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DXWinSalaryStandard WinSalaryStandard = new DXWinSalaryStandard();

            string uri = String.Format("http://{0}/payroll/SalaryStandard/GetInfo?Id={1}", Host, gridControl1.View.FocusedRowData.CellData[0].Value);
            HttpWebRequest req = ReqMan.GenerateGETRequest(uri, null, null, false, proxy);
            HttpWebResponse resp = ReqMan.GetResponse(req);
            if (resp != null)
            {
                if (resp.StatusCode == HttpStatusCode.OK && resp.ContentType.ToLower().Contains("json"))
                {
                    string respcont = ReqMan.GetResponseContent(resp);
                    //var jtoken = Newtonsoft.Json.Linq.JContainer.FromObject(JsonConvert.DeserializeObject(respcont));
                    //dynamic model = Newtonsoft.Json.Linq.JObject.Parse(respcont); //.FromObject(JsonConvert.DeserializeObject(respcont));
                    var model = JsonConvert.DeserializeObject<GetInfoData>(respcont);
                    WinSalaryStandard.ID.Text = model.Id.ToString();
                    WinSalaryStandard.Title.Text = model.TitleInfoName;
                    WinSalaryStandard.Title.Tag = model.TitleInfoId;
                    WinSalaryStandard.Description.Text = model.Description;
                    WinSalaryStandard.EffectiveDate.SelectedDate = model.EffectiveDate;
                    if (WinSalaryStandard.ShowDialog().GetValueOrDefault())
                    // Submitted
                    {

                    }
                }
                else if (resp.StatusCode == HttpStatusCode.Found)
                {
                    Login();
                }
            }
            
        }

    }

    public class TextSplitConverter : IValueConverter
    {
        public int SplitterPosition { get; set; }
        public bool IsRightSide { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
            {
                if (IsRightSide)
                {
                    return (value as String).Substring(SplitterPosition);
                }
                return (value as String).Substring(0, SplitterPosition);
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class InsertUpdateData
    {
        public string Id { get; set; }
        public string TitleInfoId { get; set; }
        public string EffectiveDate { get; set; }
        public string Description { get; set; }
    }

    public class GetInfoData
    {
        public int Id { get; set; }
        public int TitleInfoId { get; set; }
        public string TitleInfoName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Description { get; set; }
        public Dictionary<string,string> Errors { get; set; }
    }

    public class GetListData
    {
        public int Id { get; set; }
        public int TitleInfoId { get; set; }
        public string TitleInfoName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }

    public class GetListDataViewModel : INotifyPropertyChanged
    {
        GetListData data;
        public GetListDataViewModel()
        {
            data = new GetListData() { TitleInfoName = string.Empty, EffectiveDate = DateTime.Now, CreatedAt = DateTime.Today };
        }
        public string TitleInfoName
        {
            get { return Data.TitleInfoName; }
            set
            {
                if (Data.TitleInfoName == value)
                    return;
                Data.TitleInfoName = value;
                //RaisePropertyChanged("TitleInfoName");
            }
        }
        public int TitleInfoId
        {
            get { return Data.TitleInfoId; }
            set
            {
                if (Data.TitleInfoId == value)
                    return;
                Data.TitleInfoId = value;
                RaisePropertyChanged("TitleInfoId");
            }
        }
        public DateTime EffectiveDate
        {
            get { return Data.EffectiveDate; }
            set
            {
                if (Data.EffectiveDate == value)
                    return;
                Data.EffectiveDate = value;
                RaisePropertyChanged("EffectiveDate");
            }
        }
        public int Id
        {
            get { return Data.Id; }
            set
            {
                if (Data.Id == value)
                    return;
                Data.Id = value;
                //RaisePropertyChanged("Id");
            }
        }
        public DateTime CreatedAt
        {
            get { return Data.CreatedAt; }
            set
            {
                if (Data.CreatedAt == value)
                    return;
                Data.CreatedAt = value;
                //RaisePropertyChanged("CreatedAt");
            }
        }
        public Nullable<DateTime> UpdatedAt
        {
            get { return Data.UpdatedAt; }
            set
            {
                if (Data.UpdatedAt == value)
                    return;
                Data.UpdatedAt = value;
                //RaisePropertyChanged("UpdatedAt");
            }
        }
        protected GetListData Data
        {
            get { return data; }
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class DataSource
    {
        ObservableCollection<GetListDataViewModel> source; // <GetListDataViewModel>
        public DataSource()
        {
            source = CreateDataSource();
        }
        protected ObservableCollection<GetListDataViewModel> CreateDataSource()
        {
            ObservableCollection<GetListDataViewModel> res = new ObservableCollection<GetListDataViewModel>();
            res.Add(new GetListDataViewModel() { TitleInfoId = 0, TitleInfoName = "Row0", EffectiveDate = DateTime.Now, Id = 0, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 1, TitleInfoName = "Row1", EffectiveDate = DateTime.Now, Id = 1, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 2, TitleInfoName = "Row2", EffectiveDate = DateTime.Now, Id = 2, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 3, TitleInfoName = "Row3", EffectiveDate = DateTime.Now, Id = 3, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 4, TitleInfoName = "Row4", EffectiveDate = DateTime.Now, Id = 4, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 5, TitleInfoName = "Row5", EffectiveDate = DateTime.Now, Id = 5, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 6, TitleInfoName = "Row6", EffectiveDate = DateTime.Now, Id = 6, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 7, TitleInfoName = "Row7", EffectiveDate = DateTime.Now, Id = 7, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 8, TitleInfoName = "Row8", EffectiveDate = DateTime.Now, Id = 8, CreatedAt = DateTime.Today });
            res.Add(new GetListDataViewModel() { TitleInfoId = 9, TitleInfoName = "Row9", EffectiveDate = DateTime.Now, Id = 9, CreatedAt = DateTime.Today });
            return res;
        }
        public ObservableCollection<GetListDataViewModel> Data { get { return source; } }
    }
}
