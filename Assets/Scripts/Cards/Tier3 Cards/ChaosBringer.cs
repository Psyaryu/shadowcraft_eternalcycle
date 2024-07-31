using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class ChaosBringer : MonoBehaviour
{
    int[] ManaCost = { 3, 5, 0, 0, 0, 3 };
    int health = 4;
    int attack = 4;
    string description = "Swaps light and dark tiles for 1 turn, and attacks adjacet enemys for 50% damage";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "fire", true);
    string cardName = "ChaosBringer";
    List<string> Tags = new List<string> {"Splash"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        var effectedSlots = BattleManager.shared.effectedSlots;
        effectedSlots[0].chaosBringerTurn = BattleManager.shared.turnNumber + 1;
        List<BoardSlot> slots = new List<BoardSlot>();
        foreach (var slot in BattleManager.shared.gameBoardWidget.CardSlots)
        {
            if (slot.GetCycleType() == CycleType.Light)
            {
                slot.OnDark();
            }
            else if (slot.GetCycleType() == CycleType.Shadow)
            {
                slot.OnLight();
            }
        }

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
