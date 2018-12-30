using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataScriptLanguage.DataTypes;

namespace DataScriptLanguage
{
    public static class DataScript
    {
        private static List<DataItem> Items = new List<DataItem>();
        
        public static void AddDataItem(DataItem item) { Items.Add(item); }
        public static void RemoveDataItem(DataItem item) { Items.Remove(item); }

        public static DataItem GetDataItem(string name)
        {
            foreach (DataItem item in Items)
                if (item.Name == name)
                    return item;
            Log.GetCoreLogger().Error("Could not find DataItem with name: {0}", name);
            return null;
        }

        public static void Read(string path)
        {
            string[] lines = File.ReadAllText(path).Replace("\r", "").Split('\n');

            List<string> group = new List<string>();
            foreach (string line in lines)
            {
                string s = line.Trim();
                if (!s.StartsWith("//") && s.Length > 0)
                {
                    if (!s.Contains(":") && !s.Contains("}"))
                        group.Add(s.Split(new char[] { ' ' }, 2)[0]);
                    else if (s == "}")
                        group.RemoveAt(group.Count - 1);
                    else
                    {
                        foreach (DataItem item in Items)
                        {
                            string name = string.Join(".", group) + (group.Count > 0 ? "." : "") + s.Split(new char[] { ':' }, 2)[0];
                            if (item.GetName() == name)
                            {
                                string d = s.Split(new char[] { ':' }, 2)[1];
                                if (d.StartsWith(" "))
                                    d = d.Substring(1, d.Length - 1);
                                string[] data = Regex.Matches(d, @"[\""].+?[\""]|[^,]+")
                                    .Cast<Match>()
                                    .Select(m => m.Value.Replace("\"", "")).ToArray();
                                item.SetData(data);
                            }
                        }
                    }
                }
            }
        }
    }
}
