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

        internal void Warn(string message, params object[] args)
        {
            Log.GetCoreLogger().Warn("{0} : {1}", Name, message);
        }

        internal void Error(string message, params object[] args)
        {
            Log.GetCoreLogger().Error("{0} : {1}", Name, string.Format(message, args));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
