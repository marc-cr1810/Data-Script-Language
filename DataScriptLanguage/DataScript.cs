﻿using System;
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

        public static void Read(string path)
        {
            string[] lines = File.ReadAllText(path).Replace("\r", "").Split('\n');

            foreach (string s in lines)
            {
                if (!s.StartsWith("//") && s.Length > 0)
                {

                    foreach (DataItem item in Items)
                    {
                        string name = s.Split(new char[] { ':' }, 2)[0];
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
