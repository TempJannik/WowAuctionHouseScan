using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProfitChecker
{

    public partial class Prompt : Window
    {
        BaseItem basee;
        public Prompt(BaseItem basee)
        {
            this.basee = basee;
            InitializeComponent();
            this.Topmost = true;
            submitbtn.IsDefault = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(itemid.Text.Equals(String.Empty) || maxprof.Text.Equals(String.Empty))
            {
                MessageBox.Show("A field was not filled out correctly.");
                return;
            }
            else if(itemid.Text.Length>10)
            {
                MessageBox.Show("Incorrect Item ID");
                return;
            }
            int itemID = Int32.Parse(itemid.Text);
            int maxGold = Int32.Parse(maxprof.Text);
            string itemname = String.Empty;
            using (WebClient downloader = new WebClient())
            {
                try
                {
                    string resp = downloader.DownloadString("https://eu.api.battle.net/wow/item/" + itemID + "?locale=en_GB&apikey=7t9urwbp4ckdgu9dcfn6tnsp8zhqj3yh");
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    RootObjectItem item = jss.Deserialize<RootObjectItem>(resp);
                    basee.id = itemID;
                    basee.name = item.name;
                    basee.maxprof = maxGold;
                    this.Visibility = Visibility.Hidden;
                }
                catch
                {
                    MessageBox.Show("Error");
                }
            }
        }
    }
}
