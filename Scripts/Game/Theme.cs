using UnityEngine;

public class Theme : MonoBehaviour
{
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sprite.color = GameManager.theme;
    }
}