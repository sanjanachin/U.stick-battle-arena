using UnityEngine;

namespace Game
{
    public class App
    {
        // Temporary fix for build
        // private static readonly SceneID GLOBALMANAGERS_SCENEID = SceneID.GlobalManagers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            // UnityEngine.SceneManagement.SceneManager.LoadScene(
            //     GLOBALMANAGERS_SCENEID.ToString());
            
            // temporary setting
            Application.targetFrameRate = 60;
        }

        public static void ExitApplication()
        {
            Application.Quit();
            Debug.Log("Exit!");
        }
    }
}