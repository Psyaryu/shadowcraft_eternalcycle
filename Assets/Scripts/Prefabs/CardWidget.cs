using TMPro;
using UnityEngine;

namespace ShadowCraft
{
    public class CardWidget : MonoBehaviour
    {
        public Card card { get; set; }
        private TMP_Text attack;
        private TMP_Text health;
        private TMP_Text cardName;
        public bool isPlaced = false;

        private void Start()
        {
            attack = transform.Find("Attack").GetComponent<TMP_Text>();
            health = transform.Find("Health").GetComponent<TMP_Text>();
            cardName = transform.Find("Name").GetComponent<TMP_Text>();

            attack.text = card.attack.ToString();
            health.text = card.health.ToString();
            cardName.text = card.cardName;

        }

        public void OnHoverEnter()
        {

        }

        public void OnHoverExit()
        {

        }

        public void OnClick()
        {

        }

        public void OnClickRelease()
        {
            if (isPlaced)
                return;

            if (BattleManager.shared == null || !BattleManager.shared.GetIsStandByPhase())
                return;

            var boardSlot = InputManager.shared.GetBoardSlot();

            if (boardSlot == null)
            {
                BattleManager.shared.PositionHandCards();
                return;
            }

            if (!BattleManager.shared.CanPlaceCardInSlot(boardSlot))
            {
                BattleManager.shared.PositionHandCards();
                return;
            }

            isPlaced = true;
            BattleManager.shared.AddCardToBoardSlot(this, boardSlot);
        }

        public void OnDrag(Vector2 newLocation)
        {
            if (isPlaced)
                return;

            if (BattleManager.shared == null || !BattleManager.shared.GetIsStandByPhase())
                return;

            // I think I messed up and flipped the camera to the other side, so we have to reverse the directions so it works x.x
            transform.localPosition = new Vector3(-newLocation.x, -(newLocation.y + transform.parent.position.y), transform.localPosition.z);
        }
    }
}
