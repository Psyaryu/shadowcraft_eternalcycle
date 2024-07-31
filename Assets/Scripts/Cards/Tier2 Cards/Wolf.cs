using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using Unity.Mathematics;

public class Wolf : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 2;
    int attack = 1;
    string description = "Creature. +1/+0 for each creature in play";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "nature", true);
    string cardName = "Wolf";
    List<string> Tags = new List<string> {  "Creature" };


    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
       var effectedSlots = BattleManager.shared.effectedSlots;
       var slots = BattleManager.shared.gameBoardWidget.GetSlotsofTag("Creature");
        
        foreach (var slot in slots)
        {
            effectedSlots[0].card.card.attack += 1;
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
