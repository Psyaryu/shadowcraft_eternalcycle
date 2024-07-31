using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class Whirlpool : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 0;
    int attack = 0;
    string description = "Spell. Switch position of two cards on your side";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "water", true);
    string cardName = "Whirlpool";
    List<string> Tags = new List<string> { "Whirlpool" };

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        var effectedCards = BattleManager.shared.effectedCards;

        if (effectedCards.Count < 2)
            return;

        var effect1 = effectedCards[0];
        var effect2 = effectedCards[1];

        if (effect1 == null || effect2 == null)
            return;

        int card1Slot = effect1.card.boardSlot;
        int card2Slot = effect2.card.boardSlot;


        BattleManager.shared.gameBoardWidget.SwapCards(card1Slot, card2Slot);
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
