using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AltarCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject CutscenePanel;   // �ƽ� �г�
    public bool isPlayed = false;   // ���� ����

    void Start()
    {
        // PlayableDirector�� stopped �̺�Ʈ�� �޼ҵ� ����
        playableDirector.stopped += OnCutsceneStopped;

        if (CutscenePanel != null )
        {
            CutscenePanel.SetActive( false );
        }
    }

    void PlayCutscene()
    {
        isPlayed = true;
        playableDirector.gameObject.SetActive(true);
        playableDirector.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayed)
            return;

        if (collision.CompareTag("Player") && StageManager.Instance.allStageCleared) // �̺�Ʈ �ƽ� ���� ���� Ȯ��
        {
            PlayCutscene();
            CutscenePanel.SetActive(true); // UI ��Ҹ� Ȱ��ȭ�մϴ�.
            Invoke("DeactivateCutscenePanel", 3f); // 3�� �� DeactivateTutorialPanel �޼��带 ȣ��
        }
    }

    // ��ȭâ �г� ��Ȱ��ȭ
    void DeactivateCutscenePanel()
    {
        if (CutscenePanel != null)
        {
            CutscenePanel.SetActive(false);
        }
    }

    // �ƽ��� ���� �� ȣ��� �޼ҵ�
    void OnCutsceneStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            GameManager.instance.isLive = true; // �ƽ��� ������ isLive�� true�� ����
            playableDirector.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // ������Ʈ�� �ı��� �� �̺�Ʈ ���� ����
        playableDirector.stopped -= OnCutsceneStopped;
    }
}
