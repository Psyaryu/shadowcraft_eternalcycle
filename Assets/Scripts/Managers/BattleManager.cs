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

        [SerializeField]
        CanvasGroup hudCanvasGroup;

        [SerializeField]
        GameObject RewardsGameObject = null;

        [SerializeField]
        CardWidget CardReward1 = null;

        [SerializeField]
        CardWidget CardReward2 = null;

        [SerializeField]
        CardWidget CardReward3 = null;

        //Index Identifiers for ManaTypes
        int lightMana = 0;
        int fire = 1;
        int water = 2;
        int nature = 3;
        int shadow = 4;
        int death = 5;


        public static BattleManager shared = null;

        public List<CardWidget> effectedCards = new List<CardWidget>();
        public List<BoardSlot> effectedSlots = new List<BoardSlot>();

        [SerializeField]
        public CardWidget cardPrefab = null;

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
        public GameBoardWidget gameBoardWidget = null;

        bool battleRunning = true;
        bool isStandByPhase = false;

        public Player player = null;
        public AIPlayer opponent = null;
        public CharacterAsset opponentCharacter = null;

        public Player currentPlayer = null;
        public Player otherCharacter = null;

        public int turnNumber = 0;

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
            RewardsGameObject.gameObject.SetActive(false);
            hudCanvasGroup.alpha = 1f;

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
            turnNumber = 0;
            player.ShuffleDeck();
            RewardsGameObject.gameObject.SetActive(false);
            AudioManager.Instance.PlayBattleMusic();
            yield return null;
        }

        IEnumerator StartOfRound()
        {
            turnNumber++;
            foreach(var slot in gameBoardWidget.CardSlots)
            {
                slot.UpdateEnchant();
            }
            cycleIndexStart = ((cycleIndexStart - 1) + lightDarkCycle.Count) % lightDarkCycle.Count;

            var cycleCounts = new Dictionary<BoardSlot.CycleType, int>();
            cycleCounts[BoardSlot.CycleType.Light] = 0;
            cycleCounts[BoardSlot.CycleType.Shadow] = 0;

            for (int i = 0; i < gameBoardWidget.numberOfCardSlots * 2; i++)
            {
                var cycleIndex = (cycleIndexStart + i % gameBoardWidget.numberOfCardSlots) % lightDarkCycle.Count;
                gameBoardWidget.SetCycle(lightDarkCycle[cycleIndex], i);
                cycleCounts[lightDarkCycle[cycleIndex]] = cycleCounts[lightDarkCycle[cycleIndex]] + 1;
            }

            AudioManager.Instance.AdjustBattleMusic(cycleCounts[BoardSlot.CycleType.Light], cycleCounts[BoardSlot.CycleType.Shadow]);

            yield return null;
        }

        IEnumerator StartOfTurn(Player player)
        {
            Debug.Log($"{player.character.Name} Start of Turn");
            player.currentMana = ProduceMana(player.manaProductionRate, player);
            if(player != opponent)
            SetManaTextValues(player.currentMana);

           var slots = gameBoardWidget.GetSlotsofTag("ShadowAssasin");
            foreach(var slot in slots)
            {
                Type type = Type.GetType(slot.card.card.cardName);

                GameObject newObject = new GameObject("ScriptInstanceObject");
                MonoBehaviour scriptInstance = newObject.AddComponent(type) as MonoBehaviour;

                MethodInfo effectMethod = type.GetMethod("Effect");
                if (effectMethod != null)
                    effectMethod.Invoke(scriptInstance, null);
            }

            player.finishedStandBy = false;

            yield return null;
        }

        public IEnumerator DrawPhase(Player player)
        {
            var cardWidget = player.Draw();

            if (cardWidget == null)
                yield break;

            cardWidget.ResetCard();

            var card = cardWidget.card;

            if (card == null)
                yield break;

            Debug.Log($"{player.character.Name} Draw");
            if (player != opponent)
            {
                var cardWidget2 = Instantiate(cardPrefab, handParent);
                cardWidget2.card = card;
                PositionHandCards();
            }
            else
            {
                cardWidget.transform.parent = handParentEnemy;
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

            otherCharacter = GetOtherCharacter(player);

            var cardsToRemove = new List<CardWidget>();

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
                    if(oppositeCard.card.Tags.Contains("Nightmare"))
                    {
                        if(gameBoardWidget.CardSlots[oppositeCard.card.boardSlot].GetCycleType() == BoardSlot.CycleType.Light)
                        {
                            oppositeCard.card.health -= card.card.attack;
                        }
                    }
                    else if (card.card.Tags.Contains("Spirit"))
                    {
                        int randomNumber = UnityEngine.Random.Range(0, 5);
                        int randomNumber2 = UnityEngine.Random.Range(0, 5);
                        if(currentPlayer == player)
                        {
                                randomNumber = (randomNumber + 5) % 10;
                            randomNumber2 = (randomNumber2 + 5) % 10;
                        }
                        if(randomNumber == 5)
                        {
                            gameBoardWidget.CardSlots[randomNumber2].card.card.health -= card.card.attack;
                        }
                        else if(randomNumber2 == 5)
                        {
                            gameBoardWidget.CardSlots[randomNumber].card.card.health -= card.card.attack;
                        }
                        else
                        {
                            gameBoardWidget.CardSlots[randomNumber2].card.card.health -= card.card.attack;
                            gameBoardWidget.CardSlots[randomNumber].card.card.health -= card.card.attack;
                        }
                    }
                    else
                    oppositeCard.card.health -= card.card.attack;

                    if (card.card.Tags.Contains("Splash"))
                    {
                        effectedCards.Clear();
                        effectedCards.Add(card);

                        effectedSlots.Clear();
                        effectedSlots.Add(gameBoardWidget.CardSlots[card.card.boardSlot]);
                        if((Oppositeslot != 0 )&&(Oppositeslot != 4 )&&(Oppositeslot != 5)&&(Oppositeslot != 9))
                        {
                            int left = Oppositeslot - 1;
                            int right = Oppositeslot + 1;

                            if (gameBoardWidget.CardSlots[left].card != null)
                            {
                                effectedCards.Add(gameBoardWidget.CardSlots[left].card);
                            }

                            if (gameBoardWidget.CardSlots[right].card != null)
                            {
                                effectedCards.Add(gameBoardWidget.CardSlots[right].card);
                            }

                        }
                        else if(Oppositeslot == 0)
                        {
                            int right = Oppositeslot + 1;

                            if(gameBoardWidget.CardSlots[right].card != null)
                            {
                                effectedCards.Add(gameBoardWidget.CardSlots[right].card);
                            }
                        }
                        else if (Oppositeslot == 4)
                        {
                            int left = Oppositeslot - 1;

                            if (gameBoardWidget.CardSlots[left].card != null)
                            {
                                effectedCards.Add(gameBoardWidget.CardSlots[left].card);
                            }
                        }
                        else if (Oppositeslot == 5)
                        {
                            int right = Oppositeslot + 1;

                            if (gameBoardWidget.CardSlots[right].card != null)
                            {
                                effectedCards.Add(gameBoardWidget.CardSlots[right].card);
                            }
                        }
                        else if (Oppositeslot == 9)
                        {
                            int left = Oppositeslot - 1;

                            if (gameBoardWidget.CardSlots[left] != null)
                            {
                                effectedCards.Add(gameBoardWidget.CardSlots[left].card);
                            }
                        }
                    }
                    if(card.card.Tags.Contains("Breath"))
                    {
                        otherCharacter.TakeDamage(card.card.attack);
                    }
                    if(card.card.Tags.Contains("Vampire"))
                    {
                        effectedSlots.Clear();
                        effectedSlots.Add(gameBoardWidget.CardSlots[card.card.boardSlot]);
                    }

                    Type type = Type.GetType(card.card.cardName);

                    GameObject newObject = new GameObject("ScriptInstanceObject");
                    MonoBehaviour scriptInstance = newObject.AddComponent(type) as MonoBehaviour;

                    MethodInfo effectMethod = type.GetMethod("EffectAttack");
                    if(effectMethod != null)
                    effectMethod.Invoke(scriptInstance, null);

                    CheckDeath(oppositeCard);
                  
                }
                else
                {
                    otherCharacter.TakeDamage(card.card.attack);
                }
            }

            cardsToRemove.ForEach(Card => {
                otherCharacter.SendToGraveYard(Card);
                RemoveCardFromBoardSlot(Card);
            });

            yield return null;
        }
        public IEnumerator BoardSelectSlot()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Perform raycasting
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        BoardSlot temp = hit.collider.GetComponent<BoardSlot>();


                        if (temp != null)
                        {
                            // Invoke the callback with the hit object
                            effectedSlots.Add(temp);
                            yield break; // Stop the coroutine once the object is found
                        }
                    }
                }
                yield return null;
            }
        }

        public IEnumerator CardSelectFieldCor()
        {
            yield return null;
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Perform raycasting
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        CardWidget temp = hit.collider.GetComponent<CardWidget>();

                        if (temp == null)
                            temp = hit.collider.GetComponentInParent<CardWidget>();

                        if (temp == null)
                        {

                            yield break;
                        }
                        Card temp1 = temp.card;

                        if (temp != null)
                        {
                            // Invoke the callback with the hit object
                            effectedCards.Add(temp);
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

            var startTime = Time.time;
            var endTime = startTime + 0.25f;

            while (Time.time < endTime)
            {
                var t = (Time.time - startTime) / endTime;
                var alpha = Mathf.Lerp(0f, 1f, t);

                hudCanvasGroup.alpha = 1f - alpha;
                yield return null;
            }

            hudCanvasGroup.alpha = 0f;

            if (!player.IsDead()) {
                var allCards = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToList();

                var cardRewards = new List<CardWidget> { CardReward1, CardReward2, CardReward3 };

                foreach (var cardReward in cardRewards)
                {
                    var randomIndex = UnityEngine.Random.Range(0, allCards.Count);
                    var randomCard = allCards[randomIndex];
                    Card.AttachCardToCardWidget(cardReward, randomCard);
                    allCards.RemoveAt(randomIndex);
                }

                RewardsGameObject.gameObject.SetActive(true);
                handParent.gameObject.SetActive(false);
                gameBoardWidget.gameObject.SetActive(false);

                yield return CardSelectFieldCor();

                var selectedCard = effectedCards.Last();
                effectedCards.Clear();

                var reward = cardRewards.First(Reward => Reward.card == selectedCard);

                player.AddToDeck(reward);
            }

            yield return TransitionToMainMenu();
        }

        IEnumerator TransitionToMainMenu()
        {
            yield return AudioManager.Instance.FadeOutBattle();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }


        IEnumerator Whirlpool(CardWidget cardWidget)
        {
            yield return CardSelectFieldCor();
            yield return CardSelectFieldCor();

            Type type = Type.GetType(cardWidget.card.cardName);

            GameObject newObject = new GameObject("ScriptInstanceObject");
            MonoBehaviour scriptInstance = newObject.AddComponent(type) as MonoBehaviour;

            MethodInfo effectMethod = type.GetMethod("Effect");
            if (effectMethod != null)
                effectMethod.Invoke(scriptInstance, null);

            CheckDeath(cardWidget);
        }
        #endregion

        #region Battle Utilities
        public void CheckDeath(CardWidget card)
        {
            if (card.card.health <= 0)
            {
                otherCharacter.SendToGraveYard(card);
                effectedSlots.Clear();
                effectedSlots.Add(gameBoardWidget.CardSlots[card.card.boardSlot]);
                RemoveCardFromBoardSlot(card);
                Type type1 = Type.GetType(card.card.cardName);

                GameObject newObject1 = new GameObject("ScriptInstanceObject");
                MonoBehaviour scriptInstance1 = newObject1.AddComponent(type1) as MonoBehaviour;

                MethodInfo effectMethod1 = type1.GetMethod("EffectDeath");
                if(effectMethod1 != null)
                effectMethod1.Invoke(scriptInstance1, null);

                int extraDamage = math.abs(card.card.health);
                if (!card.card.Tags.Contains("Breath"))
                    otherCharacter.TakeDamage(extraDamage);
            }
        }

        public void PositionHandCards()
        {
            var maxWidth = 11f; // board is 15, so 10 + card buffer
            var sectionWidth = 2.5f; // card size
            var totalCards = Mathf.Floor(maxWidth / sectionWidth) + 1;

            //var yPosition = handParent.position.y;

            var shiftAmount = Mathf.Min((handParent.childCount - 1) / 2f * sectionWidth, maxWidth / 2f);

            var cardPercent = (handParent.childCount - totalCards) / handParent.childCount;

            for (int i = 0; i < handParent.childCount; i++)
            {
                var flexShift = handParent.childCount > totalCards ? cardPercent * sectionWidth * i : 0;
                var xPosition = i * sectionWidth - shiftAmount - flexShift;

                var cardTransform = handParent.GetChild(i);
                cardTransform.localPosition = new Vector3(xPosition, 0, -(i / 100f));
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

            effectedSlots.Clear();
            effectedSlots.Add(boardSlot);

            if (cardWidget.card.Tags.Contains("Whirlpool"))
            {
                effectedCards.Clear();
                StartCoroutine(Whirlpool(cardWidget));
                return;
            }
            Type type = Type.GetType(cardWidget.card.cardName);

            GameObject newObject = new GameObject("ScriptInstanceObject");
            MonoBehaviour scriptInstance = newObject.AddComponent(type) as MonoBehaviour;

            MethodInfo effectMethod = type.GetMethod("Effect");
            if(effectMethod != null)
            effectMethod.Invoke(scriptInstance, null);

            CheckDeath(cardWidget);

        }
        public void RemoveCardFromBoardSlot(CardWidget cardWidget)
        {
            var graveyard = currentPlayer == player ? OpponentGraveyard : PlayerGraveyard;
            gameBoardWidget.RemoveCard(cardWidget.card.boardSlot, currentPlayer, graveyard);
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
                    mastery[lightMana]++;
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

        public void FreeBoardSlot(int boardSlot, Player player)
        {
            var graveyard = player == opponent ? OpponentGraveyard : PlayerGraveyard;
            gameBoardWidget.RemoveCard(boardSlot, player, graveyard);
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
            StartCoroutine(TransitionToMainMenu());
        }

        public void OnEndTurn()
        {
            currentPlayer.finishedStandBy = true;
        }

        public void SetManaTextValues(int[] values)
        {
            lightText.text = "light: " + values[lightMana].ToString();
            fireText.text = "fire: " + values[fire].ToString();
            waterText.text = "water: " + values[water].ToString();
            natureText.text = "nature: " + values[nature].ToString();
            shadowText.text = "shadow: " + values[shadow].ToString();
            deathText.text = "death: " + values[death].ToString();
        }

        #endregion
    }
}
