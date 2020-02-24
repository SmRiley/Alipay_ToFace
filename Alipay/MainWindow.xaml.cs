using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;
using ThoughtWorks.QRCode.Codec;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Aop.Api;
using Aop.Api.Request;
using System.Windows.Media.Imaging;

namespace Alipay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : System.Windows.Window
    {
        static string APPID, Public_Key, Private_Key;
        DefaultAopClient AliPayClient;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MsBoxShow(string Msg) {
            HandyControl.Controls.MessageBox.Show(Msg);
        }
        private void MsBoxShow(string Msg, string Title)
        {
            HandyControl.Controls.MessageBox.Show(Msg, Title);
        }

        private void Draw_Form(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }



        private void Minimize_Form(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Form(object sender, RoutedEventArgs e)
        {
            Close();
        }

        struct QueryContent
        {
            public QueryContent(string Out_Trade_no, string Trade_no) {
                out_trade_no = Out_Trade_no;
                trade_no = Trade_no;
            }
            public string out_trade_no;
            public string trade_no;
        }


        private void Query_Oder(object sender, RoutedEventArgs e)
        {

            if (AliPayClient != null)
            {
                if (Order_Num.Text != "")
                {
                    var Query_Request = new AlipayTradeQueryRequest();
                    var BizContent = new QueryContent(Order_Num.Text, "");
                    Query_Request.BizContent = JsonConvert.SerializeObject(BizContent);
                    var Query_Response = AliPayClient.Execute(Query_Request);

                    if (Query_Response.Code == "10000")
                    {
                        //var JsonObj = JObject.Parse(Query_Response.Body);
                        Pay_State.Content = $"已支付:{Query_Response.TotalAmount}元";
                    }
                    else {
                        Pay_State.Content = "等待付款";
                    }
                }
                else {
                    MsBoxShow("订单号不能为空");
                }
            }
            else {
                MsBoxShow("请先填写相关API");
            }
        }

        private void Setting_Alipay(object sender, RoutedEventArgs e)
        {
            APPID = APPID_Text.Text;
            Public_Key = Public_Key_Text.Text;
            Private_Key = Private_Key_Text.Text;
            Expander_Setting.IsExpanded = false;
            AliPayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, Private_Key, "json", "1.0", "RSA2", Public_Key, "utf-8");
        }

        struct PrecreatReContent{
            public string out_trade_no, total_amount, subject, timeout_express;
            public PrecreatReContent(int Trade_ID,string Total_amount,string Subject,string Timeout_express) {
                out_trade_no = Trade_ID.ToString();
                total_amount = Total_amount;
                subject = Subject;
                timeout_express = Timeout_express+"m";
            }
        }

        private void Start_Request(object sender, RoutedEventArgs e)
        {
            if (AliPayClient != null)
            {
                var request = new AlipayTradePrecreateRequest();
                var trade_ID = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
                if (Price.Text != "" && Operation.Text != "")
                {
                    request.BizContent = JsonConvert.SerializeObject(new PrecreatReContent(trade_ID, Price.Text, Operation.Text, Convert.ToInt32(QrTime_Setting.Value).ToString()));
                }
                else {
                    MsBoxShow("两项不能为空!");
                }
                

                var Alipay_Response = AliPayClient.Execute(request);
                var JsonObj = JObject.Parse(Alipay_Response.Body)["alipay_trade_precreate_response"];
                if (Alipay_Response.Code == "10000")
                {
                    var QR_String = JsonObj["qr_code"].ToString();
                    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                    qrCodeEncoder.QRCodeVersion = 0;
                    Bitmap img = qrCodeEncoder.Encode(QR_String, Encoding.UTF8);
                    BitmapImage bitmapimg = new BitmapImage();
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, ImageFormat.Bmp);
                        bitmapimg.BeginInit();
                        bitmapimg.StreamSource = ms;
                        bitmapimg.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapimg.EndInit();
                    }
                    Pay_State.Content = "已生成待支付";
                    QRImg.Source = bitmapimg;
                    Expander_Request.IsExpanded = false;
                    Order_Num.Text = trade_ID.ToString();
                }
                else
                {
                    MsBoxShow(Alipay_Response.Msg, $"错误代码:{Alipay_Response.Code}");
                }
            }
            else {
                MsBoxShow("请先填写相关API");
            }
        }
    }
}
