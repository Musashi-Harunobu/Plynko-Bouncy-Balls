using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image progressFillImage;
    
    [SerializeField] private float minLoadTime = 1f;

    public void LoadScene(int sceneIndex)
    {
        loadingPanel.SetActive(true);

        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        float startTime = Time.time;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (Time.time - startTime < minLoadTime)
        {
            float t = (Time.time - startTime) / minLoadTime;
            if (progressFillImage != null)
                progressFillImage.fillAmount = t;
            yield return null;
        }

        if (progressFillImage != null)
            progressFillImage.fillAmount = 1f;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        loadingPanel.SetActive(false);
    }
}