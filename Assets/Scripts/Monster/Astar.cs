using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public class Astar : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight;   // 맵의 하단 좌측과 상단 우측의 월드 좌표
    public List<Node> FinalNodeList = new List<Node>();  // 바로 초기화하여 null 문제 제거
    public bool allowDiagonal, dontCrossCorner;
    public bool playerInWall = false;   // 플레이어가 Wall 타일 근처에 있을 때

    int sizeX, sizeY;   // 맵 크기
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public void Initialize(StageData stageData)
    {
        // Debug.Log("스테이지 정보" + stageData.stageID);
        bottomLeft = stageData.bottomLeft;
        topRight = stageData.topRight;
        // 필요한 경우 추가 초기화 로직을 여기에 추가합니다.
        NodeArray = StageManager.Instance.currentStage.NodeArray;
    }

    public Vector2Int WorldToTilemapPosition(Vector2 worldPos)   // 월드 좌표를 타일맵 좌표로 변환
    {
        int x = Mathf.RoundToInt((worldPos.x - bottomLeft.x) / 0.5f);
        int y = Mathf.RoundToInt((worldPos.y - bottomLeft.y) / 0.5f);

        return new Vector2Int(x, y);
    }

    public Vector2 TilemapToWorldPosition(Vector2Int tilemapPos)   // 타일맵 좌표를 월드 좌표로 변환
    {
        // 여기서 bottomLeft를 사용하여 올바른 월드 좌표를 계산합니다.
        return new Vector2(tilemapPos.x * 0.5f + bottomLeft.x, tilemapPos.y * 0.5f + bottomLeft.y);
    }

    public List<Node> PathFinding(Vector2Int startPos, Vector2Int targetPos)   // 시작 타일맵 좌표, 목표 타일맵 좌표
    {
        /* 초기화 부분은 StageManager에서 수행
        sizeX = Mathf.Abs(topRight.x - bottomLeft.x) * 2;  // 타일맵 가로 크기
        sizeY = Mathf.Abs(topRight.y - bottomLeft.y) * 2;  // 타일맵 세로 크기

        // Debug.Log("topRight " + topRight + "bottomLeft " + bottomLeft);

        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                Vector2 worldPosition = TilemapToWorldPosition(new Vector2Int(i, j));
                foreach (Collider2D col in Physics2D.OverlapCircleAll(worldPosition, 0.1f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i, j);
            }
        }
        */

        // Debug.Log("startPos + " + startPos.x + startPos.y);

        // 플레이어의 위치가 벽 타일인 경우 검사
        if (NodeArray[targetPos.x, targetPos.y].isWall)
        {
            Debug.Log("플레이어가 벽 타일에 있음");
            playerInWall = true;
            return new List<Node>(); // 빈 리스트 반환하여 빠르게 종료
        }
        else
        {
            playerInWall = false;
            TargetNode = NodeArray[targetPos.x, targetPos.y]; // 기존 목표 노드 설정
        }

        StartNode = NodeArray[startPos.x, startPos.y];   // 시작 노드
        TargetNode = NodeArray[targetPos.x, targetPos.y];   // 목표 노드

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList.Clear();

        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                // for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                return FinalNodeList;  // 반환값 수정
            }

            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }

        return FinalNodeList;  // 여기도 반환값 추가
    }

    void OpenListAdd(int checkX, int checkY)
    {
        Vector2Int tilemapBottomLeft = WorldToTilemapPosition(bottomLeft);
        Vector2Int tilemapTopRight = WorldToTilemapPosition(topRight);

        if (checkX >= tilemapBottomLeft.x && checkX < tilemapTopRight.x && checkY >= tilemapBottomLeft.y && checkY < tilemapTopRight.y)
        {
            Node checkingNode = NodeArray[checkX, checkY];
            if (!checkingNode.isWall && !ClosedList.Contains(checkingNode))
            {
                if (allowDiagonal) if (NodeArray[CurNode.x - tilemapBottomLeft.x, checkY - tilemapBottomLeft.y].isWall && NodeArray[checkX - tilemapBottomLeft.x, CurNode.y - tilemapBottomLeft.y].isWall) return;
                if (dontCrossCorner) if (NodeArray[CurNode.x - tilemapBottomLeft.x, checkY - tilemapBottomLeft.y].isWall || NodeArray[checkX - tilemapBottomLeft.x, CurNode.y - tilemapBottomLeft.y].isWall) return;

                int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

                if (MoveCost < checkingNode.G || !OpenList.Contains(checkingNode))
                {
                    checkingNode.G = MoveCost;
                    checkingNode.H = (Mathf.Abs(checkingNode.x - TargetNode.x) + Mathf.Abs(checkingNode.y - TargetNode.y)) * 10;
                    checkingNode.ParentNode = CurNode;

                    OpenList.Add(checkingNode);
                }
            }
        }
    }

    // 경로를 기즈모로 보여주는 메서드
    void OnDrawGizmos()
    {
        if (FinalNodeList != null && FinalNodeList.Count != 0)
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
            {
                Vector2 startPos = TilemapToWorldPosition(new Vector2Int(FinalNodeList[i].x, FinalNodeList[i].y));
                Vector2 endPos = TilemapToWorldPosition(new Vector2Int(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
                Gizmos.DrawLine(startPos, endPos);
            }
    }
}