using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class Player
    {
        public CharacterAsset character = null;

        public int health = -1;

        protected List<CardWidget> deck = new List<CardWidget>();
        protected List<CardWidget> hand = new List<CardWidget>();
        public List<CardWidget> field = new List<CardWidget>();
        public List<CardWidget> graveyard = new List<CardWidget>();

        public int[] manaProductionRate = { 0, 0, 0, 0, 0, 0 };
        public int[] currentMana = { 0, 0, 0, 0, 0, 0 };
        public int[] elementalMastery = { 0, 0, 0, 0, 0, 0 };

        public bool finishedStandBy = false;
        public int identity = 1;

        List<string> AddedCards = new List<string>();

        public Player(CharacterAsset characterAsset)
        {
            character = characterAsset;
            manaProductionRate = SetProductionRate();
            if (this.identity == 1)
            SetDeck();
            health = character.health;
        }
        
        
        virtual public void SetDeck()
        {
            StartingDecksManager.shared.SetBasePlayerDeck(this);
            ShuffleDeck();
        }


        public int[] SetProductionRate()
        {

            switch ("Fire")
            {
                case "Fire":
                    int[] rate = { 1,1,1,1,1,1};
                    return rate;
                    

            }
            //TODO: Add statment to set production rate dependant on class?
       
            //int[] defaultrate = { 0,0,0,0,0,0};
            //return defaultrate;
        }

        virtual public void AddToDeck(string cardType)
        {
            CardWidget newCard = Card.CreateCard(cardType);
            
            if(newCard != null)
            {
                deck.Add(newCard);
            }
        }

        public void AddToDeck(CardWidget newCard)
        {
            deck.Add(newCard);
        }

        virtual public CardWidget Draw()
        {
            if (deck.Count == 0)
            {
                ShuffleGraveYard();
            }

            if (deck.Count == 0)
                return null;

            var card = deck[0];

            hand.Add(card);
            deck.RemoveAt(0);

            return card;
        }

        virtual public IEnumerator StandByPhase()
        {
            while (!finishedStandBy)
            {
                yield return null;
            }
        }

        public void PlayCard(CardWidget cardWidget)
        {
            field.Add(cardWidget);
            hand.Remove(cardWidget);
            cardWidget.isPlaced = true;
        }

        public void RemovCard(CardWidget cardWidget)
        {
            field.Remove(cardWidget);
            cardWidget.isPlaced = false;
        }

        public void SendToGraveYard(CardWidget cardWidget)
        {
            graveyard.Add(cardWidget);
            field.Remove(cardWidget);
        }

        public void ShuffleDeck()
        {
            var shuffledDeck = new List<CardWidget>();

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
            var graveyardCards = graveyard.FindAll(Card => Card != null);
            graveyardCards.ForEach(Card => Card.ResetCard());
            deck.AddRange(graveyardCards);

            ShuffleDeck();

            graveyard.Clear();
        }

        public void ShuffleHand()
        {
            graveyard.AddRange(hand);
            ShuffleDeck();
        }

        public void TakeDamage(int damage)
        {
            // TODO: Determine if we need to attack the player directly or one of the cards on the field
            health -= damage;

            Debug.Log($"{character.Name} took {damage} damage");
        }

        public bool IsDead() => health <= 0;

        public void Reset()
        {
            health = character.health;
            currentMana = new int[] { 0, 0, 0, 0, 0, 0 };
            elementalMastery = new int[] { 0, 0, 0, 0, 0, 0 };
            manaProductionRate = SetProductionRate();

            deck.Clear();
            hand.Clear();
            graveyard.Clear();
            field.Clear();

            SetDeck();
            LoadRewardCards();
        }

        public void AddReward(CardWidget cardWidget) => AddedCards.Add(cardWidget.card.cardName);

        private void LoadRewardCards() => AddedCards.ForEach(Reward => AddToDeck(Reward));
    }
}
