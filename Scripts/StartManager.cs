using UnityEngine;
using UnityEngine.AddressableAssets;
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

    private SceneInstance currentScene;
    private bool canLoadScene = true;

    // Start is called before the first frame update
    void Awake()
    {
        level1.GetComponent<Button>().onClick.AddListener(() => LoadSceneByKey("Level_1"));
        level2.GetComponent<Button>().onClick.AddListener(() => LoadSceneByKey("Level_2"));
        level3.GetComponent<Button>().onClick.AddListener(() => LoadSceneByKey("Level_3"));
        backup.GetComponent<Button>().onClick.AddListener(UnLoadScene);
        backup.SetActive(false);
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