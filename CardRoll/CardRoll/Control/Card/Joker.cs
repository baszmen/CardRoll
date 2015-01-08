using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace CardRoll.Control.Card
{
    public class Joker : CardObject
    {
        public Joker(Image image, int x, CardColor color, int y = 0)
            : base(image, x, y, color, CardType.Joker)
        {

        }
    }
}
