using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptble Object/WaveData")]   // 커스텀 메뉴를 생성하는 속성
public class WaveData : ScriptableObject
{
    [Header("웨이브 설정")]
    [Tooltip("각 웨이브에서 소환될 몬스터 정보")]
    public List<SpawnInfo> spawnInfos; // 이 웨이브에서 소환될 몬스터의 정보

    [Tooltip("이 웨이브의 스폰 포인트 위치 정보")]
    public Vector2[] spawnPointsPositions; // 이 웨이브의 스폰 포인트 위치 정보
}