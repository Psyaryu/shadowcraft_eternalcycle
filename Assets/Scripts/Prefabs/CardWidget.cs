using TMPro;
using UnityEngine;

namespace ShadowCraft
{
    public class CardWidget : MonoBehaviour
    {
        public Card card { get; set; }
        public bool isPlaced = false;
        private string[] manaTypes = { "Light", "Fire", "Water", "Nature", "Shadow", "Death" };
        private Color[] manaColors = { Color.yellow, Color.red, Color.cyan, Color.green, Color.gray, Color.black };

        [SerializeField]
        TextMeshProUGUI attackText = null;

        [SerializeField]
        TextMeshProUGUI healthText = null;

        [SerializeField]
        TextMeshProUGUI cardNameText = null;

        [SerializeField]
        TextMeshProUGUI mana1Text = null;

        [SerializeField]
        TextMeshProUGUI mana2Text = null;

        [SerializeField]
        TextMeshProUGUI mana3Text = null;

        [SerializeField]
        TextMeshProUGUI descriptionText = null;

        private void Start()
        {
            if (card != null)
            {
                attackText.text = card.attack.ToString();
                healthText.text = card.health.ToString();
                cardNameText.text = card.cardName;
                descriptionText.text = card.description.ToString();
                SetMana();
            }
        }

        private void Update()
        {
            if (card == null)
                return;

            attackText.text = card.attack.ToString();
            healthText.text = card.health.ToString();
            cardNameText.text = card.cardName;
            descriptionText.text = card.description.ToString();
            SetMana();         
        }

        public void SetMana()
        {
            var manaCosts = this.card.manaCost;

            TMP_Text[] manaTexts = { mana1Text, mana2Text, mana3Text };
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
                        manaTexts[manaIndex].color = manaColors[manaIndex];
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
