using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class FlameEater : MonoBehaviour
{
    int[] ManaCost = { 0, 2, 0, 0, 2, 3 };
    int health = 3;
    int attack = 1;
    string description = "Attacks both opposing card and player";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "fire", true);
    string cardName = "FlameEater";
    List<string> Tags = new List<string> {"Breath"};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        var slot = BattleManager.shared.effectedSlots[0];

        int numSlot = slot.SlotNumber;

        if (numSlot != 0 || numSlot != 4 || numSlot != 5 || numSlot != 9)
        {
            int left = numSlot - 1;
            int right = numSlot + 1;

            BattleManager.shared.gameBoardWidget.CardSlots[left].OnDark();
            BattleManager.shared.gameBoardWidget.CardSlots[right].OnDark();

        }
        else if (numSlot == 0)
        {
            int right = numSlot + 1;

            BattleManager.shared.gameBoardWidget.CardSlots[right].OnDark();

        }
        else if (numSlot == 4)
        {
            int left = numSlot - 1;
            BattleManager.shared.gameBoardWidget.CardSlots[left].OnDark();
        }
        else if (numSlot == 5)
        {
            int right = numSlot + 1;
            BattleManager.shared.gameBoardWidget.CardSlots[right].OnDark();
        }
        else if (numSlot == 9)
        {
            int left = numSlot - 1;
            BattleManager.shared.gameBoardWidget.CardSlots[left].OnDark();

        }

        int Oppositeslot = (slot.SlotNumber + 5) % 10;
        BattleManager.shared.gameBoardWidget.CardSlots[Oppositeslot].OnLight();



    }
    public void EffectBattle()
    {

    }
    public void EffectDeath()
    {
        var slot = BattleManager.shared.effectedSlots[0];
        

        int numSlot = slot.SlotNumber;

        if (numSlot != 0 || numSlot != 4 || numSlot != 5 || numSlot != 9)
        {
            int left = numSlot - 1;
            int right = numSlot + 1;
            int OppositeslotLeft = (left + 5) % 10;
            int OppositeslotRight = (right + 5) % 10;

            if (BattleManager.shared.gameBoardWidget.CardSlots[OppositeslotLeft].cycleType == CycleType.Light)
            {
                BattleManager.shared.gameBoardWidget.CardSlots[left].OnLight();
            }
            else
            {
                BattleManager.shared.gameBoardWidget.CardSlots[left].OnDark();
            }
            if (BattleManager.shared.gameBoardWidget.CardSlots[OppositeslotRight].cycleType == CycleType.Light)
            {
                BattleManager.shared.gameBoardWidget.CardSlots[right].OnLight();
            }
            else
            {
                BattleManager.shared.gameBoardWidget.CardSlots[right].OnDark();
            }

        }
        else if (numSlot == 0)
        {
         
            int right = numSlot + 1;
            int OppositeslotRight = (right + 5) % 10;

          
            if (BattleManager.shared.gameBoardWidget.CardSlots[OppositeslotRight].cycleType == CycleType.Light)
            {
                BattleManager.shared.gameBoardWidget.CardSlots[right].OnLight();
            }
            else
            {
                BattleManager.shared.gameBoardWidget.CardSlots[right].OnDark();
            }
        }
        else if (numSlot == 4)
        {
            int left = numSlot - 1;
           
            int OppositeslotLeft = (left + 5) % 10;
           

            if (BattleManager.shared.gameBoardWidget.CardSlots[OppositeslotLeft].cycleType == CycleType.Light)
            {
                BattleManager.shared.gameBoardWidget.CardSlots[left].OnLight();
            }
            else
            {
                BattleManager.shared.gameBoardWidget.CardSlots[left].OnDark();
            }
           
        }
        else if (numSlot == 5)
        {
           
            int right = numSlot + 1;
           
            int OppositeslotRight = (right + 5) % 10;

            if (BattleManager.shared.gameBoardWidget.CardSlots[OppositeslotRight].cycleType == CycleType.Light)
            {
                BattleManager.shared.gameBoardWidget.CardSlots[right].OnLight();
            }
            else
            {
                BattleManager.shared.gameBoardWidget.CardSlots[right].OnDark();
            }
        }
        else if (numSlot == 9)
        {
            int left = numSlot - 1;
            
            int OppositeslotLeft = (left + 5) % 10;
           

            if (BattleManager.shared.gameBoardWidget.CardSlots[OppositeslotLeft].cycleType == CycleType.Light)
            {
                BattleManager.shared.gameBoardWidget.CardSlots[left].OnLight();
            }
            else
            {
                BattleManager.shared.gameBoardWidget.CardSlots[left].OnDark();
            }
          
        }
        int oppositeSlot = (slot.SlotNumber + 5) % 10;
        if (BattleManager.shared.gameBoardWidget.CardSlots[slot.SlotNumber].cycleType == CycleType.Light)
        {
            BattleManager.shared.gameBoardWidget.CardSlots[oppositeSlot].OnLight();
        }
        else
        {
            BattleManager.shared.gameBoardWidget.CardSlots[oppositeSlot].OnDark();
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
        newCard.Tags = Tags;

        return newCard;
    }
    #endregion
}
