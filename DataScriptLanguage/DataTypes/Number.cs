namespace DataScriptLanguage.DataTypes
{
    public class Number : DataItem
    {
        private enum NumberType
        {
            None, Int, Double, Float
        }
        private NumberType Type = NumberType.None;

        public int ValueInt { get; private set; } = 0;
        public double ValueDouble { get; private set; } = 0d;
        public float ValueFloat { get; private set; } = 0f;

        public Number(string name) 
            : base(name)
        { }

        public Number(string name, int i) 
            : base(name)
        {
            ValueInt = i;
            Type = NumberType.Int;
        }

        public Number(string name, double d) 
            : base(name)
        {
            ValueDouble = d;
            Type = NumberType.Double;
        }

        public Number(string name, float f) 
            : base(name)
        {
            ValueFloat = f;
            Type = NumberType.Float;
        }

        internal override void SetData(string[] data)
        {
            if (data.Length == 1)
            {
                char t = ' ';
                if (char.IsLetter(data[0][data[0].Length - 1]))
                {
                    t = data[0][data[0].Length - 1].ToString().ToLower()[0];
                    data[0] = data[0].Substring(0, data[0].Length - 1);
                }

                if (data[0].EndsWith(".")) data[0] = data[0].Substring(0, data[0].Length - 1);

                if (data[0].Contains("."))
                {
                    if (t == 'd') { ValueDouble = double.Parse(data[0]); Type = NumberType.Double; }
                    else if (t == 'f') { ValueFloat = float.Parse(data[0]); Type = NumberType.Float; }
                    else { ValueDouble = double.Parse(data[0]); Type = NumberType.Double; }
                }
                else
                {
                    if (t == 'd') { ValueDouble = double.Parse(data[0]); Type = NumberType.Double; }
                    else if (t == 'f') { ValueFloat = float.Parse(data[0]); Type = NumberType.Float; }
                    else { ValueInt = int.Parse(data[0]); Type = NumberType.Int; }
                }
            }
            else
                Error("Invalid number arguments");
        }

        public static implicit operator int(Number n)
        {
            return (n.Type == NumberType.Int) ? n.ValueInt :
                   (n.Type == NumberType.Double) ? (int)n.ValueDouble :
                   (n.Type == NumberType.Float) ? (int)n.ValueFloat : 0;
        }

        public static implicit operator double(Number n)
        {
            return (n.Type == NumberType.Int) ? n.ValueInt :
                   (n.Type == NumberType.Double) ? n.ValueDouble :
                   (n.Type == NumberType.Float) ? (double)n.ValueFloat : 0;
        }

        public static implicit operator float(Number n)
        {
            return (n.Type == NumberType.Int) ? n.ValueInt :
                   (n.Type == NumberType.Double) ? (float)n.ValueDouble :
                   (n.Type == NumberType.Float) ? n.ValueFloat : 0;
        }

        public override string ToString()
        {
            return (Type == NumberType.Int) ? ValueInt.ToString() :
                   (Type == NumberType.Double) ? ValueDouble.ToString() + "d" :
                   (Type == NumberType.Float) ? ValueFloat.ToString() + "f" :
                   "0";
        }
    }
}
