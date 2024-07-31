using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using Unity.Mathematics;

public class Treant : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 2;
    int attack = 2;
    string description = "Creature. +0/+3 when played on a light tile";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "nature", true);
    string cardName = "Treant";
    List<string> Tags = new List<string> {"Creature"};


    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
       var effectedSlots = BattleManager.shared.effectedSlots;
       
        if (effectedSlots[0].GetCycleType() == BoardSlot.CycleType.Light)
        {
            effectedSlots[0].card.card.health += 3;
        }
    }

    public void EffectAttack()
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
