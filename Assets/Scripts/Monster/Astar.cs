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
    public Vector2Int bottomLeft, topRight;   // ���� �ϴ� ������ ��� ������ ���� ��ǥ
    public List<Node> FinalNodeList = new List<Node>();  // �ٷ� �ʱ�ȭ�Ͽ� null ���� ����
    public bool allowDiagonal, dontCrossCorner;
    public bool playerInWall = false;   // �÷��̾ Wall Ÿ�� ��ó�� ���� ��

    int sizeX, sizeY;   // �� ũ��
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public void Initialize(StageData stageData)
    {
        // Debug.Log("�������� ����" + stageData.stageID);
        bottomLeft = stageData.bottomLeft;
        topRight = stageData.topRight;
        // �ʿ��� ��� �߰� �ʱ�ȭ ������ ���⿡ �߰��մϴ�.
        NodeArray = StageManager.Instance.currentStage.NodeArray;
    }

    public Vector2Int WorldToTilemapPosition(Vector2 worldPos)   // ���� ��ǥ�� Ÿ�ϸ� ��ǥ�� ��ȯ
    {
        int x = Mathf.RoundToInt((worldPos.x - bottomLeft.x) / 0.5f);
        int y = Mathf.RoundToInt((worldPos.y - bottomLeft.y) / 0.5f);

        return new Vector2Int(x, y);
    }

    public Vector2 TilemapToWorldPosition(Vector2Int tilemapPos)   // Ÿ�ϸ� ��ǥ�� ���� ��ǥ�� ��ȯ
    {
        // ���⼭ bottomLeft�� ����Ͽ� �ùٸ� ���� ��ǥ�� ����մϴ�.
        return new Vector2(tilemapPos.x * 0.5f + bottomLeft.x, tilemapPos.y * 0.5f + bottomLeft.y);
    }

    public List<Node> PathFinding(Vector2Int startPos, Vector2Int targetPos)   // ���� Ÿ�ϸ� ��ǥ, ��ǥ Ÿ�ϸ� ��ǥ
    {
        /* �ʱ�ȭ �κ��� StageManager���� ����
        sizeX = Mathf.Abs(topRight.x - bottomLeft.x) * 2;  // Ÿ�ϸ� ���� ũ��
        sizeY = Mathf.Abs(topRight.y - bottomLeft.y) * 2;  // Ÿ�ϸ� ���� ũ��

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

        // �÷��̾��� ��ġ�� �� Ÿ���� ��� �˻�
        if (NodeArray[targetPos.x, targetPos.y].isWall)
        {
            Debug.Log("�÷��̾ �� Ÿ�Ͽ� ����");
            playerInWall = true;
            return new List<Node>(); // �� ����Ʈ ��ȯ�Ͽ� ������ ����
        }
        else
        {
            playerInWall = false;
            TargetNode = NodeArray[targetPos.x, targetPos.y]; // ���� ��ǥ ��� ����
        }

        StartNode = NodeArray[startPos.x, startPos.y];   // ���� ���
        TargetNode = NodeArray[targetPos.x, targetPos.y];   // ��ǥ ���

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

    // ��θ� ������ �����ִ� �޼���
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