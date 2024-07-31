using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class Torchbearer : MonoBehaviour
{
    int[] ManaCost = { 2, 3, 0, 0, 0, 0 };
    int health = 2;
    int attack = 1;
    string description = "Adds 1 torch to hand when played";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "fire", true);
    string cardName = "Torchbearer";
    List<string> Tags = new List<string> {""};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        BattleManager.shared.currentPlayer.AddToDeck("Torch");
        StartCoroutine(BattleManager.shared.DrawPhase(BattleManager.shared.currentPlayer));

       
        BattleManager.shared.PositionHandCards();
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
