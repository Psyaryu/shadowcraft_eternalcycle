using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                var nextBoardSlots = new List<BoardSlot>();
                nextBoardSlots.AddRange(boardSlots);

                var totalSlots = nextBoardSlots.Count;

                for (int i = 0; i < totalSlots; i++)
                {
                    var randomIndex = Random.Range(0, nextBoardSlots.Count);
                    var boardSlot = nextBoardSlots[randomIndex];
                    nextBoardSlots.RemoveAt(randomIndex);

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

            yield return null;
        }
    }
       
}
