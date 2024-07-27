using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowCraft
{
    public class BattleManager : MonoBehaviour
    {
        #region Properties

        public static BattleManager shared = null;

        [SerializeField]
        CardWidget cardPrefab = null;

        [SerializeField]
        Transform handParent = null;

        [SerializeField]
        int startingHand = 5;

        [SerializeField]
        GameBoardWidget gameBoardWidget = null;

        bool battleRunning = true;
        bool endOfTurn = false;
        bool isStandByPhase = false;

        Player player = null;
        Player opponent = null;
        Player currentPlayer = null;

        List<CardWidget> hand = new List<CardWidget>();
        List<CardWidget> field = new List<CardWidget>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            player = GameManager.shared.player;
            opponent = GameManager.shared.ai;
            shared = this;
        }

        private void Start()
        {
            StartCoroutine(MainBattleSequence());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            shared = null;
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
                    currentPlayer = character;
                    yield return StartOfTurn(character);
                    yield return DrawPhase(character);
                    yield return StandbyPhase(character);
                    yield return BattlePhase(character);

                    if (GetOtherCharacter(character).IsDead())
                        break;

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
            var cardWidget = Instantiate(cardPrefab, handParent);
            cardWidget.card = card;

            PositionHandCards();
        }

        IEnumerator StandbyPhase(Player player)
        {
            isStandByPhase = true;
            Debug.Log($"{player.character.Name} Stand By");
            while (!endOfTurn)
            {
                yield return null;
            }
            isStandByPhase = false;
        }

        IEnumerator BattlePhase(Player player)
        {
            Debug.Log($"{player.character.Name} Battle");
            var otherCharacter = GetOtherCharacter(player);

            player.field.ForEach(Card => otherCharacter.Attack(Card));

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

        #region Battle Utilities

        public void PositionHandCards()
        {
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

        public void AddCardToBoardSlot(CardWidget cardWidget, BoardSlot boardSlot)
        {
            var canPlaceCard = currentPlayer == player ? boardSlot.SlotNumber < 5 : boardSlot.SlotNumber > 4;
            if (!gameBoardWidget.GetIsSlotEmpty(boardSlot) || !canPlaceCard)
            {
                PositionHandCards();
                cardWidget.isPlaced = false;
                return;
            }

            gameBoardWidget.AddCard(cardWidget, boardSlot);
            currentPlayer.PlayCard(cardWidget.card);
        }

        public bool CanPlaceCardInSlot(BoardSlot boardSlot)
        {
            var canPlaceCard = currentPlayer == player ? boardSlot.SlotNumber < 5 : boardSlot.SlotNumber > 4;
            return canPlaceCard;
        }

        #endregion

        #region Getters

        private bool GetBattleIsRunning()
        {
            return !player.IsDead() && !opponent.IsDead() && battleRunning;
        }

        private Player GetOtherCharacter(Player battlePlayer) => battlePlayer == player ? opponent : player;

        public bool GetIsStandByPhase() => isStandByPhase;

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
