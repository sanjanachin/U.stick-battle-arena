using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class App
    {
        private static readonly string GLOBALMANAGERS_PATH = $"GlobalManagers";
        private static readonly SceneID GLOBALMANAGERS_SCENEID = SceneID.GlobalManagers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            // load globalManagers
            // GameObject app = Object.Instantiate(Resources.Load(GLOBALMANAGERS_PATH)) as GameObject;
            // if (app == null)
            //     throw new ApplicationException();
            //
            // Object.DontDestroyOnLoad(app);

            UnityEngine.SceneManagement.SceneManager.LoadScene(
                GLOBALMANAGERS_SCENEID.ToString());
            
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