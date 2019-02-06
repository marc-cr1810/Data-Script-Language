using System.Windows.Forms;
using DataScriptLanguage;
using DataScriptLanguage.DataTypes;

using Drawing = System.Drawing;

namespace Sandbox
{
    public partial class Window : Form
    {
        private Text Title = new Text("style.window.title");
        private Array<Number> WinSize = new Array<Number>("style.window.size");
        private Bool Resizeable = new Bool("style.window.resizeable");
        private Color ColorBackground = new Color("style.window.color.background");

        private Array<Array<Number>> PolyVertices = new Array<Array<Number>>("objects.poly.vertices");
        private Color PolyColor = new Color("objects.poly.color");

        private Array<Text> ConsoleMessage = new Array<Text>("lang.console.messages");

        private Dialog dialog;

        public Window()
        {
            InitializeComponent();
            
            DataScript.Read("../../examples/resources.dsl");
            
            Text = Title;
            Size = new Drawing.Size(WinSize["width"], WinSize["height"]);
            BackColor = ColorBackground;
            FormBorderStyle = (Resizeable ? FormBorderStyle.Sizable : FormBorderStyle.Fixed3D);
            MaximizeBox = Resizeable;
            
            for (int i = 0; i < ConsoleMessage.Count; i++)
                System.Console.WriteLine(ConsoleMessage[i]);

            dialog = new Dialog();
            if ((Bool)DataScript.GetDataItem("style.dialog.show"))
            {
                dialog.ShowDialog();
            }

            System.Console.WriteLine(WinSize["height"] - (15 * 2));
        }

        private void Window_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillPolygon(new Drawing.SolidBrush(PolyColor), new Drawing.Point[] {
                new Drawing.Point(PolyVertices["vert1"]["x"], PolyVertices["vert1"]["y"]),
                new Drawing.Point(PolyVertices["vert2"]["x"], PolyVertices["vert2"]["y"]),
                new Drawing.Point(PolyVertices["vert3"]["x"], PolyVertices["vert3"]["y"]),
                new Drawing.Point(PolyVertices["vert4"]["x"], PolyVertices["vert4"]["y"])
            });
            e.Graphics.DrawRectangle(Drawing.Pens.Red, PolyVertices["vert1"]["x"], PolyVertices["vert1"]["y"], PolyVertices["vert3"]["x"], PolyVertices["vert3"]["y"]);
        }
    }
}
