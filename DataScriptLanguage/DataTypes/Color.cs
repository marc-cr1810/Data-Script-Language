using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                int a = int.Parse(data[0]);
                int r = int.Parse(data[1]);
                int g = int.Parse(data[2]);
                int b = int.Parse(data[3]);
                SetData(r, g, b, a);
            }
            else Error("Invalid color arguments");
        }

        public void SetData(string hex)
        {
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hex);
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public void SetData(int r, int g, int b, int a = 255)
        {
            A = a;
            R = r;
            G = g;
            B = b;
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
