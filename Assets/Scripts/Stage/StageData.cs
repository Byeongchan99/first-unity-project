using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageData 스크립터블 오브젝트
[CreateAssetMenu(fileName = "Stage", menuName = "Scriptble Object/StageData")]   // 커스텀 메뉴를 생성하는 속성
public class StageData : ScriptableObject
{
    [Header("Stage Settings")]
    public string stageName; // 스테이지 이름
    [Tooltip("이 스테이지의 ID")]
    public int stageID;
    [Tooltip("이 스테이지의 웨이브 정보")]
    public List<WaveData> waves; // 이 스테이지의 웨이브 정보
    public GameObject stagePrefab; // 스테이지 오브젝트의 프리팹
    public Vector3 startPosition; // 스테이지 시작 위치
}
