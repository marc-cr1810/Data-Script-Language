﻿using System;
using System.Windows.Forms;
using DataScriptLanguage;
using DataScriptLanguage.DataTypes;

namespace Sandbox
{
    public partial class Window : Form
    {
        private Text Title = new Text("style.window.title");
        private Color ColorBackground = new Color("style.window.background_color");

        public Window()
        {
            InitializeComponent();
            
            DataScript.Read("resources.dsl");
            
            Text = Title;
            BackColor = ColorBackground;
        }
    }
}
