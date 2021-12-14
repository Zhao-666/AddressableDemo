using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [Header("Button")]
    //UI Button
    [SerializeField]
    private GameObject level1;

    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level3;

    // Start is called before the first frame update
    void Awake()
    {
        level1.GetComponent<Button>().onClick.AddListener(() => ButtonClick("Level_1"));
        level2.GetComponent<Button>().onClick.AddListener(() => ButtonClick("Level_2"));
        level3.GetComponent<Button>().onClick.AddListener(() => ButtonClick("Level_3"));
    }

    private void ButtonClick(string sceneName)
    {
        Addressables.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
    }
}