using System;
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

        public CycleType cycleType = CycleType.Light;

        public CardWidget card = null;

        public int torchTurn = 0;
        public int chaosBringerTurn = 0;
        public bool redDragon = false;

        public void SetCard(CardWidget card) => this.card = card;

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

        public CycleType GetCycleType() => cycleType;

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

        public void UpdateEnchant()
        {
            if (torchTurn == BattleManager.shared.turnNumber)
            {
                int Oppositeslot = (SlotNumber + 5) % 10;

                if(BattleManager.shared.gameBoardWidget.CardSlots[Oppositeslot].cycleType == CycleType.Light)
                {
                    this.OnLight();
                }
                else
                {
                    this.OnDark();
                }
            }
            if (chaosBringerTurn == BattleManager.shared.turnNumber)
            {
                foreach (var slot in BattleManager.shared.gameBoardWidget.CardSlots)
                {
                    if (slot.GetCycleType() == CycleType.Light)
                    {
                        slot.OnDark();
                    }
                    if (slot.GetCycleType() == CycleType.Light)
                    {
                        slot.OnLight();
                    }
                }
            }
            if (redDragon == true)
            {

                int light1 = 0;
                int dark1 = 0;
                foreach (var slot in BattleManager.shared.gameBoardWidget.CardSlots)
                {
                   
                        if (slot.GetCycleType() == CycleType.Light)
                        {
                            light1++;
                        }
                        if (slot.GetCycleType() == CycleType.Shadow)
                        {
                            dark1++;
                        }
                }

                int attack = Math.Abs(light1 - dark1);
                this.card.card.attack = attack;
            }
        }
        public bool GetIsFilled() => card != null;
    }
}
