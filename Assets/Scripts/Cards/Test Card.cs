using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;

public class TestCard : MonoBehaviour
{
    int[] ManaCost = { 2, 2, 2, 2, 2, 2 };
    int health = 2;
    int attack = 2;
    string description = "This is a test card with 2/2 stats";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "shadow", true);
    string cardName = "TestCard";

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        List<Card> effectedCards = new List<Card>();
        StartCoroutine(BattleManager.shared.CardSelectFieldCor());

        effectedCards = BattleManager.shared.effectedCards;

        for (int i = 0; i < effectedCards.Count; i++)
        {
            effectedCards[i].health++; 
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
