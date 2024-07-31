using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowCraft
{
    public class AIPlayer : Player
    {
        
        public AIPlayer(CharacterAsset characterAsset) : base(characterAsset)
        {
            identity = 0;
        }

        public override void SetDeck()
        {
         
            StartingDecksManager.shared.SetBaseEnemyDeck(this);
        }

        public override CardWidget Draw()
        {
            CardWidget card = base.Draw();
            return card;
        }
        
        public override IEnumerator StandByPhase()
        {

            var gameBoard = BattleManager.shared.GetGameBoard();
            var boardSlots = gameBoard.GetOpponentBoardSlots();

            List<CardWidget> temphand = new List<CardWidget>(hand);

            List<CardWidget> tempCard = new List<CardWidget>();

            foreach (var cardWidget in temphand)
            {
                foreach (var boardSlot in boardSlots)
                {
                    if (!BattleManager.shared.CanPlaceCardInSlot(boardSlot))
                        continue;

                    BattleManager.shared.AddCardToBoardSlot(cardWidget, boardSlot, this);

                    if (boardSlot.GetIsFilled())
                    {
                        tempCard.Add(cardWidget);
                        break;
                    }
                }                
            }

            //tempCard.ForEach(CardWidget => GameObject.Destroy(CardWidget.gameObject));

            yield return null;
        }
    }
       
}
