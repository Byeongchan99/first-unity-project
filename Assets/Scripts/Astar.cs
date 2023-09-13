using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public class Astar : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList = new List<Node>();  // �ٷ� �ʱ�ȭ�Ͽ� null ���� ����
    public bool allowDiagonal, dontCrossCorner;

    int sizeX, sizeY;   // �� ũ��
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public Vector2Int WorldToTilemapPosition(Vector2 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x + 0.5f), Mathf.FloorToInt(worldPos.y + 0.5f));
    }

    public Vector2 TilemapToWorldPosition(Vector2Int tilemapPos)
    {
        return new Vector2(tilemapPos.x * 0.5f, tilemapPos.y * 0.5f);
    }

    public List<Node> PathFinding(Vector2Int start, Vector2Int target)
    {
        startPos = WorldToTilemapPosition(start);
        targetPos = WorldToTilemapPosition(target);
        sizeX = (topRight.x - bottomLeft.x) * 2 + 1;  // Ÿ�ϸ� ��ǥ�� ��ȯ
        sizeY = (topRight.y - bottomLeft.y) * 2 + 1;  // Ÿ�ϸ� ��ǥ�� ��ȯ

        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                Vector2 worldPosition = TilemapToWorldPosition(new Vector2Int(i, j));
                foreach (Collider2D col in Physics2D.OverlapCircleAll(worldPosition, 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i, j);
            }
        }

        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

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

                // for (int i = 0; i < FinalNodeList.Count; i++) print(i + "��°�� " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                return FinalNodeList;  // ��ȯ�� ����
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

        return FinalNodeList;  // ���⵵ ��ȯ�� �߰�
    }

    void OpenListAdd(int checkX, int checkY)
    {
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1)
        {
            Node checkingNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            if (!checkingNode.isWall && !ClosedList.Contains(checkingNode))
            {
                if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;
                if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

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