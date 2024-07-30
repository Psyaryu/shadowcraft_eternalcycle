using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;

public class SoldierofRain : MonoBehaviour
{
    int[] ManaCost = { 1, 0, 0, 1, 0, 0 };
    int health = 1;
    int attack = 1;
    string description = "Basic card for water";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "water", true);
    string cardName = "SoldierofRain";

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
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

        return newCard;
    }
    #endregion
}
