using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    [CreateAssetMenu(fileName = "New Card", menuName = "ShadowCraft/New Card")]
    public class Card: ScriptableObject
    {
        [Serializable]
        public class ManaCost
        {
            public Mana.Type manaType = Mana.Type.Void;
            public int cost = 1;
        }

        public int attack = 1;
        public int health = 1;

        [TextArea]
        public string description = "This card does nothing!";

        [SerializeField]
        public List<ManaCost> manaCosts = new List<ManaCost>();

        // TODO: Figure out the Board Passives
        // TODO: Figure out the dark/light augmentations
    }
}
