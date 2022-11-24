using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
[CreateAssetMenu(menuName = "SceneSO/SceneSO", fileName = "SceneSO")]
public class SceneSO : ScriptableObject
{
    [SerializeField] string sceneName;

    public void LoadScene(LoadSceneMode loadSceneMode)
    {
        AsyncOperation handle = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
    }
    public void LoadSceneWithLoadedAction(LoadSceneMode loadSceneMode, Action onLoaded)
    {
        AccountData.Instance.StartCoroutine(LoadScene(loadSceneMode, onLoaded));
    }
    IEnumerator LoadScene(LoadSceneMode loadSceneMode, Action onLoaded)
    {
        AsyncOperation handle = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        yield return handle;
        onLoaded?.Invoke();
    }
    public void LoadScene(int idLoadSceneMode)
    {
        LoadScene((LoadSceneMode)idLoadSceneMode);
    }
    public void UnloadScene()
    {
        AsyncOperation handle = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        handle.completed += (handle) =>
        {
            Resources.UnloadUnusedAssets();
        };
    }
}
