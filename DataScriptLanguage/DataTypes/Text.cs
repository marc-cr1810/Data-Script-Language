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

        internal override string GetData(string data)
        {
            if (data.StartsWith("("))
            {
                Error("This is not an Array DataItem");
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
