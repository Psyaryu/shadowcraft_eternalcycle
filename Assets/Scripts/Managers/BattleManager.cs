using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ShadowCraft.Card;

namespace ShadowCraft
{
    public class BattleManager : MonoBehaviour
    {
        #region Properties
        //Index Identifiers for ManaTypes
        int light1 = 0;
        int light2 = 1;
        int neut1 = 2;
        int neut2 = 3;
        int dark1 = 4;
        int dark2 = 5;

        int[] elementalMastery = { 0, 0, 0, 0, 0, 0 };

        int[] playerMana = { 0, 0, 0, 0, 0, 0 };

        public static BattleManager shared = null;

        public List<Card> effectedCards = new List<Card>();

        [SerializeField]
        CardWidget cardPrefab = null;

        [SerializeField]
        Transform handParent = null;

        [SerializeField]
        int startingHand = 5;

        [SerializeField]
        GameBoardWidget gameBoardWidget = null;

        bool battleRunning = true;
        bool isStandByPhase = false;
        bool playerTurn = true;

        Player player = null;
        Player opponent = null;
        Player currentPlayer = null;

        List<CardWidget> hand = new List<CardWidget>();
        List<CardWidget> field = new List<CardWidget>();

        public GameObject manaTextParent = null;
        private TMP_Text light1Text = null;
        private TMP_Text light2Text = null;
        private TMP_Text neut1Text = null;
        private TMP_Text neut2Text = null;
        private TMP_Text dark1Text = null;
        private TMP_Text dark2Text = null;

        private List<BoardSlot.CycleType> lightDarkCycle = new List<BoardSlot.CycleType>();

        private int cycleIndexStart = 1;

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
            lightDarkCycle.AddRange(Enumerable.Repeat(BoardSlot.CycleType.Light, gameBoardWidget.numberOfCardSlots));
            lightDarkCycle.AddRange(Enumerable.Repeat(BoardSlot.CycleType.Shadow, gameBoardWidget.numberOfCardSlots));

            light1Text = manaTextParent.transform.Find("Light1").GetComponent<TMP_Text>();
            light2Text = manaTextParent.transform.Find("Light2").GetComponent<TMP_Text>();
            neut1Text = manaTextParent.transform.Find("Neut1").GetComponent<TMP_Text>();
            neut2Text = manaTextParent.transform.Find("Neut2").GetComponent<TMP_Text>();
            dark1Text = manaTextParent.transform.Find("Dark1").GetComponent<TMP_Text>();
            dark2Text = manaTextParent.transform.Find("Dark2").GetComponent<TMP_Text>();

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
                yield return StartOfRound();

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

        IEnumerator StartOfRound()
        {
            cycleIndexStart = ((cycleIndexStart - 1) + lightDarkCycle.Count) % lightDarkCycle.Count;

            for (int i = 0; i < gameBoardWidget.numberOfCardSlots * 2; i++)
            {
                var cycleIndex = (cycleIndexStart + i % gameBoardWidget.numberOfCardSlots) % lightDarkCycle.Count;
                gameBoardWidget.SetCycle(lightDarkCycle[cycleIndex], i);
            }

            yield return null;
        }

        IEnumerator StartOfTurn(Player player)
        {
            Debug.Log($"{player.character.Name} Start of Turn");
            playerMana = ProduceMana(playerMana, player.manaProductionRate);
            SetManaTextValues(playerMana);

            player.finishedStandBy = false;

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

            yield return player.StandByPhase();

            isStandByPhase = false;
        }

        IEnumerator BattlePhase(Player player)
        {
            Debug.Log($"{player.character.Name} Battle");
            var otherCharacter = GetOtherCharacter(player);

            player.field.ForEach(Card => otherCharacter.Attack(Card));

            yield return null;
        }

        public IEnumerator CardSelectFieldCor()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Perform raycasting
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        CardWidget temp = hit.collider.GetComponent<CardWidget>();
                        Card temp1 = temp.card;

                        if (temp != null)
                        {
                            // Invoke the callback with the hit object
                            effectedCards.Add(temp1);
                            yield break; // Stop the coroutine once the object is found
                        }
                    }
                }
                yield return null;
            }   
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
            int[] mastery = CanAffordMana(playerMana, cardWidget.card.manaCost);

            if (!gameBoardWidget.GetIsSlotEmpty(boardSlot) || !canPlaceCard || mastery == null)
            {
                PositionHandCards();
                cardWidget.isPlaced = false;
                return;
            }
            playerMana = SubtractMana(mastery, playerMana);
            SetManaTextValues(playerMana);
            elementalMastery = UpdateElementalMastery(elementalMastery, cardWidget.card.cardType);
            gameBoardWidget.AddCard(cardWidget, boardSlot);
            currentPlayer.PlayCard(cardWidget.card);

            Type type = Type.GetType(cardWidget.card.cardName);

            GameObject newObject = new GameObject("ScriptInstanceObject");
            MonoBehaviour scriptInstance = newObject.AddComponent(type) as MonoBehaviour;

            MethodInfo effectMethod = type.GetMethod("Effect");

            effectMethod.Invoke(scriptInstance, null);

        }
        public int[] CanAffordMana(int[] playerMana, int[] manaCost)
        {
            int[] masteryCost;

            masteryCost = SubtractMana(elementalMastery, manaCost);

            for(int i = 0; i < playerMana.Length; i++)
            {
                if (playerMana[i] < manaCost[i])
                {
                    return null;
                }
            }

            return masteryCost;
        }

        public int[] SubtractMana(int[] elementalMastery, int[] manaCost)
        {
            int[] remainingCost = manaCost.Zip(elementalMastery, (a, b) => a - b).ToArray();

            for (int i = 0; i < playerMana.Length; i++)
            {
                if (remainingCost[i] < 0)
                {
                    remainingCost[1] = 0; ;
                }
            }

            return remainingCost;
        }

        public bool CanPlaceCardInSlot(BoardSlot boardSlot)
        {
            var canPlaceCard = currentPlayer == player ? boardSlot.SlotNumber < 5 : boardSlot.SlotNumber > 4;
            return canPlaceCard;
        }

        public int[] ProduceMana(int[] increaseAmounts, int[] playerMana)
        {
            int[] newMana = increaseAmounts.Zip(playerMana, (a, b) => a + b).ToArray();
            return newMana;
        }

        public int[] UpdateElementalMastery(int[] mastery, ManaTypes cardType)
        {

            switch(cardType.ToString())
            {
                case "light1":
                    mastery[light1]++;
                    break;
                case "light2":
                    mastery[light2]++;
                    break;
                case "neut1":
                    mastery[neut1]++;
                    break;
                case "neut2":
                    mastery[neut2]++;
                    break;
                case "dark1":
                    mastery[dark1]++;
                    break;
                case "dark2":
                    mastery[dark2]++;
                    break;
            }

            return mastery;
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
            currentPlayer.finishedStandBy = true;
        }

        public void SetManaTextValues(int[] values)
        {
            light1Text.text = "Light1: " + values[light1].ToString();
            light2Text.text = "Light2: " + values[light2].ToString();
            neut1Text.text = "neut1: " + values[neut1].ToString();
            neut2Text.text = "neut2: " + values[neut2].ToString();
            dark1Text.text = "dark1: " + values[dark1].ToString();
            dark2Text.text = "dark2: " + values[dark2].ToString();
        }

        #endregion
    }
}
