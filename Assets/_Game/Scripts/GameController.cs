using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    private float cellSize = 1f;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private bool animatePath = true;
    [SerializeField] private float pathAnimationDelay = 0.05f;

    private float wallSpawnChance = 0.2f;

    [Header("Sprites")]
    [SerializeField] private Sprite roadSprite;
    [SerializeField] private Sprite wallSprite;
    [SerializeField] private Sprite startSprite;
    [SerializeField] private Sprite goalSprite;
    [SerializeField] private Sprite pathSprite;

    [Header("---References---")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TextMeshProUGUI notFoundTxt;

    private void Awake()
    {
        notFoundTxt.gameObject.SetActive(false);

        GridModel model = new GridModel(this.width, this.height);

        IMapGenerator generator = new RandomMapGenerator(model, this.wallSpawnChance);
        generator.Generate();

        GridView view = new GridView(model, this.cellPrefab, this.cellSize, this.transform, this.roadSprite, 
            this.wallSprite, this.startSprite, this.goalSprite, this.pathSprite);

        float widthSize = width * cellSize;
        float heightSize = height * cellSize;
        cameraController.FitToGrid(widthSize, heightSize);

        IPathfinder pathfinder = new AstarPathfinder();
        List<Vector2Int> path = pathfinder.FindPath(model, model.StartPosition, model.GoalPosition);
        if(path == null)
        {
            notFoundTxt.gameObject.SetActive(true);
            return;
        }

        StartCoroutine(ShowPath(path, view));
    }

    private IEnumerator ShowPath(List<Vector2Int> path, GridView view)
    {
        if (animatePath)
        {
            yield return StartCoroutine(view.AnimatePath(path, pathAnimationDelay));
        }
        else
        {
            view.DrawPath(path);
        }
    }
}