using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    public List<Vector2Int> FindPath(int[,] map, Vector2Int start, Vector2Int target)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        List<Vector2Int> openSet = new List<Vector2Int> { start }; // �������� �����
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>(); // �������� �����

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

        gScore[start] = 0;
        fScore[start] = Heuristic(start, target);

        while (openSet.Count > 0)
        {
            Vector2Int current = GetLowestFScoreNode(openSet, fScore);

            if (current == target)
            {
                return ReconstructPath(cameFrom, current); // ���������� ����
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current, map))
            {
                if (closedSet.Contains(neighbor) || !IsWalkable(map, neighbor)) continue;

                float tentativeGScore = gScore[current] + 1; // ��������������� ���������

                if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                else if (tentativeGScore >= (gScore.ContainsKey(neighbor) ? gScore[neighbor] : float.MaxValue)) continue;

                // ���������� ����� ���������
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, target);
            }
        }

        return new List<Vector2Int>(); // ���� ���� �� ������
    }

    // ��������� ���� � ���������� ��������� f
    private Vector2Int GetLowestFScoreNode(List<Vector2Int> openSet, Dictionary<Vector2Int, float> fScore)
    {
        Vector2Int lowest = openSet[0];
        foreach (Vector2Int node in openSet)
        {
            if (fScore.ContainsKey(node) && fScore[node] < fScore[lowest])
                lowest = node;
        }
        return lowest;
    }

    // ������������� �������
    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Vector2Int.Distance(a, b); // ��������� ����������
    }

    // ��������� �������
    private List<Vector2Int> GetNeighbors(Vector2Int node, int[,] map)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = {
            new Vector2Int(0, 1),   // �����
            new Vector2Int(0, -1),  // ����
            new Vector2Int(-1, 0),  // �����
            new Vector2Int(1, 0)    // ������
        };

        foreach (var direction in directions)
        {
            Vector2Int neighbor = node + direction;
            if (IsWalkable(map, neighbor))
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    // �������� ������������
    private bool IsWalkable(int[,] map, Vector2Int position)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        return position.x >= 0 && position.x < width &&
               position.y >= 0 && position.y < height &&
               map[position.x, position.y] == 1;
    }

    // �������������� ����
    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> totalPath = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse(); // ���� �� ������ �� �����
        return totalPath;
    }
}
