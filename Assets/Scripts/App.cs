using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public class App
    {
        private static readonly string GLOBALMANAGERS_PATH = $"GlobalManagers";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            // load globalManagers
            GameObject app = Object.Instantiate(Resources.Load(GLOBALMANAGERS_PATH)) as GameObject;
            if (app == null)
                throw new ApplicationException();
            
            Object.DontDestroyOnLoad(app);

            // temporary setting
            Application.targetFrameRate = 60;
        }
    }
}