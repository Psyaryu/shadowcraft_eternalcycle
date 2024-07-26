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

        public Player player = null;
        public Player ai = null;

        private void Awake()
        {
            if (shared == null)
                shared = this;
            else
                Destroy(this);

            DontDestroyOnLoad(shared);

            player = new Player(playerCharacter);
            ai = new AIPlayer(opponentCharacter);
        }
    }
}
