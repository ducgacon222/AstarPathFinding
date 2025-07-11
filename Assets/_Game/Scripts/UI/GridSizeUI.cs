using TMPro;
using UnityEngine;

public class GridSizeUI : MonoBehaviour
{
    [Header("---References---")]
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;

    private void Start()
    {
        widthInput.text = GameController.Instance.Width.ToString();
        heightInput.text = GameController.Instance.Height.ToString();

        widthInput.onEndEdit.AddListener(str =>
        {
            if (int.TryParse(str, out var w) && w < 0)
            {
                widthInput.text = "1";
            }

            GameController.Instance.SetSize(w, GameController.Instance.Height);
            widthInput.text = GameController.Instance.Width.ToString();
        });
        heightInput.onEndEdit.AddListener(str =>
        {
            if (int.TryParse(str, out var h) && h < 0)
            {
                heightInput.text = "1";
            }

            GameController.Instance.SetSize(GameController.Instance.Width, h);
            heightInput.text = GameController.Instance.Height.ToString();
        });
    }
}
