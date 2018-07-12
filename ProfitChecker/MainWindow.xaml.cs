using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Media;

namespace ProfitChecker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<MonitoredItem> itemlist;
        private static int baseHeightModifier = 30;
        private int consolecount = 0;
        private Prompt myp;
        private BaseItem basee;
        private bool running = false;
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        public MainWindow()
        {
            InitializeComponent();

            itemlist = new List<MonitoredItem>();
            Threads.Add("MainThread", StartLoop);
            InitializeConfig();
        }



        private void InitializeConfig()
        {
            if (!File.Exists("config.cfg")) return;
            Config cfg = new Config(itemlist);
            foreach(var item in cfg.returnConfig())
            {
                addItem(item.id, item.name, item.maxprof, false);
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (myp.Visibility == Visibility.Visible)
            {
                Thread.Sleep(50);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(running)
            {
                MessageBox.Show("Please stop the Bot in order to add additional items.");
                return;
            }
            basee = new BaseItem(null, 0, 0);
            myp = new Prompt(basee);
            myp.Show();
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(finishWorking);
        }
        void finishWorking(object sender, RunWorkerCompletedEventArgs e)
        {
            addItem(basee.id, basee.name, basee.maxprof, false);
        }

        public void addItem(int itemId, string itemName, int maxprofit, bool controlonly)
        {
            Label dynamicLabel = new Label();
            dynamicLabel.Name = "Element"+itemlist.Count.ToString();
            dynamicLabel.Content = "Item: "+itemName+" | LF under "+maxprofit+"g | Curr Price: ";
            dynamicLabel.Width = 350;
            dynamicLabel.Height = 30;
            dynamicLabel.Margin = new Thickness(10, baseHeightModifier*(itemlist.Count+1), 0, 0);
            dynamicLabel.HorizontalAlignment = HorizontalAlignment.Left;
            dynamicLabel.VerticalAlignment = VerticalAlignment.Top;
            mainGrid.Children.Add(dynamicLabel);

            Label statusLabel = new Label();
            statusLabel.Name = "StatusElement" + itemlist.Count.ToString();
            statusLabel.Content = "Not Checked";
            statusLabel.Width = 100;
            statusLabel.Height = 30;
            statusLabel.Margin = new Thickness(360, baseHeightModifier * (itemlist.Count + 1), 0, 0);
            statusLabel.HorizontalAlignment = HorizontalAlignment.Left;
            statusLabel.VerticalAlignment = VerticalAlignment.Top;
            mainGrid.Children.Add(statusLabel);

            Button removeButton = new Button();
            removeButton.Content = "Mute";
            removeButton.Width = 110;
            removeButton.Height = 20;
            removeButton.Margin = new Thickness(460, baseHeightModifier * (itemlist.Count + 1), 0, 0);
            removeButton.HorizontalAlignment = HorizontalAlignment.Left;
            removeButton.VerticalAlignment = VerticalAlignment.Top;
            removeButton.ToolTip = "Thread" + itemlist.Count.ToString();
            mainGrid.Children.Add(removeButton);
            removeButton.Click += new RoutedEventHandler(removeButton_Click);
           
            if(!controlonly)
            {
                MonitoredItem newItem = new MonitoredItem(dynamicLabel, statusLabel, removeButton, itemId, itemName, maxprofit, "Thread" + itemlist.Count.ToString());
                itemlist.Add(newItem);
                outputConsole("Item added (ID: " + itemId + ")");
            } 
        }

        void removeButton_Click(object sender, RoutedEventArgs e)
        {
            /*Button clickedbutton = (Button)sender;
            MonitoredItem itemtoRemove = null;
            foreach(var item in itemlist)
            {
                if(item.threadName.Equals(clickedbutton.ToolTip))
                {
                    itemtoRemove = item;
                }
            }
            mainGrid.Children.Remove(itemtoRemove.Data);
            mainGrid.Children.Remove(itemtoRemove.status);
            mainGrid.Children.Remove(itemtoRemove.removalButton);
            itemlist.Remove(itemtoRemove);
            outputConsole("Item removed (ID: " + itemtoRemove.itemId + ")");*/
            Button clickedbutton = (Button)sender;
            MonitoredItem itemtoMute = null;
            foreach (var item in itemlist)
            {
                if (item.threadName.Equals(clickedbutton.ToolTip))
                {
                    itemtoMute = item;
                }
            }
            itemtoMute.muted = !itemtoMute.muted;
            if (clickedbutton.Content.Equals("Mute")) clickedbutton.Content = "Unmute";
            else clickedbutton.Content = "Mute";
        }

        private void StartLoop()
        {
            foreach (var item in itemlist)
            {
                item.status.Dispatcher.BeginInvoke((Action)(() =>
                item.status.Content = "Running..."
                ));
            }
            while (true)
            {
                WebClient client = new WebClient();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                outputConsole("Gathering base data");
                RootObjectBase filebase = jss.Deserialize<RootObjectBase>(client.DownloadString("https://eu.api.battle.net/wow/auction/data/lothar?locale=en_GB&apikey=7t9urwbp4ckdgu9dcfn6tnsp8zhqj3yh"));
                update.Dispatcher.BeginInvoke((Action)(() =>
                update.Content = "Last AH update: " + UnixTimeStampToDateTime(filebase.files[0].lastModified).ToShortTimeString()
                ));
                if (UnixTimeStampToDateTime(filebase.files[0].lastModified).ToShortTimeString().Equals(DateTime.Now.ToShortTimeString()))
                {
                    SoundPlayer audio = new SoundPlayer(Properties.Resources.I_Have_Crippling_Depression___iDubbbz_Sound_Effect__HD_);
                    audio.Play();
                }

                nextupdate.Dispatcher.BeginInvoke((Action)(() =>
                nextupdate.Content = "Next AH update: " + UnixTimeStampToDateTime(filebase.files[0].lastModified).AddMinutes(40).ToShortTimeString()
                ));
                outputConsole("Downloading auction house data");
                string data = null;
                string str = null;
                RootObjectAuction auctiondata = null;
                try
                {
                    byte[] dataArray = client.DownloadData(filebase.files[0].url);
                    str = System.Text.Encoding.Default.GetString(dataArray);
                    auctiondata = jss.Deserialize<RootObjectAuction>(str);
                }
                catch(Exception e)
                {
                    System.IO.File.WriteAllText("debug.txt", e.Message);
                    System.IO.File.WriteAllText("Dump.txt", str);
                }
                
                foreach(var item in itemlist)
                {
                    outputConsole("Checking "+item.itemId);
                    List<Auction> unsortedFound = new List<Auction>();
                    foreach (var auction in auctiondata.Auctions)
                    {
                        if (auction.Item == item.itemId)
                        {
                            unsortedFound.Add(auction);
                        }
                    }
                    List<Auction> SortedList = unsortedFound.OrderBy(o => o.Buyout).ToList();
                    double lowestprice = ((double)SortedList[0].Buyout / 10000) / SortedList[0].Quantity;
                    item.Data.Dispatcher.BeginInvoke((Action)(() =>
                    item.Data.Content = "Item: " + item.itemName + " | LF under " + item.maxprofit + "g | Curr Price: " + lowestprice+ "g"
                    ));
                    if(lowestprice>item.maxprofit)
                    {
                        item.status.Dispatcher.BeginInvoke((Action)(() =>
                        item.status.Content = "No Profit!"
                        ));
                        item.status.Dispatcher.BeginInvoke((Action)(() =>
                        item.status.Foreground = Brushes.Red
                        ));
                    }else
                    {
                        if(!item.muted)
                        {
                            SoundPlayer audio = new SoundPlayer(Properties.Resources.profit);
                            audio.Play();
                        }
                        item.status.Dispatcher.BeginInvoke((Action)(() =>
                        item.status.Content = "Profit!"
                        ));
                        item.status.Dispatcher.BeginInvoke((Action)(() =>
                        item.status.Foreground = Brushes.Green
                        ));
                    }
                }
                outputConsole("Pausing for 20s");
                Thread.Sleep(20000);
                
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            running = !running;
            Threads.ToggleThread("MainThread");
            if (running)
            {
                startButton.Content = "Stop";
                outputConsole("Start pressed");
            }
            else
            {
                startButton.Content = "Start";
                outputConsole("Stop pressed");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Config cfg = new Config(itemlist);
            cfg.RewriteConfig();
            outputConsole("Config saved.");
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void outputConsole(string text)
        {
            consolecount++;
            if(consolecount>14)
            {
                textconsole.Dispatcher.BeginInvoke((Action)(() =>
                textconsole.Text = ""
                ));
                consolecount = 1;
            }
            textconsole.Dispatcher.BeginInvoke((Action)(() =>
            textconsole.Text += "\n" + DateTime.Now.ToLongTimeString() + ": " + text
            ));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Environment.Exit(3);
        }
    }
}
