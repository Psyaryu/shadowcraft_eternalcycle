using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class Nightmare : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 2, 1, 3, 0 };
    int health = 4;
    int attack = 2;
    string description = "Can only be damaged while on a light tile ";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "shadow", true);
    string cardName = "Nightmare";
    List<string> Tags = new List<string> {"Nightmare"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
       
    }
    public void EffectAttacked()
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
