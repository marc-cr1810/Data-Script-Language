using System.Windows.Forms;
using DataScriptLanguage;
using DataScriptLanguage.DataTypes;

namespace Sandbox
{
    public partial class Dialog : Form
    {
        private Text Title = new Text("style.dialog.title");
        private Color ColorBackground = new Color("style.dialog.color.background");
        private Bool DialogShow = new Bool("style.dialog.show");

        public Dialog()
        {
            InitializeComponent();

            DataScript.Read("../../examples/resources.dsl");

            Text = Title;
            BackColor = ColorBackground;
        }
    }
}
