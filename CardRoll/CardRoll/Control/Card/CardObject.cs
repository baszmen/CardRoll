using System.Windows.Controls;

namespace CardRoll.Control.Card
{
    public abstract class CardObject
    {
        public Image Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public CardColor Color { get; set; }
        public CardType Type { get; set; }

        public CardObject(Image image, int x, int y, CardColor color, CardType type)
        {
            X = x;
            Y = y;
            Image = image;
            Color = color;
            Type = type;
        }
    }
}
