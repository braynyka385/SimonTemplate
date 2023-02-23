using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimonSays
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            //remove this screen and start the GameScreen
            Form1.ChangeScreen(this, new GameScreen());
        }


        private void exitButton_Click(object sender, EventArgs e)
        {
            //end the application
            Application.Exit();
        }

        private void MenuScreen_Load(object sender, EventArgs e)
        {

        }

        private void shuffleButton_Click(object sender, EventArgs e) //Enables/disables the shuffle game mode
        {
            Form1.shuffleMode = !Form1.shuffleMode;
            shuffleButton.BackColor = Color.Red;
            if (Form1.shuffleMode)
            {
                shuffleButton.BackColor = Color.Green;
            }
            Refresh();
        }
    }
}
