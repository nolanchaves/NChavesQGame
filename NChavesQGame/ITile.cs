using static NChavesQGame.PlayForm;

namespace NChavesQGame
{
    public interface ITile
    {
        int Column { get; set; }
        int Row { get; set; }
        Types TileType { get; set; }
    }
}