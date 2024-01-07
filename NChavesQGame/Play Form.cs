/*
 * Programmed by: Nolan Chaves 
 * 
 * Revision History:
 * 
 *      20-NOV-2023: Project Started
 *                      Created and initialized all the variables
 *                      Created the buttons, gamespace, textboxes, labels and file menu
 *                      Created the openlevel method to generate the level from the txt file
 *                      Created Tile event handler, buttons and restart level methods
 *                      
 *      28-NOV-2023: Created a Tile class that inherits from PictureBox
 *                      Created the MoveTile Method
 *                      Created the IsOverlap Method
 *                      Added the methods to the buttons
 *                      
 *						
 *      03-DEC-2023: Completed the Move functionality
 *						Tested project
 *						Completed Documentation
 *						Project Completed
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NChavesQGame
{
    public partial class PlayForm : Form
    {
        //Initializing Variables
        List<Tile> tileList = new List<Tile>();
        Tile selectedTile = null;

        int rows;
        int columns;
        int tileSize = 75;
        int moveSpeed = 75;
        int boxCount = 0;
        int movesMade = 0;
        int filler = 100;
        int gameSpaceWidth = 470;
        int gameSpaceHeight = 392;

        Image wall = Properties.Resources.wall;
        Image redDoor = Properties.Resources.red_door;
        Image greenDoor = Properties.Resources.green_door;
        Image redBox = Properties.Resources.red_box;
        Image greenBox = Properties.Resources.green_box;

        /// <summary>
        /// Constructor of Assignment 3
        /// </summary>
        public PlayForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "QGame|*.qgame";
            openFileDialog.Title = "Load Game";
            DialogResult result = openFileDialog.ShowDialog();

            switch (result)
            {
                case DialogResult.OK:
                    string filePath = openFileDialog.FileName;
                    OpenLevel(filePath);
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// To Turn The Buttons On To Move The Boxes
        /// Allows one method to turn buttons on or off
        /// </summary>
        /// <param name="button"></param>
        private void Buttons(bool button)
        {
            btnUp.Enabled = button;
            btnDown.Enabled = button;
            btnLeft.Enabled = button;
            btnRight.Enabled = button;
        }

        /// <summary>
        /// Open Level method thats gets passed the the openToolStripMenu
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenLevel(string filePath)
        {
            ResetLevel();

            Dictionary<int, System.Drawing.Image> tileDictionary = new Dictionary<int, System.Drawing.Image>
                {
                    {0, null},
                    {1, wall},
                    {2, redDoor},
                    {3, greenDoor},
                    {4, redBox},
                    {5, greenBox}

                };
            Dictionary<int, Types> tileTypeDictionary = new Dictionary<int, Types>
                {
                    {0, Types.Empty},
                    {1, Types.Wall},
                    {2, Types.Door},
                    {3, Types.Door},
                    {4, Types.Box},
                    {5, Types.Box}
                };

            List<string> lines = new List<string>(File.ReadAllLines(filePath));

            rows = int.Parse(lines[0]);
            columns = int.Parse(lines[1]);

            int tileWidth = tileSize * columns;
            int tileHeight = tileSize * rows;

            gameSpace.Size = new Size(tileWidth, tileHeight);

            if (controlPanel.Height > tileHeight)
            {
                this.ClientSize = new Size(controlPanel.Width + tileWidth + filler, controlPanel.Height + filler);
            }
            else
            {
                this.ClientSize = new Size(controlPanel.Width + tileWidth + filler, tileHeight + filler);
            }


            for (int i = 2; i < lines.Count; i += 3) // Increment by 3 to read three values at a time
            {
                int row = int.Parse(lines[i]);
                int column = int.Parse(lines[i + 1]);
                int tileType = int.Parse(lines[i + 2]);
                Types type = tileTypeDictionary[tileType];


                Tile tile = new Tile(row, column, type);


                tile.Size = new Size(tileSize, tileSize);
                tile.Location = new Point(column * tile.Width, row * tile.Height);
                tile.BorderStyle = BorderStyle.FixedSingle;
                tile.SizeMode = PictureBoxSizeMode.StretchImage;
                tile.Click += tile_Click;
                tile.Enabled = true;


                if (tileDictionary.ContainsKey(tileType))
                {
                    System.Drawing.Image content = tileDictionary[tileType];
                    tile.Image = content;
                }

                if (tile.Image == wall || tile.Image == redDoor || tile.Image == greenDoor)
                {
                    tile.Enabled = false;
                }

                gameSpace.Controls.Add(tile);
                tileList.Add(tile);

                if (tile.TileType == Types.Box)
                {
                    boxCount++;
                }
                txtBoxesLeft.Text = boxCount.ToString();
                txtMovesMade.Text = movesMade.ToString();

                Buttons(true);

                foreach (Tile t in tileList.ToList())
                {
                    if (t.TileType == Types.Empty)
                    {
                        gameSpace.Controls.Remove(t);
                        tileList.Remove(t);
                    }
                }
            }
        }

        /// <summary>
        /// Tile event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tile_Click(object sender, EventArgs e)
        {
            Tile clickedTile = sender as Tile;

            if (clickedTile == selectedTile)
            {
                clickedTile.BorderStyle = BorderStyle.None;
                selectedTile = null;
            }
            else
            {
                if (selectedTile != null && selectedTile.Enabled == false)
                {
                    selectedTile.BorderStyle = BorderStyle.None;
                }
                clickedTile.BorderStyle = BorderStyle.FixedSingle;
                selectedTile = clickedTile;
            }
        }

        /// <summary>
        /// Checks if there is an overlap and returns true or false is there is
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        private bool IsOverlap(int newX, int newY, Tile tile)
        {
            foreach (Tile otherTile in tileList.ToList())
            {
                if (otherTile == selectedTile)
                    continue;

                if (new Rectangle(newX, newY, tile.Width, tile.Height).IntersectsWith(otherTile.Bounds))
                {
                    if (otherTile.Image == wall)
                    {
                        return true;
                    }
                    if (otherTile.TileType == Types.Box && selectedTile.TileType == Types.Box)
                    {
                        return true;
                    }
                    if (otherTile.Image == greenDoor && selectedTile.Image == redBox)
                    {
                        return true;
                    }
                    if (otherTile.Image == redDoor && selectedTile.Image == greenBox)
                    {
                        return true;
                    }
                    if (otherTile.Image == redDoor && selectedTile.Image == redBox)
                    {
                        gameSpace.Controls.Remove(selectedTile);
                        boxCount--;
                        txtBoxesLeft.Text = boxCount.ToString();
                        break;
                    }
                    if (otherTile.Image == greenDoor && selectedTile.Image == greenBox)
                    {
                        gameSpace.Controls.Remove(selectedTile);
                        boxCount--;
                        txtBoxesLeft.Text = boxCount.ToString();
                        break;
                    }
                }
            }
            if (newX < 0 || newY < 0 || newX + tile.Width > gameSpace.Width || newY + tile.Height > gameSpace.Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To Restart The GameSpace after finishing the game
        /// Resets the GameSpace to A Default Size
        /// Sets MovesMade and BoxCount to 0
        /// Disables Buttons Until New Level Loaded
        /// </summary>
        public void ResetLevel()
        {
            foreach (Tile tile in tileList)
            {
                gameSpace.Controls.Remove(tile);
                tile.Dispose();
            }
            tileList.Clear();

            boxCount = 0;
            movesMade = 0;

            txtMovesMade.Text = movesMade.ToString();
            txtBoxesLeft.Text = boxCount.ToString();

            gameSpace.Size = new Size(gameSpaceWidth, gameSpaceHeight);
            this.ClientSize = new Size(controlPanel.Width + gameSpaceWidth + filler, gameSpaceHeight + filler);

            Buttons(false);
        }

        /// <summary>
        /// Used to make code less repetetive and move the selectedbox in chosen direction
        /// </summary>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        private void MoveTile(int moveX, int moveY)
        {
            if (selectedTile == null)
            {
                MessageBox.Show("Click to select a box", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int originalX = selectedTile.Location.X;
                int originalY = selectedTile.Location.Y;

                int newX = originalX + moveX;
                int newY = originalY + moveY;

                bool hasOverlap = false; 

                while (!IsOverlap(newX, newY, selectedTile))
                {
                    selectedTile.Location = new Point(newX, newY);

                    hasOverlap = true;

                    newX += moveX;
                    newY += moveY;
                }
                movesMade++;
                txtMovesMade.Text = movesMade.ToString();

                if (!hasOverlap)
                {
                    movesMade--;
                }

                txtMovesMade.Text = movesMade.ToString();

                if (boxCount == 0)
                {
                    DialogResult result = MessageBox.Show("Congratulations \nEnd Game", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    switch (result)
                    {
                        case DialogResult.OK:
                            ResetLevel();
                            break;
                    }
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            MoveTile(0, -moveSpeed);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            MoveTile(0, moveSpeed);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            MoveTile(-moveSpeed, 0);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            MoveTile(moveSpeed, 0);
        }

        public enum Types
        {
            Empty,
            Wall,
            Box,
            Door
        }

    }
}

