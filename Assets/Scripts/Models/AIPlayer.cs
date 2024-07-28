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

        public override Card Draw()
        {
            base.Draw();
            return null;
        }

        public override IEnumerator StandByPhase()
        {
            // TODO: Tell the BattleManager what cards to add
            // BattleManager.shared.AddCardToBoardSlot(CardWidget cardWidget, BoardSlot boardSlot)

            yield return null;
        }
    }
}
