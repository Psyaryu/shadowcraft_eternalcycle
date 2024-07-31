using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager shared = null;

        [SerializeField]
        CharacterAsset playerCharacter = null;

        [SerializeField]
        CharacterAsset opponentCharacter = null;

        [SerializeField]
        public CardWidget cardPrefab = null;

        [SerializeField]
        public Transform cardParent = null;

        public Player player = null;
        public AIPlayer ai = null;

        private void Awake()
        {
            if (shared == null)
                shared = this;
            else
                Destroy(this);

            DontDestroyOnLoad(shared);

            player = new Player(playerCharacter);
            
            
        }
    }
}
