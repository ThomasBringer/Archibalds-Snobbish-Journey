using UnityEngine;

public class Tree : MonoBehaviour
{
    public Vector2 speed;
    public float maxPos = 25;
    public Color[] colors;

    public SpriteRenderer[] sprites;

    void Awake()
    {
        Color color = colors[Random.Range(0, colors.Length)];
        foreach (var sprite in sprites)
        {
            sprite.color = color;
        }
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime);
        if (Mathf.Abs(transform.position.x) >= maxPos)
        {
            Destroy(gameObject);
        }
    }
}