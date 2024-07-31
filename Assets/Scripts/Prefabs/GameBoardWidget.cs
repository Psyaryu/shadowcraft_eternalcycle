using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShadowCraft
{
    public class GameBoardWidget : MonoBehaviour
    {
        #region Properties

        [SerializeField]
        public int numberOfCardSlots = 5;

        [SerializeField]
        List<BoardSlot> CardSlots = new List<BoardSlot>();

        [SerializeField]
        List<Transform> DeckPositions = new List<Transform>();

        [SerializeField]
        GameObject boardGameObject = null;

        public CardWidget[] cards = null;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            cards = new CardWidget[CardSlots.Count];
            for (int i = 0; i < CardSlots.Count; i++)
                CardSlots[i].SlotNumber = i;
        }

        #endregion

        #region Card Functions
        
        public void AddCard(CardWidget cardWidget, BoardSlot boardSlot)
        {
            if (boardSlot.SlotNumber >= CardSlots.Count)
                return;

            cardWidget.transform.SetParent(boardSlot.transform);
            cardWidget.transform.localPosition = new Vector3(0, 0, -0.5f);
            cards[boardSlot.SlotNumber] = cardWidget;
            cardWidget.card.boardSlot = boardSlot.SlotNumber;
            boardSlot.SetCard(cardWidget);
        }

        public void MoveCard(int fromSlot, int toSlot)
        {
            cards[fromSlot].transform.position = CardSlots[toSlot].transform.position;
            cards[toSlot] = cards[fromSlot];
            cards[fromSlot] = null;
        }

        public void RemoveCard(int slot, Player player, Transform graveyard)
        {
            var cardWidget = cards[slot];
            player.SendToGraveYard(cardWidget);

            if (cardWidget != null && cardWidget.transform != null)
            {
                cardWidget.transform.parent = graveyard;
                cardWidget.transform.localPosition = new Vector3(0, 0, -0.5f);
            }

            CardSlots[slot].SetCard(null);

            cards[slot] = null;
        }

        #endregion

        #region Getters

        public bool GetIsSlotEmpty(BoardSlot boardSlot) => cards[boardSlot.SlotNumber] == null;

        public BoardSlot.CycleType GetCycleType(int boardSlot) => CardSlots[boardSlot].GetCycleType();

        public List<BoardSlot> GetOpponentBoardSlots()
        {
            var opponentBoardSlots = CardSlots.Skip(numberOfCardSlots).ToList();
            return opponentBoardSlots;
        }

        #endregion

        #region Setters

        public void SetCycle(BoardSlot.CycleType cycle, int boardSlot)
        {
            if (cycle == BoardSlot.CycleType.Light)
                CardSlots[boardSlot].OnLight();
            else if (cycle == BoardSlot.CycleType.Shadow)
                CardSlots[boardSlot].OnDark();
        }

        #endregion

        #region Editor
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (boardGameObject == null)
                return;

            var width = boardGameObject.transform.localScale.x;
            var height = boardGameObject.transform.localScale.y;

            var startX = (width / numberOfCardSlots);
            var playerY = height / 4f - height / 2;

            for (int i = 0; i < CardSlots.Count; i++)
            {
                var positionX = (startX * (i % numberOfCardSlots)) - (width / 2 - width / numberOfCardSlots / 2);
                var positionY = (playerY + (height / 2f * (int)(i / numberOfCardSlots)));
                CardSlots[i].transform.position = new Vector3(positionX, positionY, CardSlots[i].transform.position.z);
            }
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            CardSlots.ForEach(Slot => Handles.DrawSolidArc(Slot.transform.position, Slot.transform.forward, Slot.transform.up, 360f, 1f));

            Handles.color = Color.blue;
            DeckPositions.ForEach(Slot => Handles.DrawSolidArc(Slot.position, Slot.forward, Slot.up, 360f, 1f));
        }
#endif
        #endregion
    }
}
