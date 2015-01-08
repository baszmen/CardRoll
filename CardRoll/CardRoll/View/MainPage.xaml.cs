using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CardRoll.Control;
using CardRoll.Exceptions;
using Microsoft.Phone.Controls;

namespace CardRoll.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Consts

        private const int CardSize = 90;
        private const int _margin = 5;

        #endregion

        #region Fields

        private CardMoveControl _cardMoveControl;
        private PresentationControl _control;
        public bool IsNewGame = false;

        #endregion

        public MainPage()
        {
            InitializeComponent();

            Loaded += (o, s) =>
                {
                    _control = new PresentationControl(
                        (int)AddsStackPanel.ActualHeight, (int)JokerBoardStackPanel.ActualHeight,
                        (int)GameBoardStackPanel.ActualHeight, (int)NewBoardStackPanel.ActualHeight,
                        40, 480, 100, canvas);
                    SetUpFourNewCard(_control.FourCardRoll());
                };
        }

        #region SetUp Images

        /// <summary>
        /// Set up new four card, which rolls
        /// </summary>
        /// <param name="images"></param>
        private void SetUpFourNewCard(List<KeyValuePair<FrameworkElement, int>> images)
        {
            images.ForEach(pair =>
                {
                    pair.Key.Width = CardSize;
                    pair.Key.Height = CardSize;
                    canvas.Children.Add(pair.Key);
                    SetUpCanvasPosition(pair);
                    SetManipulation(pair.Key);
                });
        }

        /// <summary>
        /// Set position on a Canvas.
        /// </summary>
        private void SetUpCanvasPosition(KeyValuePair<FrameworkElement, int> conf)
        {
            var coords = _control.GetPositionCoords(new KeyValuePair<BoardType, KeyValuePair<int, int>>(BoardType.NewBoard,
                                                                                           new KeyValuePair<int, int>(
                                                                                               conf.Value, 0)));
            Canvas.SetLeft(conf.Key, coords.Key + _margin);
            Canvas.SetTop(conf.Key, coords.Value + _margin);
        }

        #endregion

        #region Manage Manipulations

        private void SetManipulation(UIElement element)
        {
            element.ManipulationStarted += CardRoll_ManipulationStarted;
            element.ManipulationDelta += CardRoll_ManipulationDelta;
            element.ManipulationCompleted += CardRoll_ManipulationCompleted;
        }

        #endregion

        #region Manipulations

        private void CardRoll_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (!(sender is FrameworkElement))
                return;

            Canvas.SetZIndex((sender as UIElement), Canvas.GetZIndex((sender as UIElement)) + 1000);

            _cardMoveControl = new CardMoveControl
                {
                    Actual = new Point(Canvas.GetLeft((sender as FrameworkElement)),
                                       Canvas.GetTop((sender as FrameworkElement))),
                    Element = (sender as FrameworkElement),
                    Start = new Point(Canvas.GetLeft((sender as FrameworkElement)),
                                      Canvas.GetTop((sender as FrameworkElement)))
                };
            (sender as FrameworkElement).Height = CardSize * 1.15;
            (sender as FrameworkElement).Width = CardSize * 1.15;
            Canvas.SetLeft((sender as FrameworkElement), Canvas.GetLeft((sender as FrameworkElement)) - (0.15 * CardSize) / 2);
            Canvas.SetTop((sender as FrameworkElement), Canvas.GetTop((sender as FrameworkElement)) - (0.15 * CardSize) / 2);
        }

        private void CardRoll_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (!(sender is FrameworkElement))
                return;

            Canvas.SetLeft((sender as FrameworkElement),
                           Canvas.GetLeft((sender as FrameworkElement)) + e.DeltaManipulation.Translation.X);
            Canvas.SetTop((sender as FrameworkElement),
                          Canvas.GetTop((sender as FrameworkElement)) + e.DeltaManipulation.Translation.Y);
            _cardMoveControl.Actual = new Point(Canvas.GetLeft((sender as FrameworkElement)),
                                                Canvas.GetTop((sender as FrameworkElement)));
        }

        private void CardRoll_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (!(sender is FrameworkElement))
                return;

            Canvas.SetZIndex((sender as UIElement), Canvas.GetZIndex((sender as UIElement)) - 1000);

            (sender as FrameworkElement).Height = CardSize;
            (sender as FrameworkElement).Width = CardSize;

            try
            {
                KeyValuePair<int, int> key = _control.RecognizeBoard(sender)
                                                     .CanReleaseCard()
                                                     .ReleaseCard(_cardMoveControl.Start.X, _cardMoveControl.Start.Y)
                                                     .GetPositionCoords();

                Canvas.SetLeft((sender as FrameworkElement), key.Key + _margin);
                Canvas.SetTop((sender as FrameworkElement), key.Value + _margin);
            }
            catch (OutOfBoardException)
            {
                Canvas.SetLeft((sender as FrameworkElement), _cardMoveControl.Start.X);
                Canvas.SetTop((sender as FrameworkElement), _cardMoveControl.Start.Y);
                return;
            }
            catch (DestinationLockedException)
            {
                Canvas.SetLeft((sender as FrameworkElement), _cardMoveControl.Start.X);
                Canvas.SetTop((sender as FrameworkElement), _cardMoveControl.Start.Y);
                return;
            }

            _cardMoveControl = null;
        }

        #endregion
    }
}