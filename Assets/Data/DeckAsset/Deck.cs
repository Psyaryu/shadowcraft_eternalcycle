using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "ShadowCraft/New Deck")]
    public class Deck : ScriptableObject
    {
        public List<Card> cards = new List<Card>();
    }
}
