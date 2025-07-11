using UnityEngine;
using static GameController;

public class GridModel
{
    private CellType[,] cells;

    public int Width { get; }
    public int Height { get; }

    public Vector2Int StartPosition { get; private set; }
    public Vector2Int GoalPosition { get; private set; }

    public GridModel(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.cells = new CellType[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                this.cells[x, y] = CellType.Road;
            }
        }
    }

    public CellType GetCell(int x, int y)
    {
        return this.cells[x, y];
    }

    public void SetCell(int x, int y, CellType type)
    {
        this.cells[x, y] = type;
    }

    public void SetStart(int x, int y)
    {
        this.StartPosition = new Vector2Int(x, y);
        this.SetCell(x, y, CellType.Start);
    }

    public void SetGoal(int x, int y)
    {
        this.GoalPosition = new Vector2Int(x, y);
        this.SetCell(x, y, CellType.Goal);
    }
}

public enum CellType
{
    Road = 0,
    Wall = 1,
    Start = 2,
    Goal = 3,
    Path = 4
}
