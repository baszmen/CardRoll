using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CardRoll.Control
{
    /// <summary>
    /// Kontroluje element, który jest przenoszony. Przechowuje jego referencje 
    /// oraz posiada pozycje aktualną oraz startową.
    /// </summary>
    public class CardMoveControl
    {
        public Point Start { get; set; }
        public Point Actual { get; set; }
        public FrameworkElement Element { get; set; }
    }
}
