using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ShadowCraft
{
    public class Card : ScriptableObject
    {
        public enum Cards
        {
            TestCard,
            SoldierofDark,
            SoldierofLight,
            SoldierofShadows,
            SoldierofRain,
            SoldierofNature,
            SoldierofFlames,
        }
        public enum ManaTypes
        {
            light,
            fire,
            water,
            nature,
            shadow,
            death
        }

        [TextArea]
        public string cardName = "Default Card";

        public ManaTypes cardType;
        public int attack = 1;
        public int health = 1;
        public int boardSlot = -1;

        public int[] manaCost = {0, 0, 0, 0, 0, 0};

        [TextArea]
        public string description = "This card does nothing!";

        private int startingAtk = -1;
        private int startingHealth = -1;

        public static CardWidget CreateCard(string cardName)
        {
            if (Enum.TryParse(cardName, out Cards cardType))
            {
                string className = cardType.ToString();
                Type type = Type.GetType(className);

                if (type != null)
                {

                    var cardWidget = Instantiate(GameManager.shared.cardPrefab, GameManager.shared.cardParent);

                    // Create an instance of the card class
                    MonoBehaviour cardInstance = cardWidget.gameObject.AddComponent(type) as MonoBehaviour;

                    // Check if the instance has a 'ToCard' method
                    MethodInfo toCardMethod = type.GetMethod("ToCard");
                    if (toCardMethod != null)
                    {
                        Card cardObj = toCardMethod.Invoke(cardInstance, null) as Card;
                        cardObj.startingAtk = cardObj.attack;
                        cardObj.startingHealth = cardObj.health;

                        cardWidget.card = cardObj;
                        // Invoke the ToCard method and return the Card object
                        return cardWidget;
                    }   
                    else
                    {
                        Debug.LogError($"{className} does not have a ToCard method.");
                    }
                }
                else
                {
                    Debug.LogError($"Class for {className} not found.");
                }
            }
            else
            {
                Debug.LogError($"{cardName} is not a valid card type.");
            }

            return null;
        }

        public void ResetCard()
        {
            attack = startingAtk;
            health = startingHealth;
        }
    }
}
