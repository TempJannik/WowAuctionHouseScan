using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProfitChecker
{
    class Config
    {

        private List<MonitoredItem> list;
        public Config(List<MonitoredItem> list)
        {
            this.list = list;
        }

        public void RewriteConfig()
        {
            if (System.IO.File.Exists("config.cfg")) System.IO.File.Delete("config.cfg");
            string[] datatoCopy = new string[list.Count];
            for(int i = 0;i<list.Count;i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(list[i].itemId);
                sb.Append("|");
                sb.Append(list[i].itemName);
                sb.Append("|");
                sb.Append(list[i].maxprofit);
                datatoCopy[i] = sb.ToString();
            }
            File.WriteAllLines("config.cfg", datatoCopy);
        }

        public List<BaseItem> returnConfig()
        {
            if (!File.Exists("config.cfg")) return null;
            string[] response = File.ReadAllLines("config.cfg");
            List<BaseItem> FullList = new List<BaseItem>();
            foreach(var item in response)
            {
                string[] split = item.Split('|');
                FullList.Add(new BaseItem(split[1],Int32.Parse(split[0]),Int32.Parse(split[2])));
            }
            return FullList;
        }
    }
}
