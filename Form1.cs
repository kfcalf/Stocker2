using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Ribbon;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

namespace Stocker2
{
    public partial class Form1 : KryptonForm
    {
        public Form1()
        {
            InitializeComponent();
            //textBox1.TextBox.DoubleClick += new System.EventHandler(textBox1_DoubleClick);
            //ListBox1.DoubleClick += new EventHandler(ListBox1_DoubleClick);
            //klbLeft.DoubleClick += new EventHandler(klbLeft_DoubleClick);
            InitializeAsync();

            //cWebB.LoadingStateChanged += OnLoadingStateChanged;
           
        }

        string ipInfo = "";
        string webA = "";
        string webB = "";
        string webHome = "http://mynav.ccccocccc.cc/index.html";
        async void InitializeAsync()
        {
            //浏览器控件此处要设置一下，不然不会显示
            await webView.EnsureCoreWebView2Async(null);
            await webViewB.EnsureCoreWebView2Async(null);
            await webViewAI.EnsureCoreWebView2Async(null);

            webViewB.CoreWebView2.WebMessageReceived += UpdateAddressBar;
            //webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;


            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
            //if (webView != null && webView.CoreWebView2 != null)
            //{
            //    webView.CoreWebView2.Navigate("http://quote.eastmoney.com/center/qqzs.html");
            //}
            getStockHome();//打开主页
            getTTS();
            kBtnHomePage.PerformClick();




        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            //kTxtUrl.Text = uri;
            webViewB.CoreWebView2.PostWebMessageAsString(uri);


        }

        //bmk Load
        private void Form1_Load(object sender, EventArgs e)
        {
            webHome = Properties.Settings.Default.webhome;
            kCbbUrl.Text = Properties.Settings.Default.webhome;
            this.Icon = Properties.Resources.newIcon;
            CheckForIllegalCrossThreadCalls=false;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

            this.PaletteMode = PaletteMode.Office2007Blue;
            cbbNum.Items.Clear();
            cbbPer.Items.Clear();

            for (int i = 1; i < 11; i++)
            {
                cbbNum.Items.Add(i);
                cbbPer.Items.Add(i);
            }
            for (int i = -10; i < 0; i++)
            {
                cbbPer.Items.Add(i);
            }

            cbbNum.SelectedIndex = 0;
            cbbPer.SelectedIndex = 9;

            ckbTop.Checked = this.TopMost;

             DocList();
             


            foreach (FontFamily font in FontFamily.Families)
            {
                kryFontName.Items.Add(font.Name.ToString());
            }

            kRtb.AutoWordSelection = true;
            kRtb.AutoWordSelection = false;
            kbtnRead.PerformClick();

            loadRtfList();

            

        }

        private void rbOffice2007Blue_CheckedChanged(object sender, EventArgs e)
        {
            //kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2007Blue;
        }

        private void rbOffice2010Blue_CheckedChanged(object sender, EventArgs e)
        {
            //kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2010Blue;
        }

        private void kryptonRibbonGroupButton1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Very Good!");
            //MessageBox.Show("更新完成，请重新启动程序！", "更新提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //MessageBox.Show("更新完成，请重新启动程序！", "更新提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory());


        }



        private void btnJun_Click(object sender, EventArgs e)
        {



        }


        private void kryptonButton1_Click_2(object sender, EventArgs e)
        {
        }

        private void btnJun_Click_2(object sender, EventArgs e)
        {
            try
            {
                double nCB = Convert.ToDouble(txtXGCB.Text);
                double nSL = Convert.ToDouble(txtXGSL.Text);
                double nJG = Convert.ToDouble(txtMRJG.Text);
                double nMSL = Convert.ToDouble(txtMRSL.Text);
                //double nSy = Math.Round((nMbj - nXj) * nGs, 2);
                //txtSy.Text = Convert.ToString(nSy);
                double nJun = Math.Round((nCB * nSL + nJG * nMSL) / (nSL + nMSL), 3);
                txtJun.Text = Convert.ToString(nJun);

            }
            catch (Exception)
            {

                MessageBox.Show("数据有误，请检查！");
                //throw;
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            if (txtPrice.Text == "" || cbbNum.Text == "" || cbbPer.Text == "")
            {
                return;
            }

            try
            {
                double nPrice = Convert.ToDouble(txtPrice.Text);
                int nNum = Convert.ToInt32(cbbNum.Text);
                double nPer = Convert.ToDouble(cbbPer.Text);

                txtResult.Text = Convert.ToString(Math.Round(Math.Pow((1 + nPer / 100), nNum) * nPrice, 2));

            }
            catch (Exception)
            {
                MessageBox.Show("数据有误，请检查！");
                //throw;
            }

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (txtYesPrice.Text == "" || txtNowPrice.Text == "")
            {
                return;
            }
            try
            {
                double nYesPrice = Convert.ToDouble(txtYesPrice.Text);
                double nNowPrice = Convert.ToDouble(txtNowPrice.Text);
                double nRange = Math.Round((nNowPrice - nYesPrice) / Math.Abs(nYesPrice), 4) * 100;
                txtRange.Text = Convert.ToString(nRange);
                if (nRange >= 0)
                {
                    txtRange.ForeColor = Color.Red;
                }
                else
                {
                    txtRange.ForeColor = Color.Green;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("数据有误，请检查！");
                //throw;
            }

        }

        private void btnSy_Click(object sender, EventArgs e)
        {
            if (txtXj.Text == "" || txtMbj.Text == "" || txtGs.Text == "")
            {
                return;
            }
            try
            {
                double nXj = Convert.ToDouble(txtXj.Text);
                double nMbj = Convert.ToDouble(txtMbj.Text);
                double nGs = Convert.ToDouble(txtGs.Text);
                double nSy = Math.Round((nMbj - nXj) * nGs, 2);
                txtSy.Text = Convert.ToString(nSy);
                if (nSy >= 0)
                {
                    txtSy.ForeColor = Color.Red;
                }
                else
                {
                    txtSy.ForeColor = Color.Green;
                }

            }
            catch (Exception)
            {

                MessageBox.Show("数据有误，请检查！");
                //throw;
            }

        }

        private void btnLL_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                axWindowsMediaPlayer1.URL = ofd.FileName;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void btnPas_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            //if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            //{
            //    axWindowsMediaPlayer1.Ctlcontrols.play();
            //}
            //判断视频是否已停止播放
            if ((int)axWindowsMediaPlayer1.playState == 1)
            {
                //停顿2秒钟再重新播放
                System.Threading.Thread.Sleep(2000);
                //重新播放
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void btnIpInfo_Click(object sender, EventArgs e)
        {

            // InnerIP
            var ipHost = Dns.Resolve(Dns.GetHostName());
            var ipaddress = ipHost.AddressList[0];
            int portNum = ipHost.AddressList.Length;//ip数量
            string ipStr = "内网地址:" + portNum.ToString() + Environment.NewLine;
            //MessageBox.Show();
            if (portNum>=1)
            {
                for (int i = 0; i < portNum; i++)
                {
                    ipStr += ipHost.AddressList[i] + Environment.NewLine;

                }
            }


            txtIpInfo.Text = ipStr;

            //外网
            txtIpInfo.Text += Environment.NewLine + "外网地址:" + Environment.NewLine + GetIP();

            
        }

        //获取外网IP
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception)
                {
                    return "";
                }

            }
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            try
            {
            cmd.Cmd c = new cmd.Cmd();
            txtCmd.Text = c.RunCmd(txtRun.Text.Trim());

            }
            catch (Exception)
            {
                MessageBox.Show("命令有问题，请检查");
                //throw;
            }





        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCmd.Clear();
        }

        private void txtRun_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtRun.Clear();
        }

        private void ckbTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ckbTop.Checked;
        }

        private void kbtnAdd_Click(object sender, EventArgs e)
        {
            //寿仙谷(SH:603896)>寿仙谷=SH603896  从雪球复制
            string newStock = ktxt.Text;
            if (newStock!="")
            {
                newStock = (newStock.Replace(":", "")).Replace("(","=").Replace(")","");

                if (kryLbLeft.Items.Contains(newStock))
                {
                    MessageBox.Show("要添加的数据已存在！");
                    return;
                }

                kryLbLeft.Items.Add(newStock);
                lblLeft.Text=kryLbLeft.Items.Count.ToString();
            }
        }

        private void kbtnDel_Click(object sender, EventArgs e)
        {
            if (kryLbLeft.SelectedItems.Count >= 1)
            {
                for (int i = kryLbLeft.SelectedItems.Count - 1; i >= 0; i--)
                {      //亦是从后删除

                    //klbLeft.Items.Remove(klbLeft.Items[klbLeft.SelectedIndices[i]]);

                    kryLbLeft.Items.Remove(kryLbLeft.Items[kryLbLeft.SelectedIndices[i]]);
                    //先获取索引，再获取文本内容 
                   
                }
                 calcNum();
            }




        }

        private void klbLeft_MouseUp(object sender, MouseEventArgs e)
        {


        }

        private void kbtnRight_Click(object sender, EventArgs e)
        {

            if (kryLbLeft.SelectedItems.Count>=1)
            {
                for (int i = kryLbLeft.SelectedItems.Count - 1; i >= 0; i--)
                {      //亦是从后删除

                    kryLbRight.Items.Add(kryLbLeft.Items[kryLbLeft.SelectedIndices[i]]);
                    kryLbLeft.Items.Remove(kryLbLeft.Items[kryLbLeft.SelectedIndices[i]]);
                    //先获取索引，再获取文本内容 
                   
                }
                 calcNum();
            }
        }

        private void kbtnLeft_Click(object sender, EventArgs e)
        {

            if (kryLbRight.SelectedItems.Count >= 1)
            {
                for (int i = kryLbRight.SelectedItems.Count - 1; i >= 0; i--)
                {      //亦是从后删除

                    kryLbLeft.Items.Add(kryLbRight.Items[kryLbRight.SelectedIndices[i]]);
                    kryLbRight.Items.Remove(kryLbRight.Items[kryLbRight.SelectedIndices[i]]);
                    //先获取索引，再获取文本内容 
                   

                }
                 calcNum();
            }


        }

        private void klbLeft_DragLeave(object sender, EventArgs e)
        {
            MessageBox.Show("Out");
        }

        private void txtToList(string path, KryptonListBox lst)
        {
            StreamReader file = new StreamReader(path, Encoding.UTF8);
            string s = "";
            while (s != null)
            {
                s = file.ReadLine();
                if (!string.IsNullOrEmpty(s))
                    lst.Items.Add(s);
            }
            file.Close();
        }


        private void txtToList2(string path, Krypton.Toolkit.KryptonListBox lst)
        {
            StreamReader file = new StreamReader(path, Encoding.UTF8);
            string s = "";
            while (s != null)
            {
                s = file.ReadLine();
                if (!string.IsNullOrEmpty(s))
                    lst.Items.Add(s);
            }
            file.Close();
        }





        private void calcNum()
        {
            lblLeft.Text=kryLbLeft.Items.Count.ToString();
            lblRight.Text = kryLbRight.Items.Count.ToString();

            

        }

        private void kbtnRead_Click(object sender, EventArgs e)
        {
            //txtToList(Application.StartupPath + "\\chan.txt", klbLeft);
            if (File.Exists(Application.StartupPath + "\\" + kCbbTxtList.Text))
            {
            //klbLeft.Items.Clear();
            kryLbLeft.Items.Clear();



                //txtToList(Application.StartupPath + "\\" + kCbbTxtList.Text, klbLeft);
                txtToList2(Application.StartupPath + "\\" + kCbbTxtList.Text, kryLbLeft);
                calcNum();

            }
            else
            {
                MessageBox.Show("指定的文件有问题！");
            }

           
        }

        private void klbLeft_DoubleClick(object sender, EventArgs e)
        {
             MessageBox.Show("指定的文件有问题！");
            //int index = this.klbLeft.IndexFromPoint(e.Location);
            //if (index != System.Windows.Forms.ListBox.NoMatches)
            //{
            //    MessageBox.Show(index.ToString());
            //}
        }

        private void kryptonRibbonGroupButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory());

        }

        private void kbtnSave_Click(object sender, EventArgs e)
        {


            //File.WriteAllLines(Application.StartupPath + "\\chan.txt", klbLeft.Items.Cast<string>().ToArray());
            //txtToList(Application.StartupPath + "\\" + kCbbTxtList.Text, klbLeft);


            File.WriteAllLines(Application.StartupPath + "\\" + kCbbTxtList.Text, kryLbLeft.Items.Cast<string>().ToArray());
            calcNum();


        }

        private void kchkMult_CheckedChanged(object sender, EventArgs e)
        {
            if (kchkMult.Checked==true)
            {
                kryLbLeft.SelectionMode = SelectionMode.MultiExtended;
                kryLbRight.SelectionMode = SelectionMode.MultiExtended;
            }
            else
            {
                kryLbLeft.SelectionMode = SelectionMode.MultiSimple;
                kryLbRight.SelectionMode = SelectionMode.MultiSimple;
            }
        }

        private void kContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }


        /// <summary>
        /// 获取程序目录下txt文件列表
        /// </summary>
        private void DocList()
        {
            string path = Application.StartupPath; // "C:\\myFolder1\\myFolder2";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files;
            files = dir.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            foreach (var filter in files)
            {
                //MessageBox.Show(filter.ToString());
                kCbbTxtList.Items.Add(filter.ToString());  

            }


        }

        private void krgbCalc_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/calc.exe");
        }

        private void kryptonRibbonGroupButton1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void kryptonRibbonGroupButton3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Windows\System32\drivers\etc");
        }

        private void kryptonRibbonGroupButton4_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(@"sysdm.cpl");
            //System.Diagnostics.Process.Start("explorer shell:::{BB06C0E4-D293-4f75-8A90-CB05B6477EEE}");
            try
            {
                cmd.Cmd c = new cmd.Cmd();
                txtCmd.Text = c.RunCmd("explorer shell:::{BB06C0E4-D293-4f75-8A90-CB05B6477EEE}");

            }
            catch (Exception)
            {
                MessageBox.Show("命令有问题，请检查");
                //throw;
            }


        }

        private void kryptonRibbonGroupButton1_Click_2(object sender, EventArgs e)
        {
            
        }

        private void kryptonRibbonGroupButton5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"shell:Startup");
        }

        private void kryptonRibbonGroupButton1_Click_3(object sender, EventArgs e)
        {
            
        }

        private void kryptonRibbonGroupButton6_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.Cmd c = new cmd.Cmd();
                txtCmd.Text = c.RunCmd("ie4uinit -show");

            }
            catch (Exception)
            {
                MessageBox.Show("命令有问题，请检查");
                //throw;
            }

        }

        private void kryptonRibbonGroup2_DialogBoxLauncherClick(object sender, EventArgs e)
        {
            MessageBox.Show("更新时间：2022年10月8日 10:50:13");
        }

        private void kryptonRibbonGroupButton1_Click_4(object sender, EventArgs e)
        {
            
        }

        private void kryptonRibbonGroupButton7_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/Clock.exe");
        }

        private void kryptonRibbonGroupButton8_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/EasyChange.exe");
        }

        private void kryptonRibbonGroupButton9_Click(object sender, EventArgs e)
        {
            
            Process.Start(Application.StartupPath + "/app/GetName.exe");
        }

        private void kryptonRibbonGroupButton1_Click_5(object sender, EventArgs e)
        {

        }

        private void kryptonRibbonGroupButton10_Click(object sender, EventArgs e)
        {
            Process.Start("gpedit.msc");
        }

        private void kryptonRibbonGroupButton1_Click_6(object sender, EventArgs e)
        {

        }

        private void kryptonRibbonGroupButton1_Click_7(object sender, EventArgs e)
        {

        }

        private void kryptonRibbonGroupButton11_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/ldr/ldrive.exe");
        }

        private void kryptonRibbonGroupButton12_Click(object sender, EventArgs e)
        {
           Process.Start(Application.StartupPath + "/app/Everything/Everything.exe"); 
        }

        private void kryptonRibbonGroupButton1_Click_8(object sender, EventArgs e)
        {

        }

        private void kryptonRibbonGroupButton13_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/SnapShot.exe");
        }

        private void kryptonRGBPoint_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/Pointofix/Pointofix.exe");
        }

        private void kryptonRibbonGroupButton14_Click(object sender, EventArgs e)
        {
            
          Process.Start(Application.StartupPath + "/app/notepad2.exe");
        }

        private void kryptonRibbonGroupButton15_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void kRgbInfo_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/牛牛记事/牛牛记事.exe");
        }

        private void kRgbVstart_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/VStart/VStart.exe");
        }

        private void kRgbSidebar_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/DeskWidget/DeskWidget.exe");
        }

        private void kRgbFiremin_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/Firemin/Firemin_X64.exe");
        }

        private void kryptonRibbonGroupButton16_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/ToYcon/ToYcon.exe");
        }

        private void ckbTop_Click(object sender, EventArgs e)
        {
            this.TopMost = ckbTop.Checked;

            this.Icon = Properties.Resources.viewstock;//可以即时改变

        }

        private void kRGBdisk_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/GlaryDisk/DiskAnalysis.exe");
        }

        private void kryptonRibbonGroupButton16_Click_1(object sender, EventArgs e)
        {
            notifyIcon1.Dispose();
            Application.Restart();
            Environment.Exit(0);
        }

        private void klbLeft_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("66");
        }

        private void kryLbLeft_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Up"); //不起作用
        }

        private void kryLbLeft_DoubleClick(object sender, EventArgs e)
        {
            //MessageBox.Show("Double Click");// 不起作用
            if (kryLbLeft.SelectedItems.Count >= 1)
            {
                ktxt.Text = kryLbLeft.SelectedItems[0].ToString();
                //kBtnGetChart.PerformClick();
                kBtnGetWEB.PerformClick();
                
            }
        }

        private Bitmap cropAtRect(Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
                return nb;
            }
        }



        private void kryLbLeft_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("M Double Click");
        }

        private void kryLbLeft_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("M Click");



        }

        private void kRadioB1_CheckedChanged(object sender, EventArgs e)
        {
            if (kRadioB1.Checked)
            {
                kryLbLeft.SelectionMode = SelectionMode.One;
                kryLbRight.SelectionMode = SelectionMode.One;
            }
        }

        private void kRadioB2_CheckedChanged(object sender, EventArgs e)
        {
            if (kRadioB2.Checked)
            {
                kryLbLeft.SelectionMode = SelectionMode.MultiSimple;
                kryLbRight.SelectionMode = SelectionMode.MultiSimple;
            }

        }

        private void kRadioB3_CheckedChanged(object sender, EventArgs e)
        {
            if (kRadioB3.Checked)
            {
                kryLbLeft.SelectionMode = SelectionMode.MultiExtended;
                kryLbRight.SelectionMode = SelectionMode.MultiExtended;
            }

        }

        private void kBtnEast_Click(object sender, EventArgs e)
        {
            try
            {
                string webA = "http://guba.eastmoney.com/list,";
                string webB = kryLbLeft.SelectedItems[0].ToString();

                if (kryLbLeft.SelectedItems.Count >= 1)
                {
                    webB = webB.Substring(webB.LastIndexOf('=') + 1) + ",f.html";//按发帖时间排序

                }

                 Process.Start(webA+webB);
            }
            catch (Exception)
            {
                MessageBox.Show("请检查代码是否存在！");
                //throw;
            }


        }

        private void kBtnXQ_Click(object sender, EventArgs e)
        {
            try
            {
                //string webA = "https://xueqiu.com/S/";
                //string webB = ktxt.Text.Substring(ktxt.Text.LastIndexOf('=') + 1);

                string webA = "https://xueqiu.com/S/";
                string webB = kryLbLeft.SelectedItems[0].ToString(); ;

                if (kryLbLeft.SelectedItems.Count >= 1)
                {
                    webB = webB.Substring(webB.LastIndexOf('=') + 1);

                }

                Process.Start(webA + webB);





            }
            catch (Exception)
            {
                MessageBox.Show("请检查代码是否存在！");
                //throw;
            }

        }

        private void kCbbTxtList_SelectedIndexChanged(object sender, EventArgs e)
        {
            kbtnRead.PerformClick();
        }

        private void kbtnClear_Click(object sender, EventArgs e)
        {
            kryLbLeft.Items.Clear();
            kryLbRight.Items.Clear();


        }

        private void kBtnEastAll_Click(object sender, EventArgs e)
        {
            try
            {
                string webA = "http://guba.eastmoney.com/list,";


                foreach (var item in kryLbLeft.Items.Cast<string>().ToArray())
                {
                    string webB = item.Substring(item.LastIndexOf('=') + 1) + ".html";

                    Process.Start(webA + webB);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("请检查代码是否存在！");
            }



        }

        private void kBtnXQAll_Click(object sender, EventArgs e)
        {
            try
            {
                string webA = "https://xueqiu.com/S/";


                foreach (var item in kryLbLeft.Items.Cast<string>().ToArray())
                {
                    string webB = item.Substring(item.LastIndexOf('=') + 1);

                    Process.Start(webA + webB);
                }

            }
            catch (Exception)
            {

                MessageBox.Show("请检查代码是否存在！");
            }



        }



        private void kryShutdown_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });


        }

        private void kryLogout_Click(object sender, EventArgs e)
        {
            //StartShutDown("-l");
            var psi = new ProcessStartInfo("shutdown", "/l");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);


        }

        private static void StartShutDown(string param)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "cmd";
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Arguments = "/C shutdown " + param;
            Process.Start(proc);
        }

        private void kryRestart_Click(object sender, EventArgs e)
        {
            var psi = new ProcessStartInfo("shutdown", "/r /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }

        private void kryptonCommand1_Execute(object sender, EventArgs e)
        {
            MessageBox.Show("del");

            //if (kryptonContextMenu1.Items[0].ToString()=="1")
            //{
            //    MessageBox.Show("del");
            //}
        }

        private void kryptonContextMenu1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("del");
        }

        private void kryCmdcopy_Execute(object sender, EventArgs e)
        {
            if (kryLbLeft.SelectedItems.Count >= 1)
            {
                
                Clipboard.SetDataObject(kryLbLeft.SelectedItems[0].ToString());
            }
        }

        private void kryCmdDel_Execute(object sender, EventArgs e)
        {
            kbtnDel.PerformClick();
        }

        private void QATBtnA_Click(object sender, EventArgs e)
        {
            //Process.Start("mspaint");
            Process.Start(System.IO.Directory.GetCurrentDirectory());


        }

        private void kryptonPage6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("s");
        }

        private void kryComNavEast_Execute(object sender, EventArgs e)
        {
            kBtnEast.PerformClick();
        }

        private void kryComNavXQ_Execute(object sender, EventArgs e)
        {
            kBtnXQ.PerformClick();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
             Application.Exit();
        }

        private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;
                    //Me.ShowInTaskbar = False
                    Hide();
                }
                else
                {

                    Show();
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;

                }
            }

        }

        private void QATBtMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //Me.ShowInTaskbar = False
            Hide();
        }

        private void kryLoadFont_Click(object sender, EventArgs e)
        {
            foreach (FontFamily font in FontFamily.Families)
            {
                kryFontName.Items.Add(font.Name.ToString());
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //kRtb.Height = kryptonPage7.Height - 300;
        }

        private void kryFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //float size = Convert.ToSingle(((ComboBox)sender).Text);

            //kRtb.Font = new Font(kRtb.Font.FontFamily, size);

            using (KryptonRichTextBox tmpRB = new KryptonRichTextBox())
            {
                tmpRB.SelectAll();
                tmpRB.SelectedRtf = kRtb.SelectedRtf;
                for (int i = 0; i < tmpRB.TextLength; ++i)
                {
                    tmpRB.Select(i, 1);
                    //tmpRB.SelectionFont = new Font("Arial", tmpRB.SelectionFont.Size);

                    tmpRB.SelectionFont = new Font(kryFontName.Text, tmpRB.SelectionFont.Size);

                    
                }
                tmpRB.SelectAll();
                kRtb.SelectedRtf = tmpRB.SelectedRtf;
            }




        }

        private void kryFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (KryptonRichTextBox tmpRB = new KryptonRichTextBox())
            {
                tmpRB.SelectAll();
                tmpRB.SelectedRtf = kRtb.SelectedRtf;
                for (int i = 0; i < tmpRB.TextLength; ++i)
                {
                    tmpRB.Select(i, 1);
                    //tmpRB.SelectionFont = new Font("Arial", tmpRB.SelectionFont.Size);

                    tmpRB.SelectionFont = new Font(tmpRB.SelectionFont.FontFamily, Convert.ToSingle(kryFontSize.Text));

                }
                tmpRB.SelectAll();
                kRtb.SelectedRtf = tmpRB.SelectedRtf;

            }
            //kRtb.SelectionFont = new Font(kRtb.SelectionFont.FontFamily, Convert.ToSingle(kryFontSize.Text));


        }

        private void kryFontColorBtn_SelectedColorChanged(object sender, ColorEventArgs e)
        {
            kRtb.SelectionColor = kryFontColorBtn.SelectedColor;
        }

        private void kryFontBackColorBtn_SelectedColorChanged(object sender, ColorEventArgs e)
        {
            kRtb.SelectionBackColor = kryFontBackColorBtn.SelectedColor;
        }

        private void kryFontColorBtn_Click(object sender, EventArgs e)
        {
            kRtb.SelectionColor = kryFontColorBtn.SelectedColor;
        }

        private void kryFontBackColorBtn_Click(object sender, EventArgs e)
        {
            kRtb.SelectionBackColor = kryFontBackColorBtn.SelectedColor;
        }


        private void kRtb_TextChanged(object sender, EventArgs e)
        {

        }

        private void kryPaste_Click(object sender, EventArgs e)
        {
             kRtb.Paste();//把剪贴板上的数据粘贴到目标RichTextBox
        }

        private void kryCut_Click(object sender, EventArgs e)
        {
            //剪切
            //Clipboard.SetData(DataFormats.Rtf, kRtb.SelectedRtf);//复制RTF数据到剪贴板
            //kRtb.SelectedRtf = "";//再把当前选取的RTF内容清除掉,当前就实现剪切功能了.
            kRtb.Cut();



        }

        private void kryCopy_Click(object sender, EventArgs e)
        {
            //Clipboard.SetData(DataFormats.Rtf, kRtb.SelectedRtf);//复制RTF数据到剪贴板
            kRtb.Copy();
        }

        private void getStockHome()
        {


            webView.Top = -238;
            webView.Left = -155;
            panel1.Width = 1600;
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate("https://www.cls.cn/finance");
                //tabPage1.Text = "Home";
            }

            //if (webViewB != null && webViewB.CoreWebView2 != null)
            //{
            //    webViewB.CoreWebView2.Navigate("http://www.spersky.com/mynav/");
            //    //tabPage1.Text = "Home";
            //}



        }
        private void kBtnGetWEB_Click(object sender, EventArgs e)
        {
            if (kryLbLeft.SelectedItems.Count < 1)
            {
                return;
            }



                kBtnGetWEB.Enabled = false;

            
            //await Task.Run(() =>
            //{
            //    getStockWeb();
            //});
            getStockWeb();//webview好像不能用异步，可能webview本身就是异步

            kBtnGetWEB.Enabled = true;







        }


        private  void getStockWeb()
        {
            webA = "http://quote.eastmoney.com/";
            webB = kryLbLeft.SelectedItems[0].ToString();
            webB = webB.Substring(webB.LastIndexOf('=') + 1) + ".html";

            //chromiumWebBrowser1.Top = -650;
            //chromiumWebBrowser1.Left = -496;
            //chromiumWebBrowser1.Load(webA+webB);

            //MessageBox.Show(webA+webB);
            //return;

            webView.Top = -647;
            webView.Left = -496;
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(webA+webB);
                //tabPage1.Text = "Home";
            }



        }

        private void ckbShowRightList_Click(object sender, EventArgs e)
        {

            if (ckbShowRightList.Checked==true)
            {
                kryLbRight.Visible = true;
                lblRight.Visible = true;
                kryLbRight.BringToFront();
            }
            else
            {
                kryLbRight.Visible = false;
                lblRight.Visible = false;
                kryLbRight.SendToBack();
            }

        }

        private void ckbShowRightList_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }


        private void kbtnRefresh_Click(object sender, EventArgs e)
        {
            if (webViewB != null && webViewB.CoreWebView2 != null)
            {
                webViewB.CoreWebView2.Reload();
            }




        }

        private void kOpenUrl_Click(object sender, EventArgs e)
        {
            if (webViewB != null && webViewB.CoreWebView2 != null)
            {
                webViewB.CoreWebView2.Navigate(kCbbUrl.Text);
                //tabPage1.Text = "Home";
            }

        }

        private void kWebBack_Click(object sender, EventArgs e)
        {
            if (webViewB.CanGoBack == true)
            {
                webViewB.CoreWebView2.GoBack();
            }
        }


        private void kWebForward_Click(object sender, EventArgs e)
        {
            if (webViewB.CanGoForward == true)
            {
                webViewB.CoreWebView2.GoForward();
            }
        }

        private void kBtnSelfOpen_Click(object sender, EventArgs e)
        {

            //MessageBox.Show((DirSize(new DirectoryInfo("Stocker2.exe.WebView2"))/1048576).ToString());


        }

        private void kBtnHomePage_Click(object sender, EventArgs e)
        {
            kryptonDockableNavigator1.SelectedPage= kryptonPage6;

            //cWebB.Load("https://mynav.ccccocccc.cc/index.html");
            if (webViewB != null && webViewB.CoreWebView2 != null)
            {
                webViewB.CoreWebView2.Navigate(webHome);
                //tabPage1.Text = "Home";
            }




        }

        private void kryWebCopyUrl_Click(object sender, EventArgs e)
        {
            if (webViewB.CoreWebView2.Source != null)
            {
                Clipboard.SetText(webViewB.CoreWebView2.Source.ToString());

            }

        }

        private void webViewB_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            //webViewB.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;//不打开新窗口


            kUrlInfo.TextLine1=webViewB.CoreWebView2.StatusBarText;
                webViewB.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;//不打开新窗口


        }

        private void CoreWebView2_NewWindowRequested(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {

            
            if (kBtnSelfOpen.Checked == false)
            { 
            webViewB.Source = new Uri(e.Uri.ToString());
            e.Handled = true;//禁止弹窗

                if (webViewB.CoreWebView2.Source != null)
                {
                    kCbbUrl.Text = webViewB.CoreWebView2.Source.ToString();
                }

            }




        }

        private void kStockHome_Click(object sender, EventArgs e)
        {
            getStockHome();
        }

        private void kclearWeb_Click(object sender, EventArgs e)
        {
            webView.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
            webViewB.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
        }

        private void webViewB_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (webViewB.CoreWebView2.CanGoBack == true)
            {
                kWebBack.ImageSmall = Properties.Resources.tagmark7;
                kWebBack.Enabled = true;
            }
            else
            {
                kWebBack.ImageSmall = Properties.Resources.tagmark7_Dis;
                kWebBack.Enabled = false;
            }


            if (webViewB.CoreWebView2.CanGoForward == true)
            {
                kWebForward.ImageSmall = Properties.Resources.tagmark1;
                kWebForward.Enabled = true;
            }
            else
            {
                kWebForward.ImageSmall = Properties.Resources.tagmark1_Dis;
                kWebForward.Enabled = false;
            }

            kCbbUrl.Text = webViewB.CoreWebView2.Source.ToString();
            kryptonPage6.Text = webViewB.CoreWebView2.DocumentTitle;
            prAlexa();
        }

        private void kBtnCode_Click(object sender, EventArgs e)
        {
            if (webViewB != null && webViewB.CoreWebView2 != null)
            {
                webViewB.Focus();
                SendKeys.Send("^u");//Ctrl+U 查看源码
                //kryptonProgressBar1.Style = ProgressBarStyle.Marquee;
            }
        }

        private void webViewB_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
        }

        private void webViewB_ContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            //kCbbUrl.Text = webViewB.CoreWebView2.StatusBarText;
        }

        public string searchE(string allStr, string findStr) //正则表达式
        {

            Regex reg = new Regex(findStr);
            Match m = reg.Match(allStr);

            //return m.Groups(0).Value.ToString();
            return m.Groups[0].Value;
            // Return m.Value
        }

        private async void prAlexa() //新线程查询pr
        {

            await Task.Run(() =>
            {
                try
                {
                    string queryUrl = searchE(kCbbUrl.Text, "^.+\\.(com.cn|com|net.cn|net|org.cn|org|gov.cn|gov|cn|mobi|me|la|info|name|biz|cc|tv|asia|hk|tw|网络|公司|中国)\\/");
                    //label1.Text = queryUrl;
                    //lblPr.Text = "" + GetPr(queryUrl); //get pr
                    string strConfig = "http://data.alexa.com/data?cli=10&dat=snba&url=" + queryUrl;
                    System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(strConfig);
                    string alexaInfo = "";
                    string CountryRank = "";
                    //Thread.Sleep(3000);
                    while (reader.Read())
                    {
                        if (reader.Name == "POPULARITY")
                        {
                            alexaInfo = reader.GetAttribute("TEXT");
                        }
                        if (reader.Name == "COUNTRY")
                        {
                            //CountryRank = reader.GetAttribute("NAME") + ":" + reader.GetAttribute("RANK");
                            CountryRank =reader.GetAttribute("RANK");

                        }
                        //<COUNTRY CODE="CN" NAME="China" RANK="44"/>
                    }
                    reader.Close();
                    if (string.IsNullOrEmpty(alexaInfo))
                    {
                        //textBox1.Text = "" + "-";
                        //kAlexa.TextLine1 = "排名";
                    }
                    else
                    {

                        lblAlexa.TextLine1= alexaInfo;
                        lblCN.TextLine1 = CountryRank;

                    }

                }
                catch (Exception)
                {
                    return;
                    //throw;
                }



            });


        }

        private void kHelp_Click(object sender, EventArgs e)
        {
        
            Process.Start("https://github.com/Krypton-Suite/Standard-Toolkit");
        }

        private void kExitTop_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void kIsTop_Click(object sender, EventArgs e)
        {
            if (this.TopMost == false)
            {
                this.TopMost = true;
                kIsTop.Image = Properties.Resources.red_pin;

            }
            else
            {
                this.TopMost = false;
                kIsTop.Image = Properties.Resources.blue_pin;
            }
        }


        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        private void kFolderSize_Click(object sender, EventArgs e)
        {
             MessageBox.Show((DirSize(new DirectoryInfo("Stocker2.exe.WebView2"))/1048576).ToString()+"M");
        }

        private void timSpace_Tick(object sender, EventArgs e)
        {
            kFolderSize.TextLine1 = (DirSize(new DirectoryInfo("Stocker2.exe.WebView2")) / 1048576).ToString() + "M";
        }

        private void kCalc_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/calc.exe");

        }

        private void kPad_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/notepad2.exe");

        }



        private void kTempClear_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/app/Clear/TEMP.bat");

        }

        private void kDisk_Click(object sender, EventArgs e)
        {
            Process.Start("diskmgmt.msc");
        }

        private void kCmd_Click(object sender, EventArgs e)
        {
            Process.Start("cmd");
        }

        private void kNetwork_Click(object sender, EventArgs e)
        {
            Process.Start("ncpa.cpl");//网络连接
        }

        private void kDiskIcon_Click(object sender, EventArgs e)
        {
            //Process.Start("rundll32.exe shell32.dll,Control_RunDLL desk.cpl,,0");//桌面图标
            try
            {
                cmd.Cmd c = new cmd.Cmd();
                txtCmd.Text = c.RunCmd("rundll32.exe shell32.dll,Control_RunDLL desk.cpl,,0");

            }
            catch (Exception)
            {
                MessageBox.Show("命令有问题，请检查");
                //throw;
            }
        }

        private void kShutDown_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }

        private void calcClear_Click(object sender, EventArgs e)
        {
            txtXGCB.Text = "";
            txtXGSL.Text = "";
            txtMRJG.Text = "";
            txtMRSL.Text = "";
            txtJun.Text = "";
        }

        private void kBtnNorthMoney_Click(object sender, EventArgs e)
        {
            webView.Top = -668;
            webView.Left = -420;
            panel1.Width = 1600;
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate("https://data.eastmoney.com/hsgt/index.html");
                //tabPage1.Text = "Home";
            }



        }

        private void kBtnTotalM_Click(object sender, EventArgs e)
        {
            webView.Top = -10;
            webView.Left = -10;
            panel1.Width = 1600;
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate("http://zx.10jqka.com.cn/fundholo/public/index.html#/index");
                //tabPage1.Text = "Home";
            }
        }

        private void kBtnRank_Click(object sender, EventArgs e)
        {
            webView.Top = -45;
            webView.Left = -276;
            panel1.Width = 1600;
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate("https://guba.eastmoney.com/rank/");
                //tabPage1.Text = "Home";
            }
        }

        private void kBtnMS_Click(object sender, EventArgs e)
        {
            getTTS();
        }


        /// <summary>
        /// 打开tts语音主页
        /// </summary>
        private void getTTS()
        {
            webViewAI.Top = -476;
            webViewAI.Left = -139;
            panelAi.Width = 1520;
            if (webViewAI != null && webViewAI.CoreWebView2 != null)
            {
                webViewAI.CoreWebView2.Navigate("https://azure.microsoft.com/zh-cn/products/cognitive-services/text-to-speech/#features");
                //webViewAI.CoreWebView2.Navigate("https://guba.eastmoney.com/rank/");
                //tabPage1.Text = "Home";
            }
        }

        private void kBtnOneRank_Click(object sender, EventArgs e)
        {
            webView.Top = -45;
            webView.Left = -276;
            panel1.Width = 1600;

            string webA = "http://guba.eastmoney.com/rank/stock?code=";
            string webB = kryLbLeft.SelectedItems[0].ToString(); ;

            if (kryLbLeft.SelectedItems.Count >= 1)
            {
                webB = webB.Substring(webB.LastIndexOf('=') + 3);

                if (webView != null && webView.CoreWebView2 != null)
                {
                    webView.CoreWebView2.Navigate(webA + webB);
                    //tabPage1.Text = "Home";
                }


            }
            //MessageBox.Show(webB);
            //return;
            //Process.Start(webA + webB);


        }



        private void kryBtnControl_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.Cmd c = new cmd.Cmd();
                txtCmd.Text = c.RunCmd("control");

            }
            catch (Exception)
            {
                MessageBox.Show("命令有问题，请检查");
                //throw;
            }
        }

        private void kryptonDockableNavigator1_Click(object sender, EventArgs e)
        {
            if (kryptonDockableNavigator1.SelectedPage == kryptonPage6)
            {
                //MessageBox.Show("1p");
                kryptonRibbon1.SelectedTab = kryptonRibbonTab4; //联动，当选择浏览器页面时，工具栏对应选择浏览工具tab
            }
            else if (kryptonDockableNavigator1.SelectedPage == kryptonPage7)
            {
                kryptonRibbon1.SelectedTab = kryptonRibbonTab3;
            }
        }

        private void kbtnSaveAs_Click(object sender, EventArgs e)
        {
            if (kCbbRtfList.Text!="")
            {
                string rtfName = Path.GetFileName(kCbbRtfList.Text);
                DialogResult dr = MessageBox.Show("确定保存吗，将覆盖<"+ rtfName +">！", "对话框标题", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    //点确定的代码
                    kRtb.SaveFile(kCbbRtfList.Text, RichTextBoxStreamType.RichText);
                }
  
            }
            else
            {
                string saveTitle = ""; //bmk 保存RTF
                if (!string.IsNullOrEmpty(kRtb.Text))
                {
                    saveTitle = (kRtb.Text.ToString().Trim(' ').Substring(0, 10)).Replace("\n", "");
                    saveTitle = saveTitle.Replace("/", "");
                    saveTitle = saveTitle.Replace("\\", "");
                    saveTitle = saveTitle.Replace(":", "");
                }
                SaveFileDialog saveFile1 = new SaveFileDialog();
                //如果当前文档没有路径(新建的文档)
                saveFile1.FilterIndex = 1;
                saveFile1.FileName = saveTitle;
                saveFile1.Filter = "写字板文件(*.rtf)|*.rtf";
                saveFile1.DefaultExt = "";
                //End If
                saveFile1.FilterIndex = 1;
                saveFile1.InitialDirectory = Application.StartupPath + @"\notes";
                saveFile1.Title = "保存文件";
                saveFile1.ShowDialog();
                //如果使用者选择了"所有文件"筛选项,将不再自动添加扩展名
                if (saveFile1.FilterIndex == 3)
                {
                    saveFile1.AddExtension = false;
                }
                else
                {
                    saveFile1.AddExtension = true;
                }
                if (saveFile1.FileName == "")
                {
                    return;
                }
                int shu1 = 0;
                string zhi1 = null;
                shu1 = saveFile1.FileName.Length;
                zhi1 = saveFile1.FileName.Substring(shu1 - 3, 3);
                if (zhi1 == "rtf") //根据选择或输入的文件类型,用不同的方式另存文件
                {
                    kRtb.SaveFile(saveFile1.FileName, RichTextBoxStreamType.RichText);
                    //oldfilename = saveFile1.FileName;
                }
                else if (zhi1 == "txt") //根据选择或输入的文件类型,用不同的方式另存文件
                {
                    kRtb.SaveFile(saveFile1.FileName, RichTextBoxStreamType.TextTextOleObjs);
                    //oldfilename = saveFile1.FileName;
                }
                else //根据选择或输入的文件类型,用不同的方式另存文件
                {
                    kRtb.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                    //oldfilename = saveFile1.FileName;
                }
                //this.Text = this.Text.Replace("*", "");


            }

        }

        private void kbtnReadRTF_Click(object sender, EventArgs e)
        {

            if (kCbbRtfList.Text!="")
            {
                kRtb.LoadFile(kCbbRtfList.Text, RichTextBoxStreamType.RichText);  
            }



            //OpenFileDialog openFile1 = new OpenFileDialog();

            //openFile1.DefaultExt = "*.rtf";
            //openFile1.Filter = "RTF Files|*.rtf";

            //if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
            //   openFile1.FileName.Length > 0)
            //{
            //    // Load the contents of the file into the RichTextBox.
            //    kRtb.LoadFile(openFile1.FileName, RichTextBoxStreamType.RichText);
            //}



        }
        private void loadRtfList()
        {
            kCbbRtfList.Items.Clear(); 
            //DirectoryInfo dir = new DirectoryInfo(@"d:\picture");
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + "//notes");


            FileInfo[] inf = dir.GetFiles();//返回当前目录的文件列表，返回结果数据类型为FileInfo[]，即数组
            foreach (FileInfo finf in inf)
            {
                if (finf.Extension.Equals(".rtf"))
                {
                    //如果扩展名为“.jpeg”
                    kCbbRtfList.Items.Add(finf.FullName);
                    //在listbox1控件中添加文件的完整目录和文件名
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void kbtnList_Click(object sender, EventArgs e)
        {
            loadRtfList();
        }

        private void kbtnClearCbbTxt_Click(object sender, EventArgs e)
        {
            kCbbRtfList.Text = "";
        }

        private void kbtnRtfDir_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "//notes");
        }

        private void kCMenuNotetemCopy_Click(object sender, EventArgs e)
        {
            //Clipboard.SetData(DataFormats.Rtf, kRtb.SelectedRtf);//复制RTF数据到剪贴板

            kRtb.Copy();

        }

        private void kCMenuNotetemPaste_Click(object sender, EventArgs e)
        {
            kRtb.Paste();//把剪贴板上的数据粘贴到目标RichTextBox

        }

        private void kCMenuNotetemCut_Click(object sender, EventArgs e)
        {
            //剪切
            //Clipboard.SetData(DataFormats.Rtf, kRtb.SelectedRtf);//复制RTF数据到剪贴板
            //kRtb.SelectedRtf = "";//再把当前选取的RTF内容清除掉,当前就实现剪切功能了.
            kRtb.Cut();

        }

        private void kCMenuCopyAll_Click(object sender, EventArgs e)
        {
            //Clipboard.SetData(DataFormats.Rtf,kRtb.Rtf);
            kRtb.SelectAll();
            kRtb.Copy();
            kRtb.SelectionLength = 0;
            
        }

        private void kbtnSearchCbbRtf_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < kCbbRtfList.Items.Count; i++)
            {
                string value = kCbbRtfList.GetItemText(kCbbRtfList.Items[i]);
                if (value.Contains(kbtnSearchTxt.Text))
                {
                    kCbbRtfList.Text = value;
                    return;//只能选择一个
                }
            }
        }

        private void kbtnDelRtf_Click(object sender, EventArgs e)
        {
            if (kCbbRtfList.Text!="")
            {
                string rtfName = Path.GetFileName(kCbbRtfList.Text);
                DialogResult dr = MessageBox.Show("确定要删除<"+rtfName+">吗，删除后可能不能被恢复！", "对话框标题", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    //点确定的代码
                    File.Delete(kCbbRtfList.Text);
                    loadRtfList();
                }


            }


        }

        private void kcMenuSelectAll_Click(object sender, EventArgs e)
        {
            kRtb.SelectAll();
        }

        private void kcMenuClear_Click(object sender, EventArgs e)
        {
            kRtb.Clear();
        }

        private void ktxt_TextChanged(object sender, EventArgs e)
        {


            kryLbLeft.ClearSelected();

            if (ktxt.Text == "")
            {
                lblSearchGP.Text= "";
                return;
            }

            int count = 0;
            for (int i = 0; i < kryLbLeft.Items.Count; i++)
            {
                if (kryLbLeft.Items[i].ToString().Contains(ktxt.Text.Trim()))
                {
                    kryLbLeft.SetSelected(i, true);
                    count++;
                }
            }
            if (count > 0)
            {
                lblSearchGP.Text= "找到匹配项：" + count.ToString();
            }
            else
            {
                lblSearchGP.Text= "";
            };
        }

        private void kAlexa_Click(object sender, EventArgs e)
        {
            // 设置 webHome 变量

            webHome = kCbbUrl.Text.Trim();
            // 保存到程序设置中
            Properties.Settings.Default.webhome = webHome;
            Properties.Settings.Default.Save();
            MessageBox.Show("已保存，当前主页为：" + webHome);
        }
    }
}
