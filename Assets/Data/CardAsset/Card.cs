using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ShadowCraft
{
    [CreateAssetMenu(fileName = "New Card", menuName = "ShadowCraft/New Card")]

    public class Card : ScriptableObject
    {
        public enum Cards
        {
            TestCard
        }
        public enum ManaTypes
        {
            light1,
            light2,
            neut1,
            neut2,
            dark1,
            dark2
        }

        [TextArea]
        public string cardName = "Default Card";

        public ManaTypes cardType;
        public int attack = 1;
        public int health = 1;

        public int[] manaCost = {0, 0, 0, 0, 0, 0};

        [TextArea]
        public string description = "This card does nothing!";

        public static Card CreateCard(string cardName)
        {
            if (Enum.TryParse(cardName, out Cards cardType))
            {
                string className = cardType.ToString();
                Type type = Type.GetType(className);

                if (type != null)
                {
                    // Create an instance of the card class
                    MonoBehaviour cardInstance = new GameObject(className).AddComponent(type) as MonoBehaviour;

                    // Check if the instance has a 'ToCard' method
                    MethodInfo toCardMethod = type.GetMethod("ToCard");
                    if (toCardMethod != null)
                    {
                        // Invoke the ToCard method and return the Card object
                        return toCardMethod.Invoke(cardInstance, null) as Card;
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
    }
}
