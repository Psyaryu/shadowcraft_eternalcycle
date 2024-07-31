using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using static ShadowCraft.BoardSlot;

public class Revalation : MonoBehaviour
{
    int[] ManaCost = { 0, 4, 0, 0, 0, 2 };
    int health = 0;
    int attack = 0;
    string description = "Draw a card for each light unit on a light tile";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "light", true);
    string cardName = "Revalation";
    List<string> Tags = new List<string> {""};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        int count = 0;
        foreach (var slot in BattleManager.shared.gameBoardWidget.CardSlots)
        {
            if (slot.card != null)
            {
                if (slot.card.card.cardType == ManaTypes.light && slot.GetCycleType() == CycleType.Light)
                {
                    count++;
                }
            }
        }

        for(int i = 0; i < count; i++)
        {
          BattleManager.shared.currentPlayer.Draw();
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
