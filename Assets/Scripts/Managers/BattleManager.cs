using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowCraft
{
    public class BattleManager : MonoBehaviour
    {
        #region Properties

        [SerializeField]
        CardWidget cardPrefab = null;

        [SerializeField]
        Transform handParent = null;

        [SerializeField]
        int startingHand = 5;

        bool battleRunning = true;
        bool endOfTurn = false;

        Player player = null;
        Player opponent = null;

        List<CardWidget> hand = new List<CardWidget>();
        List<CardWidget> field = new List<CardWidget>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            player = GameManager.shared.player;
            opponent = GameManager.shared.ai;
        }

        private void Start()
        {
            StartCoroutine(MainBattleSequence());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        #endregion

        #region Battle Sequence

        IEnumerator MainBattleSequence()
        {
            yield return StartOfBattle();

            var characters = new List<Player> { player, opponent };

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

        IEnumerator StartOfTurn(Player player)
        {
            Debug.Log($"{player.character.Name} Start of Turn");
            endOfTurn = false;
            yield return null;
        }

        IEnumerator DrawPhase(Player player)
        {
            var card = player.Draw();

            if (card == null)
                yield break;

            Debug.Log($"{player.character.Name} Draw");
            Instantiate(cardPrefab, handParent);

            var maxWidth = 11f; // board is 15, so 10 + card buffer
            var sectionWidth = 2.5f; // card size
            var totalCards = Mathf.Floor(maxWidth / sectionWidth) + 1;

            var yPosition = handParent.position.y;

            var shiftAmount = Mathf.Min((handParent.childCount - 1) / 2f * sectionWidth, maxWidth / 2f);

            var cardPercent = (handParent.childCount - totalCards) / handParent.childCount;

            for (int i = 0; i < handParent.childCount; i++)
            {
                var flexShift = handParent.childCount > totalCards ? cardPercent * sectionWidth * i : 0;
                var xPosition = i * sectionWidth - shiftAmount - flexShift;

                var cardTransform = handParent.GetChild(i);
                cardTransform.position = new Vector3(xPosition, yPosition, -(i / 100f));
            }
        }

        IEnumerator StandbyPhase(Player player)
        {
            Debug.Log($"{player.character.Name} Stand By");
            while (!endOfTurn)
            {
                yield return null;
            }
        }

        IEnumerator BattlePhase(Player player)
        {
            Debug.Log($"{player.character.Name} Battle");
            var otherCharacter = GetOtherCharacter(player);
            yield return null;
        }

        IEnumerator EndOfTurn(Player player)
        {
            Debug.Log($"{player.character.Name} End of Turn");
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

        private Player GetOtherCharacter(Player battlePlayer) => battlePlayer == player ? opponent : player;

        #endregion

        #region UI Actions

        public void OnMainMenu()
        {
            battleRunning = false;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void OnEndTurn()
        {
            endOfTurn = true;
        }

        #endregion
    }
}
