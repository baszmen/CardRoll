using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CardRoll.Control;

namespace CardRoll.Helpers
{
    public class    CardLoader
    {
        private readonly BitmapImage[][] _imageSources;

        public CardLoader()
        {
            _imageSources = new BitmapImage[4][];
            for (var i = 0; i < 4; i++)
            {
                _imageSources[i] = new BitmapImage[4];
            }

            _imageSources[0][0] = new BitmapImage(new Uri("/Content/Cards/clubs/blue.png", UriKind.Relative));
            _imageSources[1][0] = new BitmapImage(new Uri("/Content/Cards/clubs/green.png", UriKind.Relative));
            _imageSources[2][0] = new BitmapImage(new Uri("/Content/Cards/clubs/red.png", UriKind.Relative));
            _imageSources[3][0] = new BitmapImage(new Uri("/Content/Cards/clubs/yellow.png", UriKind.Relative));
            _imageSources[0][1] = new BitmapImage(new Uri("/Content/Cards/diamond/blue.png", UriKind.Relative));
            _imageSources[1][1] = new BitmapImage(new Uri("/Content/Cards/diamond/green.png", UriKind.Relative));
            _imageSources[2][1] = new BitmapImage(new Uri("/Content/Cards/diamond/red.png", UriKind.Relative));
            _imageSources[3][1] = new BitmapImage(new Uri("/Content/Cards/diamond/yellow.png", UriKind.Relative));
            _imageSources[0][2] = new BitmapImage(new Uri("/Content/Cards/heart/blue.png", UriKind.Relative));
            _imageSources[1][2] = new BitmapImage(new Uri("/Content/Cards/heart/green.png", UriKind.Relative));
            _imageSources[2][2] = new BitmapImage(new Uri("/Content/Cards/heart/red.png", UriKind.Relative));
            _imageSources[3][2] = new BitmapImage(new Uri("/Content/Cards/heart/yellow.png", UriKind.Relative));
            _imageSources[0][3] = new BitmapImage(new Uri("/Content/Cards/spade/blue.png", UriKind.Relative));
            _imageSources[1][3] = new BitmapImage(new Uri("/Content/Cards/spade/green.png", UriKind.Relative));
            _imageSources[2][3] = new BitmapImage(new Uri("/Content/Cards/spade/red.png", UriKind.Relative));
            _imageSources[3][3] = new BitmapImage(new Uri("/Content/Cards/spade/yellow.png", UriKind.Relative));
        }

        /// <summary>
        /// Returns an random Image with information about color and type of it.
        /// </summary>
        /// <returns> Image object (key propeties of main KeyValuePair) 
        /// and CardColor and CardType in properties Key andValue of KeyValuePair object which is stored in Value property
        /// of Main KeyValuePair object</returns>
        public KeyValuePair<Image, KeyValuePair<CardColor, CardType>> GetRandomCard()
        {
            var random = new Random();
            var color = random.Next(4);
            var type = random.Next(4);

            return new KeyValuePair<Image, KeyValuePair<CardColor, CardType>>(new Image()
                {
                    Source = _imageSources[color][type]
                }, 
                new KeyValuePair<CardColor, CardType>((CardColor)color, (CardType)type));
        }
    }
}
