using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [Header("---References---")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button goalBtn;
    [SerializeField] private Button roadBtn;
    [SerializeField] private Button wallBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(() => SelectPaint(CellType.Start));
        goalBtn.onClick.AddListener(() => SelectPaint(CellType.Goal));
        roadBtn.onClick.AddListener(() => SelectPaint(CellType.Road));
        wallBtn.onClick.AddListener(() => SelectPaint(CellType.Wall));

        SelectPaint(CellType.Road);
    }

    private void SelectPaint(CellType t)
    {
        GameController.Instance.SetCurrentPaintType(t);
    }
}
