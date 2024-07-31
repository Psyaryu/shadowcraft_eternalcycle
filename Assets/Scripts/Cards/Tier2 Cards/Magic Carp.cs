using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using Unity.Mathematics;

public class MagicCarp : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 2, 2, 0, 0 };
    int health = 1;
    int attack = 0;
    string description = "Creature. Deals 50% attack to adjacent enemies";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "water", true);
    string cardName = "MagicCarp";
    List<string> Tags = new List<string> {"Creature","Splash"};


    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void EffectAttack()
    {
        List<CardWidget> effectedCards = new List<CardWidget>();

        effectedCards = BattleManager.shared.effectedCards;

        for (int i = 0; i < effectedCards.Count; i++)
        {
            if (i != 0)
            {
                effectedCards[i].card.health -= (effectedCards[0].card.attack) / 2;

                if (effectedCards[i].card.health <= 0)
                {
                    BattleManager.shared.otherCharacter.SendToGraveYard(effectedCards[i]);
                    int extraDamage = math.abs(effectedCards[i].card.health);
                    BattleManager.shared.otherCharacter.TakeDamage(extraDamage);
                }
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
