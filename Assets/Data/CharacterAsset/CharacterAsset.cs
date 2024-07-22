using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    [CreateAssetMenu(fileName = "New Character Asset", menuName = "ShadowCraft/New Character Asset")]
    public class CharacterAsset : ScriptableObject
    {
        public int health = 30;
        public Deck deck = null;
    }
}
