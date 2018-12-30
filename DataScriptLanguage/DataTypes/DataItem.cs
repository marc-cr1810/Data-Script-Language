using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScriptLanguage.DataTypes
{
    public abstract class DataItem
    {
        public readonly string Name = "";

        public DataItem(string name)
        {
            Name = name;
            DataScript.AddDataItem(this);
        }

        public virtual void SetData(string[] data)
        { }

        internal void Error(string msg)
        {
            Log.GetCoreLogger().Error("{0} : {1}", Name, msg);
        }

        public string GetName() { return Name; }

        public override string ToString()
        {
            return Name;
        }
    }
}
