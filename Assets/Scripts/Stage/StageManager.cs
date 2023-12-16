using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    //public PolygonCollider2D boundingShape; // Cinemachine Confiner 컴포넌트 참조 용 콜라이더
    public List<StageData> stages; // 모든 스테이지의 데이터 리스트
    public StageData currentStage; // 현재 활성화된 스테이지 데이터
    private Dictionary<int, GameObject> stageInstances = new Dictionary<int, GameObject>(); // 스테이지 인스턴스 저장
    // 스테이지 완료 상태를 추적하는 Dictionary
    public Dictionary<int, bool> completedStages;

    [Header("# 게임 진행 상태")]
    public Dictionary<int, bool> completedRuneStages;   // 메인 스테이지의 룬 기둥을 활성화 하기 위한 클리어한 스테이지 개수, 숲, 평원, 사막, 설원 순서

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            completedStages = new Dictionary<int, bool>();
            completedRuneStages = new Dictionary<int, bool>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 시작 스테이지를 제외하고 비활성화
    private void Start()
    {
        // 모든 스테이지 인스턴스 생성 및 비활성화
        for (int i = 0; i < stages.Count; i++)
        {
            if (stages[i] != null)
            {
                GameObject stageInstance = Instantiate(stages[i].stagePrefab);
                stageInstance.SetActive(i == 0); // 첫 번째 스테이지만 활성화
                stageInstances.Add(i, stageInstance); // 인스턴스 저장
            }
        }

        // 첫 번째 스테이지를 현재 스테이지로 설정
        currentStage = stages[0];
        //boundingShape.enabled = false;
    }

    public Vector2 TilemapToWorldPosition(Vector2Int tilemapPos, Vector2Int bottomLeft, Vector2Int topRight)   // 타일맵 좌표를 월드 좌표로 변환
    {
        // 여기서 bottomLeft를 사용하여 올바른 월드 좌표를 계산합니다.
        return new Vector2(tilemapPos.x * 0.5f + bottomLeft.x, tilemapPos.y * 0.5f + bottomLeft.y);
    }

    // 벽 노드 배열인 NodeArray를 초기화하는 메서드
    public void InitializeNodeArray()
    {
        int sizeX, sizeY;   // 맵 크기
        Vector2Int bottomLeft, topRight;   // 맵의 하단 좌측과 상단 우측의 월드 좌표

        bottomLeft = currentStage.bottomLeft;
        topRight = currentStage.topRight;

        sizeX = Mathf.Abs(topRight.x - bottomLeft.x) * 2;  // 타일맵 가로 크기
        sizeY = Mathf.Abs(topRight.y - bottomLeft.y) * 2;  // 타일맵 세로 크기

        currentStage.NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                Vector2 worldPosition = TilemapToWorldPosition(new Vector2Int(i, j), bottomLeft, topRight);
                foreach (Collider2D col in Physics2D.OverlapCircleAll(worldPosition, 0.1f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                currentStage.NodeArray[i, j] = new Node(isWall, i, j);
            }
        }
    }

    // 스테이지로 이동하는 메서드
    public void TransitionToStage(int stageIndex)
    {
        if (stageInstances.ContainsKey(stageIndex))
        {
            if (currentStage != null)
            {
                // 이전 스테이지 비활성화
                stageInstances[currentStage.stageID].SetActive(false);
                //boundingShape.enabled = false;
            }

            currentStage = stages[stageIndex];
            Debug.Log("이동할 스테이지: " + currentStage.stageID);
            // 새 스테이지 활성화
            stageInstances[stageIndex].SetActive(true);
            InitializeNodeArray();   // NodeArray 초기화

            // 플레이어를 새 스테이지의 시작 위치로 이동
            PlayerStat.Instance.transform.position = currentStage.startPosition;
            //UpdateConfinerBounds();   // Confiner 업데이트

            // 전투 스테이지일 경우 몬스터 소환
            if (currentStage.stageType == "battle" || currentStage.stageType == "boss") 
                WaveManager.Instance.StartWave();
        }
    }

    /*
    // 시네머신 Confiner 업데이트
    private void UpdateConfinerBounds()
    {
        if (boundingShape != null && currentStage != null)
        {
            int topRightX = currentStage.topRight.x;
            int topRightY = currentStage.topRight.y;
            int bottomLeftX = currentStage.bottomLeft.x;
            int bottomLeftY = currentStage.bottomLeft.y;

            Vector2[] boxPoints = new Vector2[5];

            // 사각형의 꼭짓점 설정
            boxPoints[0] = new Vector2(bottomLeftX - 3, bottomLeftY - 3);  // 왼쪽 하단
            boxPoints[1] = new Vector2(topRightX + 3, bottomLeftY - 3);   // 오른쪽 하단
            boxPoints[2] = new Vector2(topRightX + 3, topRightY + 3);    // 오른쪽 상단
            boxPoints[3] = new Vector2(bottomLeftX - 3, topRightY + 3);   // 왼쪽 상단
            boxPoints[4] = boxPoints[0]; // 마지막 꼭짓점은 처음 꼭짓점과 같아야 폴리곤이 닫힌다.

            boundingShape.SetPath(0, boxPoints);
        }
        boundingShape.enabled = true;
    }
    */

    // 스테이지 완료 상태 업데이트 메서드
    public void SetStageCompleted(int stageID, bool completed)
    {
        if (!completedStages.ContainsKey(stageID))
        {
            completedStages.Add(stageID, completed);
        }
        else
        {
            completedStages[stageID] = completed;
        }
    }

    // 스테이지 완료 여부 확인 메서드
    public bool IsStageCompleted(int stageID)
    {
        return completedStages.ContainsKey(stageID) && completedStages[stageID];
    }

    // 룬 스테이지 완료 상태 업데이트 메서드
    public void SetRuneStageCompleted(int runeStageID, bool completed)
    {
        if (!completedRuneStages.ContainsKey(runeStageID))
        {
            completedRuneStages.Add(runeStageID, completed);
        }
        else
        {
            completedRuneStages[runeStageID] = completed;
        }
    }

    // 룬 스테이지 완료 여부 확인 메서드
    public bool IsRuneStageCompleted(int runeStageID)
    {
        return completedRuneStages.ContainsKey(runeStageID) && completedRuneStages[runeStageID];
    }
}
