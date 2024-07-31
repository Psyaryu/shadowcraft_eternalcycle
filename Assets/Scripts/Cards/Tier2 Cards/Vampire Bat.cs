using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class VampireBat : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 2, 0, 3, 2 };
    int health = 3;
    int attack = 2;
    string description = "Creature. Heals for damage done in dark ";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "shadow", true);
    string cardName = "VampireBat";
    List<string> Tags = new List<string> {"Vampire"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
       
    }
    public void EffectBattle()
    {
        List<BoardSlot> effectedSlots = BattleManager.shared.effectedSlots;
        if (effectedSlots[0].cycleType == CycleType.Shadow)
        {
            effectedSlots[0].card.card.health += Math.Clamp(effectedSlots[0].card.card.attack, 1, 3);
        }

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
