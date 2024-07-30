using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class AIPlayer : Player
    {
        public AIPlayer(CharacterAsset characterAsset) : base(characterAsset)
        {

        }

        protected override void SetDeck()
        {
            StartingDecksManager.shared.SetBaseEnemyDeck(this);
        }

        public override Card Draw()
        {
            base.Draw();
            return null;
        }

        public override IEnumerator StandByPhase()
        {
            var gameBoard = BattleManager.shared.GetGameBoard();
            var boardSlots = gameBoard.GetOpponentBoardSlots();

            foreach (var card in hand)
            {
                foreach (var boardSlot in boardSlots)
                {
                    if (!BattleManager.shared.CanPlaceCardInSlot(boardSlot))
                        continue;

                    BattleManager.shared.AddCardToBoardSlot(card, boardSlot);

                    if (boardSlot.GetIsFilled())
                        break;
                }                
            }

            yield return null;
        }
    }
}
