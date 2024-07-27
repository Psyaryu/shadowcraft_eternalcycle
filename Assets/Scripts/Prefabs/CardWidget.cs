using UnityEngine;

namespace ShadowCraft
{
    public class CardWidget : MonoBehaviour
    {
        public Card card { get; set; }
        public bool isPlaced = false;

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
