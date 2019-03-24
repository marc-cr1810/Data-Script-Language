namespace DataScriptLanguage.DataTypes
{
    public class Color : DataItem
    {
        public int A { get; private set; } = 255;
        public int R { get; private set; } = 255;
        public int G { get; private set; } = 0;
        public int B { get; private set; } = 255;

        public Color(string name)
            : base(name)
        { }

        public Color(string name, System.Drawing.Color color)
            : base(name)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public Color(string name, string hex)
            : base(name)
        {
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hex);
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public Color(string name, int r, int g, int b, int a = 255)
            : base(name)
        {
            SetData(r, b, b, a);
        }
        
        public override void SetData(string[] data)
        {
            if (data.Length == 1)
            {
                if (!data[0].StartsWith("#"))
                {
                    Error("Invalid HEX color code");
                    return;
                }
                SetData(data[0]);
            }
            else if (data.Length == 3)
            {
                int r = int.Parse(data[0]);
                int g = int.Parse(data[1]);
                int b = int.Parse(data[2]);
                SetData(r, g, b);
            }
            else if (data.Length == 4)
            {
                int r = int.Parse(data[0]);
                int g = int.Parse(data[1]);
                int b = int.Parse(data[2]);
                int a = int.Parse(data[3]);
                SetData(r, g, b, a);
            }
            else
            {
                if (data.Length > 4)
                {
                    string errMsg = "To many arguments ([R<-{0}, G<-{1}, B<-{2}, A<-{3}, ";
                    for (int i = 4; i < data.Length; i++)
                        if (i == data.Length - 1)
                            errMsg += "?<-{" + i.ToString() + "}])";
                        else
                            errMsg += "?<-{" + i.ToString() + "}, ";
                    Error(errMsg, data);
                }
                else
                    Error("Invalid color arguments");
            }
        }

        internal void SetData(string hex)
        {
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hex);
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        internal void SetData(int r, int g, int b, int a = 255)
        {
            A = a;
            R = r;
            G = g;
            B = b;
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
            else if (d == "r" || d == "red")
                return R.ToString();
            else if (d == "g" || d == "green")
                return G.ToString();
            else if (d == "b" || d == "blue")
                return B.ToString();
            else if (d == "a" || d == "alpha")
                return A.ToString();
            else
            {
                Error("Unknown data value ({0})", data);
                return Name;
            }
        }

        public static implicit operator System.Drawing.Color(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public override string ToString()
        {
            return System.Drawing.ColorTranslator.ToHtml(this);
        }
    }
}
