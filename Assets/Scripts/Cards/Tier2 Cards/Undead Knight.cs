using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class UndeadKnight : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 5 };
    int health = 5;
    int attack = 3;
    string description = "-1 health if placed in light";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "death", true);
    string cardName = "UndeadKnight";
    List<string> Tags = new List<string> {"ShadowAssasin"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        List<BoardSlot> effectedSlots = BattleManager.shared.effectedSlots;
        if (effectedSlots[0].cycleType == CycleType.Light)
        {
            effectedSlots[0].card.card.health -= 1;
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
