using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager shared = null;

        private CardWidget currentCard = null;

        private Vector3 LastPosition = Vector3.zero;

        private bool IsMouseDown = false;
        private bool IsDragging = false;
        private bool IsHover = false;

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
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo))
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

            if (!IsDragging && !IsMouseDown) {
                card.OnClick();
                IsMouseDown = true;
                return;
            }

            if (!IsDragging)
            {
                IsDragging = true;
                card.OnDrag(Input.mousePosition);
                return;
            }

            if (Input.mousePosition == LastPosition)
                return;

            LastPosition = Input.mousePosition;
            card.OnDrag(Input.mousePosition);
        }
    }
}
