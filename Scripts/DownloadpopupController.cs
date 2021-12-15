using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class DownloadpopupController : MonoBehaviour
{
    [SerializeField] private Text size;
    [SerializeField] private Text percent;
    [SerializeField] private Button submitBtn;
    [SerializeField] private Button cancelBtn;

    private long _sizeCount = 0;
    private string _downloadKey;

    void Awake()
    {
        submitBtn.onClick.AddListener(() => { StartCoroutine(DownloadAB()); });
        cancelBtn.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void InitSize(string key, long sizeNum)
    {
        _downloadKey = key;
        _sizeCount = sizeNum;
        size.text = "Download AB Size: " + Math.Round(sizeNum / 1024.0 / 1024, 2) + "MB";
        percent.text = "0.00%";
    }

    private IEnumerator DownloadAB()
    {
        var handle = Addressables.DownloadDependenciesAsync(_downloadKey,true);
        handle.Completed += obj =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                SendMessageUpwards("DownloadedCallback", _downloadKey);
                gameObject.SetActive(false);
            }
        };
        while (!handle.IsDone)
        {
            var state = handle.GetDownloadStatus();
            percent.text = Math.Round(state.Percent * 100, 2) + "%";
            yield return null;
        }
    }
}