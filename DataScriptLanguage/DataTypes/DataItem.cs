namespace DataScriptLanguage.DataTypes
{
    public class DataItem
    {
        public readonly string Name = "";

        public DataItem(string name)
        {
            Name = name;
            DataScript.AddDataItem(this);
        }

        internal DataItem(string name, bool addItem = false)
        {
            Name = name;
            if (addItem)
                DataScript.AddDataItem(this);
        }

        internal virtual void SetData(string[] data)
        { }

        internal virtual string GetData(string data)
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

        public override string ToString()
        {
            return Name;
        }

        internal void Warn(string message, params object[] args)
        {
            Log.GetCoreLogger().Warn("{0} : {1}", Name, message);
        }

        internal void Error(string message, params object[] args)
        {
            Log.GetCoreLogger().Error("{0} : {1}", Name, string.Format(message, args));
        }
    }
}
