using CardRoll.Control.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRoll.Control.Board
{
    public class Field
    {
        private Stack<CardObject> _cards;
        /// <summary>
        /// Check if collection is empty
        /// </summary>
        public Boolean IsEmpty
        {
            get
            {
                return (_cards.Count == 0);
            }
        }

        public Boolean IsActive { get; set; }

        /// <summary>
        /// Get active card. Remember to user IsEmpty property before to check if
        /// there are any card in collectin
        /// </summary>
        public CardObject ActiveCard
        {
            get
            {
                return _cards.Last();
            }
        }

        /// <summary>
        /// Release card on Field
        /// </summary>
        /// <param name="card"></param>
        public void ReleaseCard(CardObject card)
        {
            _cards.Push(card);
        }

        /// <summary>
        /// Remove card from field
        /// </summary>
        public CardObject RemoveCard()
        {
            var card = _cards.Last();
            _cards.Pop();
            return card;
        }

        public Field()
        {
            _cards = new Stack<CardObject>();
            IsActive = true;
        }
    }
}
