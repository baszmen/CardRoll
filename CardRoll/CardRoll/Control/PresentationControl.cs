using System;
using System.Diagnostics;
using System.Linq;
using CardRoll.Control.Card;
using CardRoll.Exceptions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CardRoll.Helpers;

namespace CardRoll.Control
{
    public class PresentationControl
    {
        #region Fields
        private readonly int _margin;
        private readonly int _width;
        private readonly int _cardSize;
        private readonly int _jokerBoardSize;
        private readonly int _gameBoardSize;
        private readonly int _newBoardSize;
        private readonly Canvas _canvas;
        private CardLoader _loader;
        public int TopJokerBoard { get; set; }
        public int TopGameBoard { get { return TopJokerBoard + _jokerBoardSize; } }
        public int TopNewBoard
        {
            get
            {
                return TopGameBoard + _gameBoardSize;
            }
        }

        #endregion

        #region Boards

        public Board.Board GameBoard { get; set; }
        public Board.Board JokerBoard { get; set; }
        public Board.Board NewBoard { get; set; }

        public KeyValuePair<BoardType, KeyValuePair<int, int>> PlaceRecognition { get; set; }
        public KeyValuePair<BoardType, KeyValuePair<int, int>> StartPlaceRecognition { get; set; }
        #endregion

        public PresentationControl(
            int topJokerBoard,
            int jokerBoardHeight,
            int gameBoardHeight,
            int newBoardHeight,
            int margin,
            int width,
            int cardSize,
            Canvas canvas)
        {
            _jokerBoardSize = jokerBoardHeight;
            _gameBoardSize = gameBoardHeight;
            _newBoardSize = newBoardHeight;

            TopJokerBoard = topJokerBoard;

            _margin = margin;
            _width = width;
            _cardSize = cardSize;

            #region BoardInitialization

            GameBoard = new Board.Board(4, 4);
            JokerBoard = new Board.Board(1, 2);
            NewBoard = new Board.Board(1, 4);

            #endregion

            _canvas = canvas;
            _loader = new CardLoader();
        }

        /// <summary>
        /// Recognize in which board there was an action
        /// </summary>
        /// <param name="sender"></param>
        /// <returns> KeyVAluePair that represents the type of board and coordinates of field</returns>
        public PresentationControl RecognizeBoard(object sender)
        {
            #region Configuration

            if (!(sender is FrameworkElement))
                throw new ArgumentException();

            var element = sender as FrameworkElement;
            var left = Canvas.GetLeft(element);
            var top = Canvas.GetTop(element);

            top += _cardSize / 2;
            left += _cardSize / 2;
            #endregion

            #region LeftCheck

            if ((left < _margin) || ((_width - left) < _margin))
            {
                throw new OutOfBoardException();
            }


            #endregion

            #region RightCheck

            #region JokerBoard
            if (top > TopJokerBoard && top < TopJokerBoard + _jokerBoardSize)
            {
                if ((left - _margin) > 2 * _cardSize)
                {
                    throw new OutOfBoardException();
                }
                PlaceRecognition = (left - _margin) > 100 ?
                                       new KeyValuePair<BoardType, KeyValuePair<int, int>>(BoardType.JokerBoard, new KeyValuePair<int, int>(1, 0)) :
                                       new KeyValuePair<BoardType, KeyValuePair<int, int>>(BoardType.JokerBoard, new KeyValuePair<int, int>(0, 0));
            }

            #endregion

            #region
            else if (top > TopGameBoard && top < TopGameBoard + _gameBoardSize)
            {
                PlaceRecognition = new KeyValuePair<BoardType, KeyValuePair<int, int>>(
                           BoardType.GameBoard,
                           new KeyValuePair<int, int>(
                               (int)((left - _margin) / 100) % 4,
                               (int)((top - TopGameBoard) / 100) % 4));
            }

            #endregion

            #region NewBoard
            else if (top > TopNewBoard && top < TopNewBoard + _newBoardSize)
            {
                PlaceRecognition = new KeyValuePair<BoardType, KeyValuePair<int, int>>(
                           BoardType.NewBoard,
                           new KeyValuePair<int, int>(
                               (int)((left - _margin) / 100) % 4,
                               0));
            }
            #endregion

            #region OutOfBoardHorizontal

            else
            {
                throw new OutOfBoardException();
            }

            #endregion

            #endregion

            return this;
        }

        public void RecognizeBoard(double left, double top)
        {
            #region Configuration

            top += _cardSize / 2;
            left += _cardSize / 2;
            #endregion

            #region LeftCheck

            if ((left < _margin) || ((_width - left) < _margin))
            {
                throw new OutOfBoardException();
            }


            #endregion

            #region RightCheck

            #region JokerBoard
            if (top > TopJokerBoard && top < TopJokerBoard + _jokerBoardSize)
            {
                if ((left - _margin) > 2 * _cardSize)
                {
                    throw new OutOfBoardException();
                }
                StartPlaceRecognition = (left - _margin) > 100 ?
                                       new KeyValuePair<BoardType, KeyValuePair<int, int>>(BoardType.JokerBoard, new KeyValuePair<int, int>(1, 0)) :
                                       new KeyValuePair<BoardType, KeyValuePair<int, int>>(BoardType.JokerBoard, new KeyValuePair<int, int>(0, 0));
            }

            #endregion

            #region
            else if (top > TopGameBoard && top < TopGameBoard + _gameBoardSize)
            {
                StartPlaceRecognition = new KeyValuePair<BoardType, KeyValuePair<int, int>>(
                           BoardType.GameBoard,
                           new KeyValuePair<int, int>(
                               (int)((left - _margin) / 100) % 4,
                               (int)((top - TopGameBoard) / 100) % 4));
            }

            #endregion

            #region NewBoard
            else if (top > TopNewBoard && top < TopNewBoard + _newBoardSize)
            {
                StartPlaceRecognition = new KeyValuePair<BoardType, KeyValuePair<int, int>>(
                           BoardType.NewBoard,
                           new KeyValuePair<int, int>(
                               (int)((left - _margin) / 100) % 4,
                               0));
            }
            #endregion

            #region OutOfBoardHorizontal

            else
            {
                throw new OutOfBoardException();
            }

            #endregion

            #endregion
        }

        /// <summary>
        /// Recognize in which board there was an action
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns> KeyVAluePair that represents the type of board and coordinates of field</returns>
        public PresentationControl ReleaseCard(double left, double top)
        {
            RecognizeBoard(left, top);

            CardObject card;

            switch (StartPlaceRecognition.Key)
            {
                case BoardType.JokerBoard:
                    switch (PlaceRecognition.Key)
                    {
                        case BoardType.JokerBoard:
                            card = JokerBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            JokerBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                        case BoardType.GameBoard:
                            card = JokerBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            GameBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                        default:
                            card = JokerBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            NewBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                    }
                    break;
                case BoardType.GameBoard:
                    switch (PlaceRecognition.Key)
                    {
                        case BoardType.JokerBoard:
                            card = GameBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            JokerBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                        case BoardType.GameBoard:
                            card = GameBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            GameBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                        default:
                            card = GameBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            NewBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                    }
                    break;
                default:
                    switch (PlaceRecognition.Key)
                    {
                        case BoardType.JokerBoard:
                            card = NewBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            JokerBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                        case BoardType.GameBoard:
                            card = NewBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            GameBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                        default:
                            card = NewBoard.BoardArray[StartPlaceRecognition.Value.Value][StartPlaceRecognition.Value.Key].RemoveCard();
                            NewBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].ReleaseCard(card);
                            break;
                    }
                    break;
            }

            return this;
        }




        /// <summary>
        /// Returns exact position of place coords on board. (X,Y)
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int, int> GetPositionCoords()
        {
            /*
             * Konieczne odejmowanie 100 i dzielenie przez 2 ponieważ stackapanele są wyższe niż ich content
             */

            switch (PlaceRecognition.Key)
            {
                case BoardType.JokerBoard:
                    return new KeyValuePair<int, int>(_margin + (PlaceRecognition.Value.Key * 100), TopJokerBoard + (_jokerBoardSize - 100) / 2);
                case BoardType.GameBoard:
                    return new KeyValuePair<int, int>(_margin + (PlaceRecognition.Value.Key * 100), TopGameBoard + (PlaceRecognition.Value.Value * 100));
                default:
                    return new KeyValuePair<int, int>(_margin + (PlaceRecognition.Value.Key * 100), TopNewBoard + (_newBoardSize - 100) / 2);
            }
        }

        /// <summary>
        /// Returns exact position of place coords on board. (X,Y)
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int, int> GetPositionCoords(KeyValuePair<BoardType, KeyValuePair<int, int>> conf)
        {
            /*
             * Konieczne odejmowanie 100 i dzielenie przez 2 ponieważ stackapanele są wyższe niż ich content
             */

            switch (conf.Key)
            {
                case BoardType.JokerBoard:
                    return new KeyValuePair<int, int>(_margin + (conf.Value.Key * 100), TopJokerBoard + (_jokerBoardSize - 100) / 2);
                case BoardType.GameBoard:
                    return new KeyValuePair<int, int>(_margin + (conf.Value.Key * 100), TopGameBoard + (conf.Value.Value * 100));
                default:
                    return new KeyValuePair<int, int>(_margin + (conf.Value.Key * 100), TopNewBoard + (_newBoardSize - 100) / 2);
            }
        }

        /// <summary>
        /// Check if card can be released
        /// </summary>
        /// <returns></returns>
        public PresentationControl CanReleaseCard()
        {
            switch (PlaceRecognition.Key)
            {
                case BoardType.GameBoard:
                    if (!GameBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].IsEmpty)
                        throw new DestinationLockedException();
                    break;
                case BoardType.JokerBoard:
                    if (!JokerBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].IsEmpty)
                        throw new DestinationLockedException();
                    break;
                default:
                    if (!NewBoard.BoardArray[PlaceRecognition.Value.Value][PlaceRecognition.Value.Key].IsEmpty)
                        throw new DestinationLockedException();
                    break;
            }

            return this;
        }

        public List<KeyValuePair<FrameworkElement, int>> FourCardRoll()
        {
            var results = new List<KeyValuePair<FrameworkElement, int>>();

            NewBoard.BoardArray[0].ToList().ForEach(field =>
                {
                    if (!field.IsEmpty)
                        throw new LevelNotCompleted();
                });

            for (var i = 0; i < 4; i++)
            {
                var image = _loader.GetRandomCard();
                NewBoard.BoardArray[0][i].ReleaseCard(new Card.Card(image.Key, i, 0, image.Value.Key, image.Value.Value));
                results.Add(new KeyValuePair<FrameworkElement, int>(image.Key, i));
            }

            return results;
        }
    }
}
