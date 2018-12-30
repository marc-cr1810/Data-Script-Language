using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScriptLanguage.DataTypes
{
    public class Text : DataItem
    {
        public string Value { get; private set; } = "";

        public Text(string name) 
            : base(name)
        {
            Value = name;
        }

        public Text(string name, string text) 
            : base(name)
        {
            Value = text;
        }

        public override void SetData(string[] data)
        {
            if (data.Length == 1)
                Value = data[0];
            else
                Error("Invalid text arguments");
        }

        public static implicit operator string(Text t)
        {
            return t.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
