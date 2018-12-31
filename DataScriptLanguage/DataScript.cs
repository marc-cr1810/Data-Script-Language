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
            if (name == "")
            {
                Log.GetCoreLogger().Warn("Specify a name of a DataItem");
                return new DataItem(name, false);
            }
            foreach (DataItem item in Items)
                if (item.Name == name)
                    return item;
            Log.GetCoreLogger().Error("Could not find DataItem with name: {0}", name);
            return new DataItem(name, false);
        }

        public static void Read(string path)
        {
            string[] parts = Regex.Split(File.ReadAllText(path), @"(?<!\$)([{])|(?<!\S)([}])|([\[\]])").Select(p => p.Trim()).ToArray();
            List<string> group = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                string s = parts[i];
                if (string.IsNullOrEmpty(s))
                    continue;

                if (!s.Contains(":") && !s.Contains(" ") && parts[i + 1] == "{")
                {
                    group.Add(s);
                    i++;
                }
                else if (s == "}")
                    group.RemoveAt(group.Count - 1);
                else if (s.Contains(":"))
                {
                    string[] lines = s.Replace("\r", "").Split(new char[] { '\n' }).Select(p => p.Trim()).ToArray();
                    for (int h = 0; h < lines.Length; h++)
                    {
                        string line = lines[h];
                        for (int j = 0; j < Items.Count; j++)
                        {
                            DataItem item = Items[j];
                            string name = string.Join(".", group) + (group.Count > 0 ? "." : "") + line.Split(new char[] { ':' }, 2)[0];
                            if (item.Name == name)
                            {
                                if (line.Split(new char[] { ':' }, 2, System.StringSplitOptions.RemoveEmptyEntries).Length == 1)
                                {
                                    if (parts[i + 1] == "[")
                                    {
                                        i++;
                                        line += " ";
                                        for (i = i + 1; i < parts.Length; i++)
                                        {
                                            if (parts[i] == "]")
                                            {
                                                i++;
                                                break;
                                            }
                                            line += parts[i];
                                        }
                                    }
                                    else
                                    {
                                        Log.GetCoreLogger().Warn("No value assigned to {1}", path, name);
                                        break;
                                    }
                                }
                                string d = line.Split(new char[] { ':' }, 2)[1];
                                string[] data = Regex.Split(d, @",(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)").Select(p => p.Trim().Replace("\"", "")).ToArray();

                                for (int k = 0; k < data.Length; k++)
                                {
                                    foreach (Match m in Regex.Matches(data[k], @"(?<=\$){(.+?)}"))
                                        data[k] = data[k].Replace("$" + m.Value, GetDataItem(m.Value.Substring(1, m.Value.Length - 2)).ToString());
                                }

                                item.SetData(data);
                            }
                        }
                    }
                }
                else
                {
                    Log.GetCoreLogger().Error("Failed to read file ({0})", path);
                    return;
                }
            }
        }
    }
}
