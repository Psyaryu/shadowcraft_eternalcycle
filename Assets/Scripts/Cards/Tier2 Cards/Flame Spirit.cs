using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class FlameSpirit : MonoBehaviour
{
    int[] ManaCost = { 3, 1, 0, 2, 0, 0 };
    int health = 2;
    int attack = 2;
    string description = "Attacks two random opponents each turn";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "fire", true);
    string cardName = "FlameSpirit";
    List<string> Tags = new List<string> {"Spirit"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
       

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
