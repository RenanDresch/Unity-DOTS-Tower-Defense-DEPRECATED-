using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup rootCG = default;
    [SerializeField]
    private Button startButton = default;
    [SerializeField]
    private Button quitButton = default;

    private IEnumerator _menuFadeCoroutine;
    private IEnumerator MenuFadeCoroutine
    {
        get
        {
            return _menuFadeCoroutine;
        }
        set
        {
            if(_menuFadeCoroutine != null)
            {
                StopCoroutine(_menuFadeCoroutine);
            }
            _menuFadeCoroutine = value;
            StartCoroutine(_menuFadeCoroutine);
        }
    }

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitApplication);
    }

    private void StartGame()
    {
        MenuFadeCoroutine = CRFadeOutCoroutine(UnloadMainMenuLoadOperation);
    }

    private void QuitApplication()
    {

    }

    private void UnloadMainMenuLoadOperation()
    {
        ApplicationManager.Instance.UnloadScene("MainMenu", LoadNewGameScene);
    }

    private void LoadNewGameScene()
    {
        ApplicationManager.Instance.LoadScene("Game", GameSceneLoaded);
    }

    private void GameSceneLoaded()
    {
        ApplicationManager.Instance.SwitchLoader(false);
    }

    private IEnumerator CRFadeOutCoroutine(System.Action callback)
    {
        rootCG.blocksRaycasts = false;
        rootCG.interactable = false;
        while(rootCG.alpha > 0)
        {
            rootCG.alpha -= Time.unscaledDeltaTime * 2;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1);
        ApplicationManager.Instance.SwitchLoader(true, callback);
    }
}
