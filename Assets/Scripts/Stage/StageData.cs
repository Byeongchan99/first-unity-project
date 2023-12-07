using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageData 스크립터블 오브젝트
[CreateAssetMenu(fileName = "Stage", menuName = "Scriptble Object/StageData")]   // 커스텀 메뉴를 생성하는 속성
public class StageData : ScriptableObject
{
    [Header("Stage Settings")]
    public string stageName;   // 스테이지 이름
    public string stageType;   // 스테이지 종류
    [Tooltip("이 스테이지의 정보")]
    public int stageID;
    public Node[,] NodeArray;   // 벽 노드 확인
    [Tooltip("이 스테이지의 웨이브 정보")]
    public List<WaveData> waves;   // 이 스테이지의 웨이브 정보
    public GameObject stagePrefab;   // 스테이지 오브젝트의 프리팹
    public Vector2 startPosition;   // 스테이지 시작 위치
    public Vector2Int bottomLeft, topRight;   // 몬스터 Astar 알고리즘을 위한 맵 크기 월드 좌표
}
