using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView
{
    private GridModel model;
    private Cell[,] cellViews;
    private float cellSize;
    private Vector3 originOffset;

    private Sprite roadSprite;
    private Sprite wallSprite;
    private Sprite startSprite;
    private Sprite goalSprite;
    private Sprite pathSprite;

    public GridView(GridModel model, GameObject cellPrefab, float cellSize, Transform parent, Sprite roadSprite,
        Sprite wallSprite, Sprite startSprite, Sprite goalSprite, Sprite pathSprite)
    {
        this.model = model;
        this.cellSize = cellSize;
        this.roadSprite = roadSprite;
        this.wallSprite = wallSprite;
        this.startSprite = startSprite;
        this.goalSprite = goalSprite;
        this.pathSprite = pathSprite;

        this.cellViews = new Cell[model.Width, model.Height];
        this.originOffset = new Vector3((model.Width * cellSize) / 2f - cellSize / 2f, (model.Height * cellSize) / 2f - cellSize / 2f, 0f);

        this.BuildView(cellPrefab, parent);
    }

    private void BuildView(GameObject prefab, Transform parent)
    {
        for (int x = 0; x < this.model.Width; x++)
        {
            for (int y = 0; y < this.model.Height; y++)
            {
                Vector3 pos = new Vector3(x * this.cellSize, y * this.cellSize, 0f) - this.originOffset;
                GameObject go = Object.Instantiate(prefab, pos, Quaternion.identity, parent);
                go.name = $"Cell {x} {y}";
                go.transform.localScale = Vector3.one * this.cellSize;

                Cell cellComp = go.GetComponent<Cell>();
                cellComp.SetSprite(this.GetSprite(this.model.GetCell(x, y)));
                this.cellViews[x, y] = cellComp;
            }
        }
    }

    private Sprite GetSprite(CellType type)
    {
        switch (type)
        {
            case CellType.Road: return this.roadSprite;
            case CellType.Wall: return this.wallSprite;
            case CellType.Start: return this.startSprite;
            case CellType.Goal: return this.goalSprite;
            case CellType.Path: return this.pathSprite;
            default: return null;
        }
    }

    public void DrawPath(List<Vector2Int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int p = path[i];

            if ((p.x == model.StartPosition.x && p.y == model.StartPosition.y) || (p.x == model.GoalPosition.x && p.y == model.GoalPosition.y))
            {
                continue;
            }

            this.model.SetCell(p.x, p.y, CellType.Path);
            this.cellViews[p.x, p.y].SetSprite(this.pathSprite);
        }
    }

    public IEnumerator AnimatePath(List<Vector2Int> path, float delay)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int p = path[i];

            if ((p == model.StartPosition) || (p == model.GoalPosition))
            {
                continue;
            }

            model.SetCell(p.x, p.y, CellType.Path);
            cellViews[p.x, p.y].SetSprite(this.pathSprite);

            yield return new WaitForSeconds(delay);
        }
    }

}
