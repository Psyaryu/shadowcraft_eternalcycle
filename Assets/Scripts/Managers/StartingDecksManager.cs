using ShadowCraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingDecksManager : MonoBehaviour
{
    public List<Card> SetBaseEnemyDeck(Player enemy)
    {
        List<Deck> Deck = new List<Deck>();

        AIPlayer.share.AddToDeck("SoldierofRain");
        AIPlayer.share.AddToDeck("SoldierofRain");
        AIPlayer.share.AddToDeck("SoldierofNature");
        AIPlayer.share.AddToDeck("SoldierofNature");
        AIPlayer.share.AddToDeck("SoldierofFlames");
        AIPlayer.share.AddToDeck("SoldierofFlames");
    }

    public void SetBasePlayerDeck(Player player)
    {
        List<Deck> Deck = new List<Deck>();
        Player.shared.AddToDeck("SoldierofLight");
        Player.shared.AddToDeck("SoldierofLight");
        Player.shared.AddToDeck("SoldierofShadows");
        Player.shared.AddToDeck("SoldierofShadows");
        Player.shared.AddToDeck("SoldierofRain");
        Player.shared.AddToDeck("SoldierofRain");

    }
}
