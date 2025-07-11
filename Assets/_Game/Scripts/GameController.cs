using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    private float cellSize = 1f;
    private float wallSpawnChance = 0.2f;

    [Header("Animation Settings")]
    [SerializeField] private float pathAnimationDelay = 0.05f;

    [Header("Sprites")]
    [SerializeField] private Sprite roadSprite;
    [SerializeField] private Sprite wallSprite;
    [SerializeField] private Sprite startSprite;
    [SerializeField] private Sprite goalSprite;
    [SerializeField] private Sprite pathSprite;

    [Header("---References---")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TextMeshProUGUI notFoundTxt;

    private GridModel model;
    private GridView view;
    private IPathfinder pathfinder;
    private CellType currentPaintType = CellType.Road;
    private Coroutine pathCoroutine;

    public int Width => width;
    public int Height => height;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        notFoundTxt.gameObject.SetActive(false);
        pathfinder = new AstarPathfinder();

        RandomizeMap();
    }

    public void SetSize(int w, int h)
    {
        width = Mathf.Max(1, w);
        height = Mathf.Max(1, h);
    }

    public void RandomizeMap()
    {
        model = new GridModel(width, height);
        RandomMapGenerator gen = new RandomMapGenerator(model, wallSpawnChance);
        gen.Generate();

        RebuildView();
        DrawPath();
    }

    public void ClearMap()
    {
        RandomizeMap();

        Vector2Int sp = model.StartPosition;
        Vector2Int gp = model.GoalPosition;

        model = new GridModel(width, height);
        model.SetStart(sp.x, sp.y);
        model.SetGoal(gp.x, gp.y);

        RebuildView();
        DrawPath();
    }

    public void SetCurrentPaintType(CellType t)
    {
        currentPaintType = t;
    }

    public void OnCellClicked(int x, int y)
    {
        CellType oldType = model.GetCell(x, y);

        switch (currentPaintType)
        {
            case CellType.Start:
                if (oldType == CellType.Goal)
                {
                    SwapStartGoal();
                }
                else if (oldType != CellType.Start)
                {
                    Vector2Int op = model.StartPosition;
                    model.SetCell(op.x, op.y, CellType.Road);
                    model.SetStart(x, y);
                }
                break;

            case CellType.Goal:
                if (oldType == CellType.Start)
                {
                    SwapStartGoal();
                }
                else if (oldType != CellType.Goal)
                {
                    var gp = model.GoalPosition;
                    model.SetCell(gp.x, gp.y, CellType.Road);
                    model.SetGoal(x, y);
                }
                break;

            case CellType.Road:
                if (oldType == CellType.Start || oldType == CellType.Goal)
                {
                    SwapStartGoal();
                }
                else
                {
                    model.SetCell(x, y, CellType.Road);
                }
                break;

            case CellType.Wall:
                if (oldType == CellType.Start || oldType == CellType.Goal)
                {
                    SwapStartGoal();
                }
                model.SetCell(x, y, CellType.Wall);
                break;
        }

        RebuildView();
        DrawPath();
    }

    private void RebuildView()
    {
        for (int xx = 0; xx < width; xx++)
        {
            for (int yy = 0; yy < height; yy++)
            {
                if (model.GetCell(xx, yy) == CellType.Path)
                {
                    model.SetCell(xx, yy, CellType.Road);
                }
            }           
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        view = new GridView(model, cellPrefab, cellSize, this.transform, roadSprite, wallSprite, startSprite, goalSprite, pathSprite);

        float worldW = width * cellSize;
        float worldH = height * cellSize;
        cameraController.FitToGrid(worldW, worldH, true);
    }

    private void DrawPath()
    {
        notFoundTxt.gameObject.SetActive(false);

        List<Vector2Int> path = pathfinder.FindPath(model, model.StartPosition, model.GoalPosition);
        if (path == null)
        {
            if (pathCoroutine != null) StopCoroutine(pathCoroutine);
            notFoundTxt.gameObject.SetActive(true);
            return;
        }

        if (pathCoroutine != null) StopCoroutine(pathCoroutine);
        pathCoroutine = StartCoroutine(view.AnimatePath(path, pathAnimationDelay));
    }

    private void SwapStartGoal()
    {
        Vector2Int s = model.StartPosition;
        Vector2Int g = model.GoalPosition;
        model.SetStart(g.x, g.y);
        model.SetGoal(s.x, s.y);
    }
}
