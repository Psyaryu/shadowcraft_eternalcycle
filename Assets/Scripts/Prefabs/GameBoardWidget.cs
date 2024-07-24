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
        [SerializeField]
        int numberOfCardSlots = 5;

        [SerializeField]
        List<Transform> CardSlots = new List<Transform>();

        [SerializeField]
        List<Transform> DeckPositions = new List<Transform>();

        [SerializeField]
        GameObject boardGameObject = null;

        CardWidget[] cards = null;

        private void Awake()
        {
            cards = new CardWidget[CardSlots.Count];
        }

        public void AddCard(CardWidget cardWidget, int slot)
        {
            if (slot >= CardSlots.Count)
                return;

            cardWidget.transform.position = CardSlots[slot].position;
            cards[slot] = cardWidget;
        }

        public void MoveCard(int fromSlot, int toSlot)
        {
            cards[fromSlot].transform.position = CardSlots[toSlot].position;
            cards[toSlot] = cards[fromSlot];
            cards[fromSlot] = null;
        }

        public void RemoveCard(int slot)
        {
            cards[slot] = null;
        }

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
                CardSlots[i].position = new Vector3(positionX, positionY, CardSlots[i].position.z);
            }
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            CardSlots.ForEach(Slot => Handles.DrawSolidArc(Slot.position, Slot.forward, Slot.up, 360f, 1f));

            Handles.color = Color.blue;
            DeckPositions.ForEach(Slot => Handles.DrawSolidArc(Slot.position, Slot.forward, Slot.up, 360f, 1f));
        }
#endif
    }
}
