using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NChavesQGame.PlayForm;

namespace NChavesQGame
{
    public class Tile : PictureBox, ITile
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Types TileType { get; set; }

        public Tile(int row, int column, Types tileType)
        {
            this.Row = row;
            this.Column = column;
            this.TileType = tileType;
        }
    }
}
