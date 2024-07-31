using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;

public class CardSkellaton : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 0;
    int attack = 0;
    string description = "";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "ManaType", true);
    string cardName = "";

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        List<CardWidget> effectedCards = new List<CardWidget>();
        StartCoroutine(BattleManager.shared.CardSelectFieldCor());

        effectedCards = BattleManager.shared.effectedCards;

        for (int i = 0; i < effectedCards.Count; i++)
        {
            effectedCards[i].card.health++; 
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

        return newCard;
    }
    #endregion
}
