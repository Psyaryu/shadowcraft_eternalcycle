using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class YinYang : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 0;
    int attack = 0;
    string description = "Spell. 5 dmg to enemy if 50% or more of board dark, 5 healing to player if 50%+ light";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "water", true);
    string cardName = "YinYang";
    List<string> Tags = new List<string> {""};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        int light = 0;
        int dark = 0;
        List<BoardSlot> slots = new List<BoardSlot>();
        foreach (var slot in BattleManager.shared.gameBoardWidget.CardSlots)
        {
                if (slot.GetCycleType() == CycleType.Light)
                {
                    light++;
                }
                if(slot.GetCycleType() == CycleType.Shadow)
                {
                    dark++;
                }
        }

        if(light >= 5)
        {
            BattleManager.shared.currentPlayer.health += 5;
        }
        if(dark >= 5)
        {
            BattleManager.shared.otherCharacter.health -= 5;
        }
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
