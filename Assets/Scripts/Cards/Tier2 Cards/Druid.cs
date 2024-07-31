using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;

public class Druid : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 5, 0, 0 };
    int health = 4;
    int attack = 0;
    string description = "Raises all creture's attack by 2";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "nature", true);
    string cardName = "Druid";
    List<string> Tags = new List<string> {""};

    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        BattleManager.shared.gameBoardWidget.DruidActive = true;
        BattleManager.shared.gameBoardWidget.UpdateEffects();
    }
    public void EffectDeath()
    {
        BattleManager.shared.gameBoardWidget.DruidActive = false;
        foreach (var slot in BattleManager.shared.gameBoardWidget.CardSlots)
        {

            if (slot.card.card.Tags.Contains("Creature"))
            {
                slot.card.card.attack--;
                slot.card.card.DruidActive = false;
            }
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
