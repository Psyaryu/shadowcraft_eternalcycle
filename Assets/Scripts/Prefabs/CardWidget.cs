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
        private TMP_Text mana1;
        private TMP_Text mana2;
        private TMP_Text mana3;
        private TMP_Text description;
        public bool isPlaced = false;
        private string[] manaTypes = { "Light", "Fire", "Water", "Nature", "Shadow", "Death" };

        private void Start()
        {
            if (transform.Find("Attack") != null)
            {
                attack = transform.Find("Attack").GetComponent<TMP_Text>();
                health = transform.Find("Health").GetComponent<TMP_Text>();
                cardName = transform.Find("Name").GetComponent<TMP_Text>();
                Transform manaTransform = transform.Find("Mana");
                mana1 = manaTransform.Find("Mana1").GetComponent<TMP_Text>();
                mana2 = manaTransform.Find("Mana2").GetComponent<TMP_Text>();
                mana3 = manaTransform.Find("Mana3").GetComponent<TMP_Text>();
                description = transform.Find("Description").GetComponent<TMP_Text>();

                attack.text = card.attack.ToString();
                health.text = card.health.ToString();
                cardName.text = card.cardName;
                description.text = card.description.ToString();
                SetMana();
            }

        }

        private void Update()
        {
            if (transform.Find("Attack") != null)
            {
                attack = transform.Find("Attack").GetComponent<TMP_Text>();
                health = transform.Find("Health").GetComponent<TMP_Text>();
                cardName = transform.Find("Name").GetComponent<TMP_Text>();
                Transform manaTransform = transform.Find("Mana");
                mana1 = manaTransform.Find("Mana1").GetComponent<TMP_Text>();
                mana2 = manaTransform.Find("Mana2").GetComponent<TMP_Text>();
                mana3 = manaTransform.Find("Mana3").GetComponent<TMP_Text>();
                description = transform.Find("Description").GetComponent<TMP_Text>();

                attack.text = card.attack.ToString();
                health.text = card.health.ToString();
                cardName.text = card.cardName;
                description.text = card.description.ToString();
                SetMana();
            }
         
        }
        public void SetMana()
        {
            var manaCosts = this.card.manaCost;

            TMP_Text[] manaTexts = { mana1, mana2, mana3 };
            foreach (var text in manaTexts)
            {
                text.text = "";
            }

            int manaIndex = 0;
            for (int i = 0; i < manaCosts.Length; i++)
            {
                if (manaCosts[i] > 0)
                {
                    if (manaIndex < manaTexts.Length)
                    {
                        manaTexts[manaIndex].text = $"{manaTypes[i]}: {manaCosts[i]}";
                        manaIndex++;
                    }
                }
            }

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
            BattleManager.shared.AddCardToBoardSlot(this, boardSlot, BattleManager.shared.player);
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

        public void ResetCard()
        {
            card.ResetCard();
        }
    }
}
