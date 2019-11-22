using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public GameObject loadingPanal;
    public Slider loadingSlider;
    public TextMeshProUGUI sliderText;

	

    public void LoadMyLevel(int sceneIndex)
    {
        loadingPanal.SetActive(true);
        StartCoroutine(LoadAsyncronously(sceneIndex));
    }


    IEnumerator LoadAsyncronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
       
        while (!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            sliderText.text = Mathf.Round((progress * 100f)) + "%";

            if (progress > 0.49)
            {
                sliderText.color = Color.white;
            }

            yield return null;
        }
    }
}
