using UnityEngine;

namespace ShadowCraft
{
    public class InputManager : MonoBehaviour
    {
        #region Properties

        public static InputManager shared = null;

        private CardWidget currentCard = null;
        private BoardSlot boardSlot = null;

        private Vector3 LastPosition = Vector3.zero;

        private bool IsMouseDown = false;
        private bool IsDragging = false;
        private bool IsHover = false;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (shared == null)
            {
                shared = this;
                DontDestroyOnLoad(this);
            }
            else
                Destroy(this);
        }

        void Update()
        {
            UpdateCard();
            UpdateBoardSlot();
        }

        #endregion

        #region Game Object Tracker

        private void UpdateCard()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var mask = LayerMask.GetMask("Card");
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, mask))
            {
                if (IsHover)
                {
                    currentCard?.OnHoverExit();
                    currentCard = null;
                    IsHover = false;
                    IsMouseDown = false;
                    IsDragging = false;
                }
                return;
            }

            if (hitInfo.collider.tag != "Card")
                return;

            var card = hitInfo.collider.gameObject.GetComponentInParent<CardWidget>();

            if (card == null)
                card = hitInfo.collider.transform.parent.GetComponentInParent<CardWidget>();

            if (!IsHover)
            {
                currentCard?.OnHoverExit();

                IsHover = true;
                card.OnHoverEnter();
                currentCard = card;
                return;
            }

            if (!Input.GetMouseButton(0))
            {
                if (IsMouseDown)
                    card.OnClickRelease();

                IsMouseDown = false;
                IsDragging = false;
                return;
            }

            if (!IsDragging && !IsMouseDown)
            {
                card.OnClick();
                IsMouseDown = true;
                return;
            }

            var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
            var location = Camera.main.ScreenToWorldPoint(screenPoint);

            if (!IsDragging)
            {
                IsDragging = true;
                card.OnDrag(location);
                return;
            }

            if (location == LastPosition)
                return;

            LastPosition = location;
            card.OnDrag(location);
        }

        private void UpdateBoardSlot()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var mask = LayerMask.GetMask("BoardSlot");

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, mask))
            {
                if (boardSlot != null)
                {
                    boardSlot?.OnHoverExit();
                    boardSlot = null;
                }
                return;
            }

            if (hitInfo.collider.tag != "BoardSlot")
                return;

            var boardSlotHit = hitInfo.collider.transform.parent.GetComponent<BoardSlot>();

            if (boardSlot == null)
            {
                boardSlot = boardSlotHit;
                boardSlot.OnHover();
                return;
            }

            if (boardSlot != boardSlotHit)
            {
                boardSlot.OnHoverExit();
                boardSlot = boardSlotHit;
                boardSlot.OnHover();
            }
        }

        #endregion

        #region Getters

        public BoardSlot GetBoardSlot() => boardSlot;
        public CardWidget GetCardWidget() => currentCard;

        #endregion
    }
}
