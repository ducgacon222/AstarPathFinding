using System.Collections.Generic;
using UnityEngine;

public class AstarPathfinder : IPathfinder
{
    private class Node
    {
        public int X, Y;
        public bool Walkable;
        public int G, H;
        public int F { get { return G + H; } }
        public Node Parent;
    }

    public List<Vector2Int> FindPath(GridModel model, Vector2Int start, Vector2Int goal)
    {
        int w = model.Width;
        int h = model.Height;
        Node[,] nodes = new Node[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                nodes[x, y] = new Node()
                {
                    X = x,
                    Y = y,
                    Walkable = (model.GetCell(x, y) != CellType.Wall)
                };
            }
        }

        Node startNode = nodes[start.x, start.y];
        Node goalNode = nodes[goal.x, goal.y];

        List<Node> openList = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        startNode.G = 0;
        startNode.H = Heuristic(startNode, goalNode);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node current = GetLowestF(openList);
            if (current == goalNode)
            {
                return Retrace(startNode, goalNode);
            }

            openList.Remove(current);
            closed.Add(current);

            foreach (Vector2Int dir in new[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
            {
                int nx = current.X + dir.x;
                int ny = current.Y + dir.y;

                if (nx < 0 || nx >= w || ny < 0 || ny >= h) continue;

                Node neighbor = nodes[nx, ny];
                if (!neighbor.Walkable || closed.Contains(neighbor)) continue;

                int tentativeG = current.G + 1;
                if (tentativeG < neighbor.G || !openList.Contains(neighbor))
                {
                    neighbor.G = tentativeG;
                    neighbor.H = Heuristic(neighbor, goalNode);
                    neighbor.Parent = current;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null; // No path found
    }

    private int Heuristic(Node a, Node b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }

    private Node GetLowestF(List<Node> list)
    {
        Node best = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].F < best.F)
            {
                best = list[i];
            }
        }
        return best;
    }

    private List<Vector2Int> Retrace(Node start, Node end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node current = end;
        while (current != start)
        {
            path.Add(new Vector2Int(current.X, current.Y));
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }
}