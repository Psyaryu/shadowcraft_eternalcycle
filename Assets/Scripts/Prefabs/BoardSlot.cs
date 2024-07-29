using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class BoardSlot : MonoBehaviour
    {
        public enum CycleType
        {
            Light,
            Shadow
        }

        public int SlotNumber = 0;

        [SerializeField]
        private MeshRenderer meshRenderer = null;

        [SerializeField]
        private Material material = null;

        [SerializeField]
        private Color DarkColor = Color.black;

        [SerializeField]
        private Color LightColor = Color.white;

        [SerializeField]
        private Color HoverLightColor = Color.green;

        [SerializeField]
        private Color HoverDarkColor = Color.green;

        [SerializeField]
        private Color HoverCantPlaceLightColor = Color.green;

        [SerializeField]
        private Color HoverCantPlaceDarkColor = Color.green;

        [SerializeField]
        private Color HoverFilledLightColor = Color.green;

        [SerializeField]
        private Color HoverFilledDarkColor = Color.green;

        private CycleType cycleType = CycleType.Light;

        private Card card = null;

        public void SetCard(Card card) => this.card = card;

        private void Awake()
        {
            meshRenderer.material = Instantiate(material);
        }

        public void OnDark()
        {
            meshRenderer.material.color = DarkColor;
            cycleType = CycleType.Shadow;
        }

        public void OnLight()
        {
            meshRenderer.material.color = LightColor;
            cycleType = CycleType.Light;
        }

        public void OnHover()
        {
            if (InputManager.shared.GetCardWidget() != null && !BattleManager.shared.CanPlaceCardInSlot(this))
            {
                meshRenderer.material.color = cycleType == CycleType.Light ? HoverCantPlaceLightColor : HoverCantPlaceDarkColor;
                return;
            }

            if (card == null)
                meshRenderer.material.color = cycleType == CycleType.Light ? HoverLightColor : HoverDarkColor;
            else
                meshRenderer.material.color = cycleType == CycleType.Light ? HoverFilledLightColor : HoverFilledDarkColor;
        }

        public void OnHoverExit()
        {
            meshRenderer.material.color = cycleType == CycleType.Light ? LightColor : DarkColor;
        }
    }
}
