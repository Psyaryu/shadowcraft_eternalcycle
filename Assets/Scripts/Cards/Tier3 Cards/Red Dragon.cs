using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class RedDragon : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 7;
    int attack = 0;
    string description = "Attack is the difference of light and dark spaces";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "fire", true);
    string cardName = "RedDragon";
    List<string> Tags = new List<string> {"RedDragon"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        var effectedSlots = BattleManager.shared.effectedSlots;
        int light = 0;
        int dark = 0;
        List<BoardSlot> slots = new List<BoardSlot>();
        foreach (var slot in BattleManager.shared.gameBoardWidget.CardSlots)
        {
            if (slot.card != null)
            {
                if (slot.GetCycleType() == CycleType.Light)
                {
                    light++;
                }
                if (slot.GetCycleType() == CycleType.Shadow)
                {
                    dark++;
                }
            }
        }

        int attack = Math.Abs(light - dark);
        effectedSlots[0].card.card.attack = attack;

    }
    public void EffectBattle()
    {

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
