using UnityEngine;
using UnityEngine.UI;

public class ClearBtn : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() => GameController.Instance.ClearMap());
    }
}
