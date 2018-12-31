namespace DataScriptLanguage.DataTypes
{
    public class Bool : DataItem
    {
        public bool Value { get; private set; } = false;

        public Bool(string name)
            : base(name)
        { }

        public Bool(string name, bool boolean)
            : base(name)
        {
            Value = boolean;
        }

        internal override void SetData(string[] data)
        {
            if (data.Length == 1)
            {
                switch (data[0].ToLower())
                {
                    case "true":
                        Value = true;
                        break;
                    case "t":
                        Value = true;
                        break;
                    case "1":
                        Value = true;
                        break;
                    case "false":
                        Value = false;
                        break;
                    case "f":
                        Value = false;
                        break;
                    case "0":
                        Value = false;
                        break;
                    default:
                        Error("Invalid bool value '{0}'", data[0]);
                        return;
                }

            }
            else
                Error("Invalid text arguments");
        }

        public static implicit operator bool(Bool t)
        {
            return t.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
