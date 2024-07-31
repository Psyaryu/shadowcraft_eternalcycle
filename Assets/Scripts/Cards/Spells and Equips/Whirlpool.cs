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
     int card1Slot = effectedCards[0].card.boardSlot;
     int  card2Slot = effectedCards[1].card.boardSlot;

     BattleManager.shared.gameBoardWidget.MoveCard(card1Slot, card2Slot);
     BattleManager.shared.gameBoardWidget.MoveCard(card2Slot, card1Slot);
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
