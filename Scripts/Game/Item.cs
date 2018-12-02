using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string message;

    public float[] sources;
    public Dictionary<GameManager.Sources, float> allSources = new Dictionary<GameManager.Sources, float>();

    public enum DifferentColors { Base, Over, Drag };

    Color baseColor;
    public Color overColor;
    public Color dragColor;

    SpriteRenderer sprite;
    Rigidbody2D rb;
    Vector2 mouseOffset;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        baseColor = sprite.color;
    }

    void OnMouseExit()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        ChangeColor(DifferentColors.Base);
    }

    void OnMouseDrag()
    {
        transform.position = (Vector2)Input.mousePosition + mouseOffset;
    }

    void OnMouseDown()
    {
        ChangeColor(DifferentColors.Drag);
        rb.bodyType = RigidbodyType2D.Kinematic;
        mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseOver()
    {
        print("mouseover");

        ChangeColor(DifferentColors.Over);
    }

    public void ChangeColor(DifferentColors color)
    {
        switch (color)
        {
            case DifferentColors.Base:
                sprite.color = baseColor;
                break;
            case DifferentColors.Over:
                sprite.color = overColor;
                break;
            case DifferentColors.Drag:
                sprite.color = dragColor;
                break;
        }
    }

    public void SetRbToKinematic(bool kinematic)
    {
        rb.bodyType = kinematic ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }
}