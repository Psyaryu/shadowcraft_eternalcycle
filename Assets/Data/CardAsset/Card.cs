using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    [CreateAssetMenu(fileName = "New Card", menuName = "ShadowCraft/New Card")]
    public class Card: ScriptableObject
    {
        [Serializable]
        class ManaCost
        {
            public Mana.Type manaType = Mana.Type.Void;
            public int cost = 1;
        }

        int attack = 1;
        int health = 1;

        [TextArea]
        string description = "This card does nothing!";

        List<ManaCost> manaCosts = new List<ManaCost>();

        // TODO: Figure out the Board Passives
        // TODO: Figure out the dark/light augmentations
    }
}
