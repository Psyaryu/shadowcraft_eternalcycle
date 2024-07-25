using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager shared = null;

        public CharacterAsset player = null;
        public CharacterAsset opponent = null;

        private void Awake()
        {
            if (shared == null)
                shared = this;
            else
                Destroy(this);

            DontDestroyOnLoad(shared);
        }
    }
}
