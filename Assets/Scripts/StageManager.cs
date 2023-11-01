using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public string stageName; // 스테이지 이름
        public Transform stageTransform; // 스테이지 오브젝트의 Transform
        public Vector3 startPosition; // 스테이지 시작 위치(포탈 위치)
    }

    public static StageManager Instance { get; private set; }

    public List<Stage> stages; // 모든 스테이지의 리스트
    private Stage currentStage; // 현재 활성화된 스테이지

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 스테이지로 이동하는 메서드
    public void TransitionToStage(int stageIndex)
    {
        if (currentStage != null)
        {
            currentStage.stageTransform.gameObject.SetActive(false); // 현재 스테이지 비활성화
        }

        currentStage = stages[stageIndex];
        currentStage.stageTransform.gameObject.SetActive(true); // 새 스테이지 활성화

        // 스테이지 Transform 기준으로 정의된 시작 위치를 월드 좌표로 변환
        Vector3 worldStartPosition = currentStage.stageTransform.TransformPoint(currentStage.startPosition);

        // 플레이어를 새 스테이지의 시작 위치로 이동
        PlayerStat.Instance.transform.position = worldStartPosition;
    }

}