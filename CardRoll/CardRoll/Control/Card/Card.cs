using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace CardRoll.Control.Card
{
    public class Card : CardObject
    {
        public Card(Image image, int x, int y, CardColor color, CardType type)
            : base(image, x, y, color, type)
        {
        }
    }
}
