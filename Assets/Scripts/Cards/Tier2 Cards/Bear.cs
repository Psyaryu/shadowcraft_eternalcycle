using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowCraft;
using static ShadowCraft.Card;
using System;
using Unity.Mathematics;

public class Bear : MonoBehaviour
{
    int[] ManaCost = { 0, 0, 0, 0, 0, 0 };
    int health = 3;
    int attack = 2;
    string description = "Creature. Deals 50% damage to adjecent cards";
    ManaTypes cardType = (ManaTypes)Enum.Parse(typeof(ManaTypes), "nature", true);
    string cardName = "Bear";
    List<string> Tags = new List<string> { "Splash", "Creature" };


    #region Effects

    //TODO: Add one attack to itself

    #endregion
    public void Effect()
    {
        
        
    }

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
