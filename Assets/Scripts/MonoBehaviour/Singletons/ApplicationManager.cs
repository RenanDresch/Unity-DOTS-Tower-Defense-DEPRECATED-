using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup loaderCG = default;

    private Dictionary<AsyncOperation, System.Action> callbacks = new Dictionary<AsyncOperation, System.Action>();

    private static ApplicationManager _instance;
    public static ApplicationManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameObject("Application Manager", new System.Type[] { typeof(ApplicationManager) }).GetComponent<ApplicationManager>();
            }
            return _instance;
        }
        set
        {
            if(_instance == null)
            {
                _instance = value;
            }
            else
            {
                Destroy(value.gameObject);
            }
        }
    }

    private IEnumerator _loaderCoroutine;
    private IEnumerator LoaderCoroutine
    {
        get
        {
            return _loaderCoroutine;
        }
        set
        {
            if(_loaderCoroutine != null)
            {
                StopCoroutine(_loaderCoroutine);
            }
            _loaderCoroutine = value;
            StartCoroutine(_loaderCoroutine);
        }
    }

    private void Awake()
    {
        _instance = this;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

    }
    public void SwitchLoader(bool state, System.Action callback = null)
    {
        LoaderCoroutine = CRSwitchLoader(state, callback);
    }

    public void LoadScene(string scene, System.Action callback = null)
    {
        var operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        if (callback != null)
        {
            operation.completed += SafeInvokeCallback;
            callbacks.Add(operation, callback);
        }
    }

    public void UnloadScene(string scene, System.Action callback = null)
    {
        var operation = SceneManager.UnloadSceneAsync(scene);
        if (callback != null)
        {
            operation.completed += SafeInvokeCallback;
            callbacks.Add(operation, callback);
        }
    }

    private void SafeInvokeCallback(AsyncOperation operation)
    {
        callbacks[operation].Invoke();
        operation.completed -= SafeInvokeCallback;
        callbacks.Remove(operation);
    }

    private IEnumerator CRSwitchLoader(bool state, System.Action callback)
    {
        if(!state)
        {
            yield return new WaitForSecondsRealtime(1);
        }
        loaderCG.blocksRaycasts = state;
        while(loaderCG.alpha != (state ? 1 : 0))
        {
            loaderCG.alpha += Time.unscaledDeltaTime * (state ? 1 : -2);
            yield return null;
        }
        if(callback != null)
        {
            callback.Invoke();
        }
    }

}
