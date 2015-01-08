using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRoll.Control.Board
{
    /// <summary>
    /// Initialization and setup
    /// </summary>
    public class Board
    {
        public Field[][] BoardArray { get; set; }

        public Board(int rows, int columns)
        {
            this.BoardArray = new Field[rows][];
            for (int i = 0; i < rows; i++)
            {
                this.BoardArray[i] = new Field[columns];
            }
            #region Initializer
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this.BoardArray[i][j] = new Field();
                }
            }
            #endregion
        }
    }
}
