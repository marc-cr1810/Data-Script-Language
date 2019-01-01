using System.Windows.Forms;
using DataScriptLanguage;
using DataScriptLanguage.DataTypes;

namespace Sandbox
{
    public partial class Dialog : Form
    {
        private Text Title = new Text("style.dialog.title");
        private Array<Number> WinSize = new Array<Number>("style.dialog.size");
        private Bool DialogShow = new Bool("style.dialog.show");
        private Color ColorBackground = new Color("style.dialog.color.background");

        public Dialog()
        {
            InitializeComponent();

            DataScript.Read("../../examples/resources.dsl");

            Text = Title;
            Size = new System.Drawing.Size(WinSize["width"], WinSize["height"]);
            BackColor = ColorBackground;
        }
    }
}
