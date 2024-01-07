/*
 * Programmed by: Nolan Chaves 
 * 
 * Revision History:
 *      04-NOV-2023: Project Started
 *						Built Control Panel Form and Designer Form
 *						Completed all functionality on control panel expect for the play button
 *						Built designer tool box, menu bar, and grid generator menu
 *						Completed the picturebox grid generator
 *						Completed the picturebox regenerator
 *						Completed validations
 *						
 *      05-NOV-2023: Completed the save functionality
 *						Tested project
 *						Completed Documentation
 *						Project Completed
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

namespace NChavesQGame
{
	/// <summary>
	/// Designer Form Class of NChavesQGame app
	/// </summary>
	public partial class Designer_Form : Form, ILevelDesigner
	{
		//Initializing Variables and Objects
		ImageList imageList;
		PictureBox pictureBox;
		Image selectedImage;
		List<PictureBox> pictureBoxes = new List<PictureBox>(); // List to track picture boxes


		int rows;
		int columns;
		int pictureBoxSize = 75;
		int wallCount;
		int doorCount;
		int boxCount;
		int filler = 100;

		Image wall = Properties.Resources.wall;
		Image redDoor = Properties.Resources.red_door;
		Image greenDoor = Properties.Resources.green_door;
		Image redBox = Properties.Resources.red_box;
		Image greenBox = Properties.Resources.green_box;

		/// <summary>
		/// Constructor of Assignment 2
		/// </summary>
		public Designer_Form()
		{
			InitializeComponent();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.Filter = "QGame|*.qgame";
			saveFileDialog.Title = "Save As";

			DialogResult result = saveFileDialog.ShowDialog();

			switch (result)
			{
				case DialogResult.OK:
					string filePath = saveFileDialog.FileName;
					SaveLevel(filePath);
					break;
				case DialogResult.Cancel:
					break;
				default:
					break;
			}
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		//This method will clear the previous generated picturebox with a new one based off the inputs from the grid generator menu  
		private void ClearPictureBoxGrid()
		{
			foreach (PictureBox picBox in pictureBoxes)
			{
				gameSpace.Controls.Remove(picBox); // Remove the picture box from the form
				picBox.Dispose(); // Dispose the picture box
			}
			pictureBoxes.Clear(); // Clear the list
		}

		//This method will prompt the user and tell them they will lose any work in their current level if they click yes to generating a new level (picturebox grid)
		private void GenerateNewLevelPrompt()
		{
			if (gameSpace.Controls.OfType<PictureBox>().Any())
			{
				DialogResult result = MessageBox.Show("Do you want to create a new level?\n" +
					"If you do, the current level will be lost.", "QGame",
					MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

				switch (result)
				{
					case DialogResult.Yes:
						ClearPictureBoxGrid();
						wallCount = 0;
						doorCount = 0;
						boxCount = 0;
						selectedImage = null;
						break;
					case DialogResult.No:
						break;
					default:
						break;
				}
			}
		}

		//This method checks the inputs in the grid generator menu and checks that they are not string and not negative numbers
		//Checks to see if they ae valid positive integers
		private void Validator()
		{
			//Checking if row and column are empty
			if (string.IsNullOrWhiteSpace(txtRows.Text) || string.IsNullOrWhiteSpace(txtColumns.Text))
			{
				MessageBox.Show("Please enter values for both rows and columns.", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Error);

				return;
			}

			//Checking if row is a valid int or a string
			else if (!int.TryParse(txtRows.Text, out rows))
			{
				MessageBox.Show("Please enter valid positive number for rows. \nLetters and words are not valid inputs", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//Checking if column is a valid int or a string
			else if (!int.TryParse(txtColumns.Text, out columns))
			{
				MessageBox.Show("Please enter valid positive number for columns. \nLetters and words are not valid inputs", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Checking if row is a positive integer
			else if (rows <= 0)
			{
				MessageBox.Show("Rows must be positive numbers.", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//Checking if cloumn is a positive integer
			else if (columns <= 0)
			{
				MessageBox.Show("Columns must be positive numbers.", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else
			{
				GenerateNewLevelPrompt();
			}
		}

		//This method will generate a grid based on the inputs in the menu bar
		private void GenerateGrid(int rows, int columns)
		{
			for (int row = 0; row < rows; row++)
			{
				for (int col = 0; col < columns; col++)
				{
					pictureBox = new PictureBox();
					pictureBox.Size = new Size(pictureBoxSize, pictureBoxSize);
					pictureBox.Location = new Point(col * pictureBox.Width, row * pictureBox.Height);
					pictureBox.BorderStyle = BorderStyle.FixedSingle;
					pictureBox.Click += pictureBox_Click; // Associate the common event handler

					gameSpace.Controls.Add(pictureBox);
					pictureBoxes.Add(pictureBox); // Add the new picture box to the list
                }
			}
			gameSpace.Size = new Size(pictureBoxSize * columns, pictureBoxSize * rows); //Matches the size of the picturebox grid

			if (this.ClientSize.Height < pictureBoxSize * rows + filler)
			{
                this.ClientSize = new Size(sidePanel.Width + pictureBoxSize * columns, pictureBoxSize * rows + filler);

            }
			else if (this.ClientSize.Height > pictureBoxSize * rows + filler)
			{
                this.ClientSize = new Size(panel1.Width, sidePanel.Height + filler);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
		{
			Validator();
			GenerateGrid(rows, columns);
		}

		private void btnNone_Click(object sender, EventArgs e)
		{
			selectedImage = null;
		}

		private void btnWall_Click(object sender, EventArgs e)
		{
			selectedImage = wall;
		}

		private void btnRedDoor_Click(object sender, EventArgs e)
		{
			selectedImage = redDoor;
		}

		private void btnGreenDoor_Click(object sender, EventArgs e)
		{
			selectedImage = greenDoor;
		}

		private void btnRedBox_Click(object sender, EventArgs e)
		{
			selectedImage = redBox;
		}

		private void btnGreenBox_Click(object sender, EventArgs e)
		{
			selectedImage = greenBox;
		}

		//This is designed to return the current image of a PictureBox if that image matches certain images
		private Image GetOldImage(PictureBox pictureBox)
		{
			if (pictureBox.Image == wall)
			{
				return wall;
			}
			else if (pictureBox.Image == greenDoor || pictureBox.Image == redDoor)
			{
				return pictureBox.Image;
			}
			else if (pictureBox.Image == greenBox || pictureBox.Image == redBox)
			{
				return pictureBox.Image;
			}
			return null; // No old image
		}

		//This is for placing the content inside the grid and the counting how many of each content is placed.
		//This also will check if there was a previous placed item before reclicking and removing it from the content count
		private void pictureBox_Click(object sender, EventArgs e)
		{
			PictureBox pictureBox = sender as PictureBox;

			if (pictureBox != null || selectedImage != null)
			{
				Image oldImage = GetOldImage(pictureBox);

				if (oldImage != null)
				{
					if (oldImage == wall)
					{
						wallCount--;
					}
					else if (oldImage == greenDoor || oldImage == redDoor)
					{
						doorCount--;
					}
					else if (oldImage == greenBox || oldImage == redBox)
					{
						boxCount--;
					}
				}

				pictureBox.Image = selectedImage;
				pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

				if (selectedImage == wall)
				{
					wallCount++;
				}
				else if (selectedImage == greenDoor || selectedImage == redDoor)
				{
					doorCount++;
				}
				else if (selectedImage == greenBox || selectedImage == redBox)
				{
					boxCount++;
				}
			}
		}

		//This interface is for saving the picturebox grid information so it can be used anywhere any save functionailies are needed
		public void SaveLevel(string filePath)
		{
			string textToSave = "";//this is where the info that goes in TXT file goes

			textToSave += $"{rows}\n" +
				$"{columns}";//This saves the size of the grid (row and columns)

			foreach (PictureBox pb in pictureBoxes)
			{
				int column = pb.Location.X / (pictureBox.Width);//This shows the column
				int row = pb.Location.Y / (pictureBox.Height);//This shows the rows

				string content = "";

				//This if statement will show what the content inside the picturebox depending on what was placed in it
				if (pb.Image == null)
				{
					content = "0";
				}
				else if (pb.Image == wall)
				{
					content = "1";
				}
				else if (pb.Image == redDoor)
				{
					content = "2";
				}
				else if (pb.Image == greenDoor)
				{
					content = "3";
				}
				else if (pb.Image == redBox)
				{
					content = "4";
				}
				else if (pb.Image == greenBox)
				{
					content = "5";
				}

				textToSave += $"\n" +
					$"{row}\n" +
					$"{column}\n" +
					$"{content}";
			}

            System.IO.File.WriteAllText(filePath, textToSave);

			MessageBox.Show($"File saved successfully:\n " +
				$"Total number of walls:{wallCount}\n " +
				$"Total number of doors: {doorCount}\n" +
				$"Total number of boxes: {boxCount}", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void CreateGrid(int rows, int columns)
		{
			throw new NotImplementedException();
		}
	}
}
