using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    Slider loadingBar;   // 슬라이더 사용하여 로딩바 구현

    private void Start()
    {
        Time.timeScale = 1;
        // "LoadSceneAfterDelay" 메서드를 2초 후에 호출
        Invoke("LoadSceneAfterDelay", 2f);
    }

    // 딜레이 후에 실행할 메서드
    void LoadSceneAfterDelay()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    // 비동기 호출
    IEnumerator LoadScene()
    {
        Debug.Log("LoadScene 코루틴 실행");
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, op.progress, timer);
                if (loadingBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, timer);
                if (loadingBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}