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
            DataItems.Clear();
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

                T item = (T)Activator.CreateInstance(typeof(T), new object[] { name });
                DataScript.RemoveDataItem(item as DataItem);
                (item as DataItem).SetData(value);
                DataItems.Add(item);
            }
        }

        internal override string GetData(string data)
        {
            if (!typeof(T).IsSubclassOf(typeof(DataItem)))
            {
                Error("Cannot get data as {0} is not a type of DataItem", typeof(T).ToString());
                return Name;
            }

            string d = data.ToLower();
            if (d == "value")
                return ToString();
            else
            {
                Error("Unknown data value ({0})", data);
                return Name;
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

        public override string ToString()
        {
            if (!typeof(T).IsSubclassOf(typeof(DataItem)))
            {
                Error("Cannot get value as {0} is not a type of DataItem", typeof(T).ToString());
                return Name;
            }

            string output = "([";
            for (int i = 0; i < DataItems.Count; i++)
                if (i == DataItems.Count - 1)
                    output += (DataItems[i] as DataItem).Name + ":" + (DataItems[i] as DataItem);
                else
                    output += (DataItems[i] as DataItem).Name + ":" + (DataItems[i] as DataItem) + ", ";
            return output + "])";
        }
    }
}
