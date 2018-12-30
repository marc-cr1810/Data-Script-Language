using System.Windows.Forms;
using DataScriptLanguage;
using DataScriptLanguage.DataTypes;

namespace Sandbox
{
    public partial class Window : Form
    {
        private Text Title = new Text("style.window.title");
        private Color ColorBackground = new Color("style.window.color.background");

        private Dialog dialog;

        public Window()
        {
            InitializeComponent();
            
            DataScript.Read("../../examples/resources.dsl");
            
            Text = Title;
            BackColor = ColorBackground;

            dialog = new Dialog();
            if ((Bool)DataScript.GetDataItem("style.dialog.show"))
            {
                dialog.ShowDialog();
            }
        }
    }
}
