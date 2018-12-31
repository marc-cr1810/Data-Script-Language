using System.Windows.Forms;
using DataScriptLanguage;
using DataScriptLanguage.DataTypes;

namespace Sandbox
{
    public partial class Window : Form
    {
        private Text Title = new Text("style.window.title");
        private Array<Number> WinSize = new Array<Number>("style.window.size");
        private Color ColorBackground = new Color("style.window.color.background");

        private Array<Text> ConsoleMessage = new Array<Text>("lang.console.messages");

        private Dialog dialog;

        public Window()
        {
            InitializeComponent();
            
            DataScript.Read("../../examples/resources.dsl");
            
            Text = Title;
            Size = new System.Drawing.Size(WinSize["width"], WinSize["height"]);
            BackColor = ColorBackground;
            
            System.Console.WriteLine(ConsoleMessage["first"]);
            System.Console.WriteLine(ConsoleMessage["second"]);
            System.Console.WriteLine(ConsoleMessage["third"]);

            dialog = new Dialog();
            if ((Bool)DataScript.GetDataItem("style.dialog.show"))
            {
                dialog.ShowDialog();
            }
        }
    }
}
