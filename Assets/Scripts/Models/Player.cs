using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class Player
    {
        public CharacterAsset character = null;

        int health = -1;

        protected List<CardWidget> deck = new List<CardWidget>();
        protected List<CardWidget> hand = new List<CardWidget>();
        public List<CardWidget> field = new List<CardWidget>();
        public List<CardWidget> graveyard = new List<CardWidget>();

        public int[] manaProductionRate = { 0, 0, 0, 0, 0, 0 };
        public int[] currentMana = { 0, 0, 0, 0, 0, 0 };
        public int[] elementalMastery = { 0, 0, 0, 0, 0, 0 };

        string playerClass = null;

        public bool finishedStandBy = false;
        public int identity = 1;

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
        }


        public int[] SetProductionRate()
        {

            switch (playerClass)
            {
                case "Fire":
                    int[] rate = { 1,1,1,1,1,1};
                    return rate;
                    

            }
            //TODO: Add statment to set production rate dependant on class?
       
            int[] defaultrate = { 0,0,0,0,0,0};
            return defaultrate;
        }

        virtual public void AddToDeck(string cardType)
        {
            CardWidget newCard = Card.CreateCard(cardType);
            
            if(newCard != null)
            {
                deck.Add(newCard);
            }
        }

        virtual public CardWidget Draw()
        {
            if (deck.Count == 0)
            {
                ShuffleGraveYard();
            }
                

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
        }

        public void RemovCard(CardWidget cardWidget)
        {
            field.Remove(cardWidget);
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
            deck.AddRange(graveyard);
            ShuffleDeck();

            foreach (var card in graveyard)
            {
                graveyard.Remove(card);
            }
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
    }

}
