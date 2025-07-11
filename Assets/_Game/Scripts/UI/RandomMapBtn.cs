using UnityEngine;
using UnityEngine.UI;

public class RandomMapBtn : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() => GameController.Instance.RandomizeMap());
    }
}
