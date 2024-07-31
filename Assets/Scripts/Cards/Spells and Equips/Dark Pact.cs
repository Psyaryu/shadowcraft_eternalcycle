using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class DarkPact : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 0;
    int attack = 0;
    string description = "Draw 2 cards and take 3 damage";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "shadow", true);
    string cardName = "DarkPact";
    List<string> Tags = new List<string> {""};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
   
        StartCoroutine(BattleManager.shared.DrawPhase(BattleManager.shared.currentPlayer));
        StartCoroutine(BattleManager.shared.DrawPhase(BattleManager.shared.currentPlayer));

        BattleManager.shared.currentPlayer.health -= 3;
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
