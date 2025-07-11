using UnityEngine;

public class Cell : MonoBehaviour
{
    private int x, y;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    private void OnMouseDown()
    {
        GameController.Instance.OnCellClicked(x, y);
    }

    public void SetSprite(Sprite s)
    {
        spriteRenderer.sprite = s;
    }
}
