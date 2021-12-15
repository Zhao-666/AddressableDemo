using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [Header("UI Panel")]
    //UI Panel
    [SerializeField]
    private GameObject OptionPanel;

    [Header("Button")]
    //UI Button
    [SerializeField]
    private GameObject level1;

    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level3;
    [SerializeField] private GameObject backup;
    [SerializeField] private GameObject level2State;
    [SerializeField] private GameObject level3State;
    [SerializeField] private GameObject downloadPopup;

    private SceneInstance currentScene;
    private bool canLoadScene = true;

    // Start is called before the first frame update
    void Awake()
    {
        level1.GetComponent<Button>().onClick.AddListener(() => LevelButtonClick("Level_1"));
        level2.GetComponent<Button>().onClick.AddListener(() => LevelButtonClick("Level_2"));
        level3.GetComponent<Button>().onClick.AddListener(() => LevelButtonClick("Level_3"));
        backup.GetComponent<Button>().onClick.AddListener(UnLoadScene);
        level2State.GetComponent<Button>().onClick.AddListener(() =>
        {
            ClearAB("Level_2");
            level2State.SetActive(false);
        });
        level3State.GetComponent<Button>().onClick.AddListener(() =>
        {
            ClearAB("Level_3");
            level3State.SetActive(false);
        });

        backup.SetActive(false);

        CheckABDownloaded("Level_2", level2State);
        CheckABDownloaded("Level_3", level3State);
    }

    private void LevelButtonClick(string key)
    {
        Addressables.GetDownloadSizeAsync(key).Completed += obj =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("AB size: " + obj.Result);
                if (obj.Result == 0)
                {
                    LoadSceneByKey(key);
                }
                else
                {
                    downloadPopup.SetActive(true);
                    downloadPopup.GetComponent<DownloadpopupController>().InitSize(key, obj.Result);
                }
            }
        };
    }

    private void DownloadedCallback(string key)
    {
        switch (key)
        {
            case "Level_2":
                CheckABDownloaded(key, level2State);
                break;
            case "Level_3":
                CheckABDownloaded(key, level3State);
                break;
        }
    }

    private void CheckABDownloaded(string key, GameObject stateObj)
    {
        Addressables.GetDownloadSizeAsync(key).Completed += obj =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("AB size: " + obj.Result);
                if (obj.Result == 0)
                {
                    stateObj.SetActive(true);
                }
            }
        };
    }

    private void ClearAB(string key)
    {
        Addressables.ClearDependencyCacheAsync(key);
    }

    private void LoadSceneByKey(string sceneKey)
    {
        if (canLoadScene)
        {
            Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive).Completed
                += obj =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        canLoadScene = false;
                        currentScene = obj.Result;
                        OptionPanel.SetActive(false);
                        backup.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Load scene fail!!!");
                    }
                };
        }
    }

    private void UnLoadScene()
    {
        if (!canLoadScene)
        {
            Addressables.UnloadSceneAsync(currentScene).Completed
                += obj =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        canLoadScene = true;
                        currentScene = new SceneInstance();
                        backup.SetActive(false);
                        OptionPanel.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("UnLoad scene fail!!!");
                    }
                };
        }
    }
}