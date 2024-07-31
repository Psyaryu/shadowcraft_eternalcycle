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

        player.AddToDeck("Bear");
        player.AddToDeck("Druid");
        player.AddToDeck("Torch");
        player.AddToDeck("FlameEater");
        player.AddToDeck("DarkRitual");
        player.AddToDeck("Nightmare");
        player.AddToDeck("Whirlpool");
        player.AddToDeck("Paladin");
        player.AddToDeck("VampireBat");
        player.AddToDeck("UndeadKnight");
        player.AddToDeck("Torchbearer");
        player.AddToDeck("RedDragon");
        player.AddToDeck("ChaosBringer");
        player.AddToDeck("Treant");
        player.AddToDeck("Wolf");
        player.AddToDeck("Revalation");
        player.AddToDeck("DarkPact");
        player.AddToDeck("ShadowAssasin");
        player.AddToDeck("YinYang");

    }
}
