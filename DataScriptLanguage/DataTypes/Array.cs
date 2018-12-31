using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataScriptLanguage.DataTypes
{
    public class Array<T> : DataItem
    {
        private List<T> DataItems = new List<T>();

        public int Count { get { return DataItems.Count; } }

        public Array(string name)
            : base(name)
        {
            if (!typeof(T).IsSubclassOf(typeof(DataItem)))
                Error("{0} is not a type of DataItem", typeof(T).ToString());
        }

        internal override void SetData(string[] data)
        {
            if (!typeof(T).IsSubclassOf(typeof(DataItem)))
            {
                Error("Cannot set data as {0} is not a type of DataItem", typeof(T).ToString());
                return;
            }

            foreach (string s in data)
            {
                string name = "";
                string v = s;
                if (s.Contains(":"))
                {
                    name = s.Split(new char[] { ':' }, 2)[0];
                    v = s.Split(new char[] { ':' }, 2)[1];
                }
                string[] value = Regex.Split(v, @",(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)").Select(p => p.Trim().Replace("\"", "")).ToArray();
                
                for (int k = 0; k < data.Length; k++)
                {
                    foreach (Match m in Regex.Matches(data[k], @"(?<=\$){(.+?)}"))
                        data[k] = data[k].Replace("$" + m.Value, DataScript.GetDataItem(m.Value.Substring(1, m.Value.Length - 2)).ToString());
                }

                T item = (T)Activator.CreateInstance(typeof(T), new object[] { name });
                DataScript.RemoveDataItem(item as DataItem);
                (item as DataItem).SetData(value);
                DataItems.Add(item);
            }
        }

        public T this[int index] {
            get
            {
                return DataItems[index];
            }
        }

        public T this[string name]
        {
            get
            {
                if (!typeof(T).IsSubclassOf(typeof(DataItem)))
                {
                    Error("Cannot get DataItem {0} as {1} is not a type of DataItem", name, typeof(T).ToString());
                    return default(T);
                }

                if (name == "")
                {
                    Warn("Specify a name of a DataItem");
                    T item = (T)Activator.CreateInstance(typeof(T), new object[] { "" });
                    DataScript.RemoveDataItem(item as DataItem);
                    return item;
                }

                foreach (T dataItem in DataItems)
                {
                    DataItem item = dataItem as DataItem;
                    if (item.Name == name)
                        return dataItem;
                }

                Error("Could not find DataItem with name: {0}", name);
                T dItem = (T)Activator.CreateInstance(typeof(T), new object[] { "" });
                DataScript.RemoveDataItem(dItem as DataItem);
                return dItem;
            }
        }
    }
}
