using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowCraft
{
    public class BattleManager : MonoBehaviour
    {
        // TODO: Replace all instances of CharacterAsset with the new player script in clickup task
        // https://app.clickup.com/t/86b1drvcb

        #region Properties

        bool battleRunning = true;
        bool endOfTurn = false;

        CharacterAsset player = null;
        CharacterAsset opponent = null;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            player = GameManager.shared.player;
            opponent = GameManager.shared.opponent;
        }

        private void Start()
        {
            StartCoroutine(MainBattleSequence());
        }

        #endregion

        #region Battle Sequence

        IEnumerator MainBattleSequence()
        {
            yield return StartOfBattle();

            var characters = new List<CharacterAsset> { player, opponent };

            while (GetBattleIsRunning())
            {
                foreach (var character in characters)
                {
                    yield return StartOfTurn(character);
                    yield return DrawPhase(character);
                    yield return StandbyPhase(character);
                    yield return BattlePhase(character);
                    yield return EndOfTurn(character);
                }
            }

            yield return EndOfBattle();
        }

        IEnumerator StartOfBattle()
        {
            Debug.Log("Start of Battle");
            yield return null;
        }

        IEnumerator StartOfTurn(CharacterAsset character)
        {
            Debug.Log($"{character.Name} Start of Turn");
            endOfTurn = false;
            yield return null;
        }

        IEnumerator DrawPhase(CharacterAsset character)
        {
            Debug.Log($"{character.Name} Draw");
            yield return null;
        }

        IEnumerator StandbyPhase(CharacterAsset character)
        {
            Debug.Log($"{character.Name} Stand By");
            while (!endOfTurn)
            {
                yield return null;
            }
        }

        IEnumerator BattlePhase(CharacterAsset character)
        {
            Debug.Log($"{character.Name} Battle");
            var otherCharacter = GetOtherCharacter(character);
            yield return null;
        }

        IEnumerator EndOfTurn(CharacterAsset character)
        {
            Debug.Log($"{character.Name} End of Turn");
            yield return null;
        }

        IEnumerator EndOfBattle()
        {
            Debug.Log("End of Battle");
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return null;
        }

        #endregion

        #region Getters

        private bool GetBattleIsRunning() => battleRunning;

        private CharacterAsset GetOtherCharacter(CharacterAsset character) => character == player ? opponent : player;

        #endregion

        #region UI Actions

        public void OnMainMenu()
        {
            battleRunning = false;
        }

        public void OnEndTurn()
        {
            endOfTurn = true;
        }

        #endregion
    }
}
