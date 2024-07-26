using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class Player
    {
        public CharacterAsset character = null;

        List<Card> deck = new List<Card>();
        List<Card> hand = new List<Card>();
        List<Card> field = new List<Card>();
        List<Card> graveyard = new List<Card>();

        public Player(CharacterAsset characterAsset)
        {
            character = characterAsset;
            deck = Object.Instantiate(character.deck).cards;
        }

        virtual public Card Draw()
        {
            if (deck.Count == 0)
                return null; // TODO: Reshuffle the deck?

            var card = deck[0];

            hand.Add(card);
            deck.RemoveAt(0);

            return card;
        }

        public void PlayCard(Card card)
        {
            field.Add(card);
            hand.Remove(card);
        }

        public void SendToGraveYard(Card card)
        {
            graveyard.Add(card);
            field.Remove(card);
        }

        public void ShuffleDeck()
        {
            var shuffledDeck = new List<Card>();

            for (int i = 0; i < deck.Count; i++)
            {
                var index = Random.Range(0, deck.Count);
                shuffledDeck.Add(deck[index]);
                deck.RemoveAt(index);
            }

            deck = shuffledDeck;
        }

        public void ShuffleGraveYard()
        {
            deck.AddRange(graveyard);
            ShuffleDeck();
        }

        public void ShuffleHand()
        {
            deck.AddRange(hand);
            ShuffleDeck();
        }
    }

}
