using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProfitChecker
{
    class MonitoredItem
    {
        public Button removalButton;
        public Label status;
        public Label Data;
        public int itemId;
        public string itemName;
        public int maxprofit;
        public string threadName;
        public bool muted;

        public MonitoredItem(Label Data, Label status, Button remove, int itemId, string itemName,int maxprofit ,string threadName, bool muted = false)
        {
            this.Data = Data;
            removalButton = remove;
            this.status = status;
            this.itemId = itemId;
            this.maxprofit = maxprofit;
            this.itemName = itemName;
            this.threadName = threadName;
            this.muted = muted;
        }
    }

    public class BaseItem
    {
        public string name;
        public int id;
        public int maxprof;

        public BaseItem(string name, int id, int maxprof)
        {
            this.name = name;
            this.id = id;
            this.maxprof = maxprof;
        }
    }
}
