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
            Bear,
            Druid,
            FlameEater,
            Torch,
            DarkRitual,
            YinYang,
            ShadowAssasin,
            DarkPact,
            Revalation,
            Wolf,
            Treant,
            ChaosBringer,
            RedDragon,
            Torchbearer,
            UndeadKnight,
            VampireBat,
            Paladin,
            Whirlpool,
            Nightmare,
            MagicCarp,
            FlameSpirit


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
        public List<string> Tags = new List<string>();

        [TextArea]
        public string description = "This card does nothing!";

        private int startingAtk = -1;
        private int startingHealth = -1;

        public bool DruidActive = false;

        public static CardWidget CreateCard(string cardName)
        {
            if (Enum.TryParse(cardName, out Cards cardType))
            {
                var cardWidget = Instantiate(GameManager.shared.cardPrefab, GameManager.shared.cardParent);

                var result = AttachCardToCardWidget(cardWidget, cardType);

                if (!result)
                {
                    Debug.LogError($"{cardName} is not a valid card type.");
                    return null;
                }

                return cardWidget;
            }
            else
            {
                Debug.LogError($"{cardName} is not a valid card type.");
            }

            return null;
        }

        public static bool AttachCardToCardWidget(CardWidget cardWidget, Cards cards)
        {
            string className = cards.ToString();
            Type type = Type.GetType(className);

            if (type != null)
            {

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

                    return true;
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

            return false;
        }
    

        public void ResetCard()
        {
            attack = startingAtk;
            health = startingHealth;
        }

        public bool IsSpell() => attack == 0 && health == 0;
    }
}
