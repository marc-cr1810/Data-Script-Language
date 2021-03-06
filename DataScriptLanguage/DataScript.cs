﻿using System.Collections.Generic;
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

        internal static string GetDataItemValue(string name)
        {
            if (name == "")
            {
                Log.GetCoreLogger().Warn("Specify a name of a DataItem");
                return name;
            }

            if (Regex.Match(name, @"([\w.]+?(->)\w+)").Success)
            {
                foreach (DataItem item in Items)
                    if (item.Name == name.Split(new[] { "->" }, System.StringSplitOptions.None)[0])
                        return item.GetData(name.Split(new[] { "->" }, System.StringSplitOptions.None)[1]);
            }
            else if (Regex.Match(name, @"([\w.]+?\([\w\""]+?\))").Success)
            {
                foreach (DataItem item in Items)
                    if (item.Name == name.Split(new[] { "(" }, System.StringSplitOptions.None)[0])
                        return item.GetData("(" + name.Split(new[] { "(" }, System.StringSplitOptions.None)[1].Replace("\"", ""));
            }
            foreach (DataItem item in Items)
                if (item.Name == name)
                    return item.ToString();
            Log.GetCoreLogger().Error("Could not find DataItem with name: {0}", name);
            return name;
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
                        string name = string.Join(".", group) + (group.Count > 0 ? "." : "") + line.Split(new char[] { ':' }, 2)[0];
                        for (int j = 0; j < Items.Count; j++)
                        {
                            if (line.Split(new char[] { ':' }, 2, System.StringSplitOptions.RemoveEmptyEntries).Length == 1)
                            {
                                if (parts[i + 1] == "[")
                                {
                                    i++;
                                    line += " ";
                                    int level = 0;
                                    for (i = i + 1; i < parts.Length; i++)
                                    {
                                        if (parts[i] == "[")
                                            level++;
                                        if (parts[i] == "]")
                                        {
                                            if (level == 0)
                                                break;
                                            level--;
                                        }
                                        line += (level > 0 ? Regex.Replace(parts[i], @",(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)", "|") : parts[i]);
                                    }
                                }
                                else if (parts[i + 1] == "{")
                                {
                                    i++;
                                    string groupName = name.Split('.')[name.Split('.').Length - 1];
                                    group.Add(groupName);
                                    break;
                                }
                                else
                                {
                                    Log.GetCoreLogger().Warn("No value assigned to {1}", path, name);
                                    break;
                                }
                            }

                            DataItem item = Items[j];
                            if (item.Name == name)
                            {
                                string d = line.Split(new char[] { ':' }, 2)[1];
                                string[] data = Regex.Split(d, @",(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)").Select(p => p.Trim().Replace("\"", "")).ToArray();

                                for (int k = 0; k < data.Length; k++)
                                {
                                    foreach (Match m in Regex.Matches(data[k], @"(?<=\$){(.+?)}"))
                                        data[k] = data[k].Replace("${" + m.Groups[1].Value + "}", GetDataItemValue(m.Groups[1].Value).ToString());

                                    foreach (Match m in Regex.Matches(data[k], @"(?<=\<)\?(.+?)>"))
                                        data[k] = data[k].Replace("<?" + m.Groups[1].Value + ">", CalculateExpression(m.Groups[1].Value));
                                }
                                item.SetData(data);
                                break;
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

        internal static string CalculateExpression(string expression)
        {
            List<string> RPN = new List<string>();
            Stack<string> operatorStack = new Stack<string>();
            foreach (Match m in Regex.Matches(expression, @"(-?)([\w])+|(-?)([\d])+|([\+\-\*\/\^\(\)])"))
            {
                string token = m.Value;
                if (token == "(" || token == ")" || 
                    token == "+" || token == "-" ||
                    token == "*" || token == "/" || token == "^")
                {
                    if (token == "(")
                    {
                        operatorStack.Push(token);
                        continue;
                    }
                    else if (token == "^")
                    {
                        operatorStack.Push(token);
                        continue;
                    }
                    else if (token == "*" || token == "/")
                    {
                        while (operatorStack.Count > 0 &&
                            operatorStack.Peek() != "+" && operatorStack.Peek() != "-" &&
                            operatorStack.Peek() != "(")
                            RPN.Add(operatorStack.Pop());
                        operatorStack.Push(token);
                        continue;
                    }
                    else if (token == "+" || token == "-")
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                            RPN.Add(operatorStack.Pop());
                        operatorStack.Push(token);
                        continue;
                    }
                    else if (token == ")")
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                            RPN.Add(operatorStack.Pop());
                        operatorStack.Pop();
                        continue;
                    }
                }
                else
                {
                    RPN.Add(token);
                    continue;
                }
            }
            while (operatorStack.Count > 0)
                RPN.Add(operatorStack.Pop());

            Stack<double> stack = new Stack<double>();
            foreach (string token in RPN)
            {
                if (token == "+" || token == "-" ||
                    token == "*" || token == "/" || token == "^")
                {
                    double right = stack.Pop();
                    double left = stack.Pop();
                    switch (token)
                    {
                        case "+":
                            stack.Push(left + right);
                            break;
                        case "-":
                            stack.Push(left - right);
                            break;
                        case "*":
                            stack.Push(left * right);
                            break;
                        case "/":
                            stack.Push(left / right);
                            break;
                        case "^":
                            stack.Push(System.Math.Pow(left, right));
                            break;
                    }
                }
                else
                {
                    stack.Push(double.Parse(token));
                }
            }

            return stack.Pop().ToString();
        }
    }
}
