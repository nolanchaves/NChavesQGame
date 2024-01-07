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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NChavesQGame
{
	/// <summary>
	/// Control Panel Class of NChavesQGame app
	/// </summary>
	public partial class Form1 : Form
	{
		/// <summary>
		/// Constructor of Assignment 2
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}

		//This button will close the Control Panel form, closing the game
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		//This button will allow a user to play a selected level 
		//Will be completed in assignment 3
		private void btnPlay_Click(object sender, EventArgs e)
		{
			PlayForm playForm = new PlayForm();
            playForm.Show();
        }

		//This button will open the designer form so a level can be generated and created
		private void btnDesign_Click(object sender, EventArgs e)
		{
			Designer_Form designer_Form = new Designer_Form();
			designer_Form.Show();
		}
	}
}
