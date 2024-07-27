using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class Player
    {
        public CharacterAsset character = null;

        int health = -1;

        List<Card> deck = new List<Card>();
        List<Card> hand = new List<Card>();
        public List<Card> field = new List<Card>();
        List<Card> graveyard = new List<Card>();
        public int[] manaProductionRate = { 0, 0, 0, 0, 0, 0 };

        public Player(CharacterAsset characterAsset)
        {
            character = characterAsset;
            AddToDeck("TestCard");
            AddToDeck("TestCard");
            AddToDeck("TestCard");
            AddToDeck("TestCard");
            AddToDeck("TestCard");
            manaProductionRate = SetProductionRate();
            health = character.health;
        }

        public int[] SetProductionRate()
        {
            //TODO: Add statment to set production rate dependant on class?
            int[] rate = { 1, 1, 1, 1, 1, 1 };
            return rate;
        }

        virtual public void AddToDeck(string cardType)
        {
            Card newCard = Card.CreateCard(cardType);
            
            if(newCard != null)
            {
                deck.Add(newCard);
            }
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

        public void Attack(Card card)
        {
            // TODO: Determine if we need to attack the player directly or one of the cards on the field
            health -= card.attack;

            Debug.Log($"{character.Name} took {card.attack} damage");
        }

        public bool IsDead() => health <= 0;
    }

}
