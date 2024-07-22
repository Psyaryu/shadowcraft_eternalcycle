using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowCraft
{
    public class BattleManager : MonoBehaviour
    {
        public void OnMainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
