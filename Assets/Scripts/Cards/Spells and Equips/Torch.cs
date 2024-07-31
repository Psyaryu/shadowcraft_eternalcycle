using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;

public class Torch : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 0;
    int attack = 0;
    string description = "Light up a space for 3 turns";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "Fire", true);
    string cardName = "Torch";

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        List<BoardSlot> effectedSlots = new List<BoardSlot>();
        StartCoroutine(BattleManager.shared.BoardSelectSlot());

        effectedSlots = BattleManager.shared.effectedSlots;

        for (int i = 0; i < effectedSlots.Count; i++)
        {
            effectedSlots[i].OnLight();
            effectedSlots[i].torchTurn = BattleManager.shared.turnNumber + 3;
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
