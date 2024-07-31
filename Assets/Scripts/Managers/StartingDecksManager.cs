using ShadowCraft;
using UnityEngine;

public class StartingDecksManager : MonoBehaviour
{
    public static StartingDecksManager shared = null;

    private void Awake()
    {
        if (shared == null)
            shared = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    public void SetBaseEnemyDeck(Player enemy)
    {
        enemy.AddToDeck("SoldierofRain");
        enemy.AddToDeck("SoldierofRain");
        enemy.AddToDeck("SoldierofNature");
        enemy.AddToDeck("SoldierofNature");
        enemy.AddToDeck("SoldierofFlames");
        enemy.AddToDeck("SoldierofFlames");
    }

    public void SetBasePlayerDeck(Player player)
    {
        player.AddToDeck("SoldierofLight");
        player.AddToDeck("SoldierofLight");
        player.AddToDeck("SoldierofShadows");
        player.AddToDeck("SoldierofShadows");
        player.AddToDeck("SoldierofRain");
        player.AddToDeck("SoldierofRain");
    }
}