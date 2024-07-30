using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ShadowCraft.Card;

namespace ShadowCraft
{
    public class BattleManager : MonoBehaviour
    {
        #region Properties
        //Index Identifiers for ManaTypes
        int light = 0;
        int fire = 1;
        int water = 2;
        int nature = 3;
        int shadow = 4;
        int death = 5;


        public static BattleManager shared = null;

        public List<Card> effectedCards = new List<Card>();

        [SerializeField]
        CardWidget cardPrefab = null;

        [SerializeField]
        Transform handParent = null;

        [SerializeField]
        public Transform handParentEnemy = null;

        [SerializeField]
        public Transform PlayerGraveyard = null;

        [SerializeField]
        public Transform OpponentGraveyard = null;

        [SerializeField]
        int startingHand = 5;

        [SerializeField]
        GameBoardWidget gameBoardWidget = null;

        bool battleRunning = true;
        bool isStandByPhase = false;
        bool playerTurn = true;

        public Player player = null;
        public AIPlayer opponent = null;
        public CharacterAsset opponentCharacter = null;
        Player currentPlayer = null;

        List<CardWidget> hand = new List<CardWidget>();
        List<CardWidget> field = new List<CardWidget>();

        public GameObject manaTextParent = null;
        private TMP_Text lightText = null;
        private TMP_Text fireText = null;
        private TMP_Text waterText = null;
        private TMP_Text natureText = null;
        private TMP_Text shadowText = null;
        private TMP_Text deathText = null;

        private List<BoardSlot.CycleType> lightDarkCycle = new List<BoardSlot.CycleType>();

        private int cycleIndexStart = 1;

        #endregion

        #region Unity Methods

        private void Awake()
        {

            player = GameManager.shared.player;
            opponent = new AIPlayer(opponentCharacter);
            shared = this;
        }

        private void Start()
        {
            lightDarkCycle.AddRange(Enumerable.Repeat(BoardSlot.CycleType.Light, gameBoardWidget.numberOfCardSlots));
            lightDarkCycle.AddRange(Enumerable.Repeat(BoardSlot.CycleType.Shadow, gameBoardWidget.numberOfCardSlots));

            lightText = manaTextParent.transform.Find("light").GetComponent<TMP_Text>();
            fireText = manaTextParent.transform.Find("fire").GetComponent<TMP_Text>();
            waterText = manaTextParent.transform.Find("water").GetComponent<TMP_Text>();
            natureText = manaTextParent.transform.Find("nature").GetComponent<TMP_Text>();
            shadowText = manaTextParent.transform.Find("shadow").GetComponent<TMP_Text>();
            deathText = manaTextParent.transform.Find("death").GetComponent<TMP_Text>();

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
           // opponent.SetDeck();
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
            player.currentMana = ProduceMana(player.manaProductionRate, player);
            if(player != opponent)
            SetManaTextValues(player.currentMana);

            player.finishedStandBy = false;

            yield return null;
        }

        IEnumerator DrawPhase(Player player)
        {
            var card = player.Draw().card;

            if (card == null)
                yield break;

            Debug.Log($"{player.character.Name} Draw");
            if (player != opponent)
            {
                var cardWidget = Instantiate(cardPrefab, handParent);
                cardWidget.card = card;
                PositionHandCards();
            }
            else
            {
                var cardWidget = Instantiate(cardPrefab, handParentEnemy);
                cardWidget.card = card;
              
            }
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

            foreach (var card in player.field)
            {
                int slot = card.card.boardSlot;
                int Oppositeslot = (slot + 5) % 10;
                CardWidget oppositeCard;

                if (gameBoardWidget.cards[Oppositeslot] != null)
                {
                    oppositeCard = gameBoardWidget.cards[Oppositeslot];
                }
                else
                {
                    oppositeCard = null;
                }
                

                if(oppositeCard != null)
                {
                    oppositeCard.card.health -= card.card.attack;

                    if (oppositeCard.card.health <= 0)
                    {
                        //TODO: remove card from widget
                        RemoveCardFromBoardSlot(gameBoardWidget.cards[Oppositeslot]);

                        otherCharacter.SendToGraveYard(oppositeCard);
                        int extraDamage = math.abs(oppositeCard.card.health);
                        otherCharacter.TakeDamage(extraDamage);

                    }
                }
                else
                {
                    otherCharacter.TakeDamage(card.card.attack);
                }
            }

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

        public void AddCardToBoardSlot(Card card, BoardSlot boardSlot, Player player)
        {
            CardWidget cardWidget = null;
            if (currentPlayer != opponent)
            {
                cardWidget = Instantiate(cardPrefab, handParent);
            }
            else
            {
                cardWidget = Instantiate(cardPrefab, handParentEnemy);
            }
            cardWidget.card = card;

            AddCardToBoardSlot(cardWidget, boardSlot, player);
        }

        public void AddCardToBoardSlot(CardWidget cardWidget, BoardSlot boardSlot, Player currentplayer)
        {
            var canPlaceCard = currentPlayer == player ? boardSlot.SlotNumber < 5 : boardSlot.SlotNumber > 4;
            int[] mastery = CanAffordMana(player, cardWidget.card.manaCost);

            if (!gameBoardWidget.GetIsSlotEmpty(boardSlot) || !canPlaceCard || mastery == null)
            {
                if(currentplayer != opponent)
                PositionHandCards();
                cardWidget.isPlaced = false;
                return;
            }
            currentplayer.currentMana = SubtractMana(mastery, currentplayer.currentMana);
            if(currentPlayer != opponent)
            SetManaTextValues(player.currentMana);
            currentplayer.elementalMastery = UpdateElementalMastery(currentplayer, cardWidget.card.cardType);
            gameBoardWidget.AddCard(cardWidget, boardSlot);
            currentPlayer.PlayCard(cardWidget);

            Type type = Type.GetType(cardWidget.card.cardName);

            GameObject newObject = new GameObject("ScriptInstanceObject");
            MonoBehaviour scriptInstance = newObject.AddComponent(type) as MonoBehaviour;

            MethodInfo effectMethod = type.GetMethod("Effect");

            effectMethod.Invoke(scriptInstance, null);

        }
        public void RemoveCardFromBoardSlot(CardWidget cardWidget)
        {
            if(currentPlayer != opponent)
            gameBoardWidget.RemoveCard(cardWidget.card.boardSlot, currentPlayer, PlayerGraveyard);
            else
            gameBoardWidget.RemoveCard(cardWidget.card.boardSlot, currentPlayer, OpponentGraveyard);
            currentPlayer.RemovCard(cardWidget);
            /*
            Type type = Type.GetType(card.card.cardName);

            GameObject newObject = new GameObject("ScriptInstanceObject");
            MonoBehaviour scriptInstance = newObject.AddComponent(type) as MonoBehaviour;

            MethodInfo effectMethod = type.GetMethod("EffectDead");

            effectMethod.Invoke(scriptInstance, null);
            */

        }
        public int[] CanAffordMana(Player player, int[] manaCost)
        {
            int[] masteryCost;

            masteryCost = SubtractMana(player.elementalMastery, manaCost);

            for(int i = 0; i < player.currentMana.Length; i++)
            {
                if (player.currentMana[i] < manaCost[i])
                {
                    return null;
                }
            }

            return masteryCost;
        }

        public int[] SubtractMana(int[] elementalMastery, int[] manaCost)
        {
            int[] remainingCost = manaCost.Zip(elementalMastery, (a, b) => a - b).ToArray();

            for (int i = 0; i < manaCost.Length; i++)
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
            canPlaceCard = canPlaceCard && !boardSlot.GetIsFilled();
            return canPlaceCard;
        }

        public int[] ProduceMana(int[] increaseAmounts, Player player)
        {
            int[] newMana = increaseAmounts.Zip(player.currentMana, (a, b) => a + b).ToArray();
            return newMana;
        }

        public int[] UpdateElementalMastery(Player player, ManaTypes cardType)
        {
            int[] mastery = player.elementalMastery;

            switch(cardType.ToString())
            {
                case "light":
                    mastery[light]++;
                    break;
                case "fire":
                    mastery[fire]++;
                    break;
                case "water":
                    mastery[water]++;
                    break;
                case "nature":
                    mastery[nature]++;
                    break;
                case "shadow":
                    mastery[shadow]++;
                    break;
                case "death":
                    mastery[death]++;
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

        public GameBoardWidget GetGameBoard() => gameBoardWidget;

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
            lightText.text = "light: " + values[light].ToString();
            fireText.text = "fire: " + values[fire].ToString();
            waterText.text = "water: " + values[water].ToString();
            natureText.text = "nature: " + values[nature].ToString();
            shadowText.text = "shadow: " + values[shadow].ToString();
            deathText.text = "death: " + values[death].ToString();
        }

        #endregion
    }
}
