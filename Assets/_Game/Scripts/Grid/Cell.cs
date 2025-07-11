using UnityEngine;

public class Cell : MonoBehaviour
{
    [Header("---References---")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
