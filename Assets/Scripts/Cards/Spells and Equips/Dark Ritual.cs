using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using System.Linq;

public class DarkRitual : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 0;
    int attack = 0;
    string description = "Spell. Sacrafice a minion on a dark space and deal 2 damage to 3 random enemys";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "death", true);
    string cardName = "DarkRitual";
    List<string> Tags = new List<string> {"Spell"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        List<CardWidget> effectedCards = new List<CardWidget>();
        StartCoroutine(BattleManager.shared.CardSelectFieldCor());

        BattleManager.shared.currentPlayer.SendToGraveYard(effectedCards[0]);

        List<BoardSlot> slots = new List<BoardSlot>();
        List<CardWidget> cardWidgets = new List<CardWidget>();
        slots = BattleManager.shared.gameBoardWidget.CardSlots;
        foreach (var slot in slots)
        {

            if (slot.card != null)
            {
                cardWidgets.Add(slot.card);
            }
        }

        if(cardWidgets.Count < 4)
        {
            foreach (var card in cardWidgets)
            {
                card.card.health -= 2;

                BattleManager.shared.CheckDeath(card);
            }
        }
        else
        {
            System.Random random = new System.Random();
            var listtemp = cardWidgets.OrderBy(x => random.Next()).Take(3).ToList();

            foreach (var card in listtemp)
            {
                card.card.health -= 2;

                BattleManager.shared.CheckDeath(card);
            }
        }
    }
    public void EffectDeath()
    {
       

    }


    #region Conversion
    public Card ToCard()
    {
        Card newCard = ScriptableObject.CreateInstance<Card>();
        newCard.attack = attack;
        newCard.health = health;
        newCard.description = description;
        newCard.manaCost = ManaCost;
        newCard.cardType = cardType;
        newCard.cardName = cardName;
        newCard.Tags = Tags;

        return newCard;
    }
    #endregion
}
