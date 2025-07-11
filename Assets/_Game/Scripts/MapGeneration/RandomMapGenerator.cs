using UnityEngine;

public class RandomMapGenerator : IMapGenerator
{
    private GridModel model;
    private float wallChance;

    public RandomMapGenerator(GridModel model, float wallSpawnChance)
    {
        this.model = model;
        this.wallChance = wallSpawnChance;
    }

    public void Generate()
    {
        for (int x = 0; x < this.model.Width; x++)
        {
            for (int y = 0; y < this.model.Height; y++)
            {
                CellType type;
                if (Random.value < this.wallChance)
                {
                    type = CellType.Wall;
                }
                else
                {
                    type = CellType.Road;
                }

                this.model.SetCell(x, y, type);
            }
        }

        Vector2Int start = this.GetRandomRoadCell();
        this.model.SetStart(start.x, start.y);

        Vector2Int goal;
        do
        {
            goal = this.GetRandomRoadCell();
        }
        while (goal == start);

        this.model.SetGoal(goal.x, goal.y);
    }

    private Vector2Int GetRandomRoadCell()
    {
        while (true)
        {
            int x = Random.Range(0, this.model.Width);
            int y = Random.Range(0, this.model.Height);

            if (this.model.GetCell(x, y) == CellType.Road)
            {
                return new Vector2Int(x, y);
            }
        }
    }
}