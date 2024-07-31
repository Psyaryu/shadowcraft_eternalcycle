using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class ShadowAssasin : MonoBehaviour
{
    int[] ManaCost = { 0, 2, 0, 0, 4, 2 };
    int health = 2;
    int attack = 0;
    string description = "+4/0 while on dark tile";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "death", true);
    string cardName = "ShadowAssasin";
    List<string> Tags = new List<string> {"ShadowAssasin"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        List<BoardSlot> effectedSlots = BattleManager.shared.effectedSlots;
        if (effectedSlots[0].cycleType == CycleType.Shadow)
        {
            effectedSlots[0].card.card.attack += 4;
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
