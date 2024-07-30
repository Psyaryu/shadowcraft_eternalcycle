using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;

public class SoldierofFlames : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 1, 1 };
    int health = 1;
    int attack = 1;
    string description = "Basic card for fire";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "fire", true);
    string cardName = "SoldierofFlames";

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
