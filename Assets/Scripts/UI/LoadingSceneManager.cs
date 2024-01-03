using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    Slider loadingBar;   // �����̴� ����Ͽ� �ε��� ����

    private void Start()
    {
        Time.timeScale = 1;
        // "LoadSceneAfterDelay" �޼��带 2�� �Ŀ� ȣ��
        Invoke("LoadSceneAfterDelay", 2f);
    }

    // ������ �Ŀ� ������ �޼���
    void LoadSceneAfterDelay()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    // �񵿱� ȣ��
    IEnumerator LoadScene()
    {
        Debug.Log("LoadScene �ڷ�ƾ ����");
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