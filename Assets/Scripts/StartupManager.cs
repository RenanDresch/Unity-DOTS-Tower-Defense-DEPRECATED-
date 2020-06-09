using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnRuntimeMethodLoad()
    {
        if(!SceneManager.GetSceneByName("Defaults").isLoaded)
        {
            SceneManager.LoadScene("Defaults", LoadSceneMode.Additive);
        }
    }
}