using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AltarCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject CutscenePanel;   // 컷신 패널
    public bool isPlayed = false;   // 실행 여부

    void Start()
    {
        // PlayableDirector의 stopped 이벤트에 메소드 연결
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

        if (collision.CompareTag("Player") && StageManager.Instance.allStageCleared) // 이벤트 컷신 실행 조건 확인
        {
            PlayCutscene();
            CutscenePanel.SetActive(true); // UI 요소를 활성화합니다.
            Invoke("DeactivateCutscenePanel", 3f); // 3초 후 DeactivateTutorialPanel 메서드를 호출
        }
    }

    // 대화창 패널 비활성화
    void DeactivateCutscenePanel()
    {
        if (CutscenePanel != null)
        {
            CutscenePanel.SetActive(false);
        }
    }

    // 컷신이 끝날 때 호출될 메소드
    void OnCutsceneStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            GameManager.instance.isLive = true; // 컷신이 끝나면 isLive를 true로 설정
            playableDirector.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 이벤트 구독 해제
        playableDirector.stopped -= OnCutsceneStopped;
    }
}
