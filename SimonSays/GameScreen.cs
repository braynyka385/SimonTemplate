using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        int waitTime = 1200;
        int guessVar = 0;
        Random random = new Random();
        SoundPlayer greenSound = new SoundPlayer(Properties.Resources.green);
        SoundPlayer redSound = new SoundPlayer(Properties.Resources.red);
        SoundPlayer yellowSound = new SoundPlayer(Properties.Resources.yellow);
        SoundPlayer blueSound = new SoundPlayer(Properties.Resources.blue);
        SoundPlayer mistakeSound = new SoundPlayer(Properties.Resources.mistake);

        GameTile[] gameTiles = new GameTile[4];


        public GameScreen()
        {
            InitializeComponent();
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            gameTiles[0] = new GameTile(greenButton, Color.ForestGreen, Color.LightGreen, greenSound);
            gameTiles[1] = new GameTile(redButton, Color.DarkRed, Color.OrangeRed, redSound);
            gameTiles[2] = new GameTile(yellowButton, Color.Goldenrod, Color.LightYellow, yellowSound);
            gameTiles[3] = new GameTile(blueButton, Color.DarkBlue, Color.LightBlue, blueSound);

            Blah();
            Form1.coloursList.Clear();
            Refresh();
            Thread.Sleep(1000);
            ComputerTurn();
        }

        private void Blah()
        {
            GraphicsPath circlePath = new GraphicsPath();
            int width = 90 * 2;
            circlePath.AddEllipse(5, 5, width, width);

            Region buttonRegion = new Region(circlePath);

            buttonRegion.Exclude(new Rectangle(0, width/2, width/2, 500)); //remove bottom line 
            buttonRegion.Exclude(new Rectangle(width/2, 0, 500, 500)); //remove right line 


            gameTiles[0].button.Region = buttonRegion;
     
            Matrix transformMatrix = new Matrix();

            transformMatrix.RotateAt(90, new PointF(50, 50));


            for (int i = 1; i < gameTiles.Length; i++)
            {
                buttonRegion.Transform(transformMatrix);

                gameTiles[i].button.Region = buttonRegion;
                transformMatrix.RotateAt(90, new PointF(50, 50));
            }
        }
  

        private void ComputerTurn()
        {
            foreach(GameTile tile in gameTiles)
            {
                tile.button.BackColor = tile.defaultColor;
            }
            Refresh();
            Thread.Sleep(waitTime / 4);
            waitTime -= 40;
            Form1.coloursList.Add(random.Next(0, 4));
            for(int i = 0; i < Form1.coloursList.Count; i++)
            {
                foreach (GameTile tile in gameTiles)
                {
                    tile.button.BackColor = tile.defaultColor;
                }

                Refresh();
                Thread.Sleep(waitTime / 4);

                
                gameTiles[Form1.coloursList[i]].Next();
                Refresh();
                Thread.Sleep(waitTime);
                foreach (GameTile item in gameTiles)
                {
                    item.button.BackColor = item.defaultColor;
                }
            }
            //TODO: set guess value back to 0
            guessVar = 0;
        }

        //TODO: create one of these event methods for each button
        private void greenButton_Click(object sender, EventArgs e)
        {
            ButtonClicked(sender, e, 0);
        }
        private void redButton_Click(object sender, EventArgs e)
        {
            ButtonClicked(sender, e, 1);
        }

        private void yellowButton_Click(object sender, EventArgs e)
        {
            ButtonClicked(sender, e, 2);
        }

        private void blueButton_Click(object sender, EventArgs e)
        {
            ButtonClicked(sender, e, 3);

        }

        private void ButtonClicked(object sender, EventArgs e, int buttonNum)
        {
            if (Form1.coloursList[guessVar] == buttonNum)
            {
                GameTile t = gameTiles[buttonNum];
                t.button.BackColor = t.pickedColor;
                t.soundPlayer.Play();
                Refresh();
                Thread.Sleep(waitTime);
                t.button.BackColor=t.defaultColor;
                blueButton.BackColor = Color.DarkBlue;
                guessVar++;
                Refresh();

            }
            else
            {
                GameOver();
            }
            if (guessVar >= Form1.coloursList.Count)
            {
                ComputerTurn();
            }
        }
        public void GameOver()
        {
            //TODO: Play a game over sound
            mistakeSound.Play();
            //TODO: close this screen and open the GameOverScreen
            Form1.ChangeScreen(this, new GameOverScreen());
        }

        
    }

    public class GameTile
    {
        public Button button;
        public Color defaultColor;
        public Color pickedColor;
        public SoundPlayer soundPlayer;

        public GameTile(Button b, Color c1, Color c2, SoundPlayer s)
        {
            button = b;
            defaultColor = c1;
            pickedColor = c2;
            soundPlayer = s;
        }
        public void Next()
        {
            this.button.BackColor = pickedColor;
            this.soundPlayer.Play();
        }
    }
}
