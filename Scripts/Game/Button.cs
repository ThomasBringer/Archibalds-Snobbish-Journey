using UnityEngine;

public class Button : MonoBehaviour
{
    public enum Buttons { Play, Info, Theme };
    public Buttons button;

    Color baseColor;
    public Color overColor;
    public string message;

    SpriteRenderer sprite;
    ThemeChanger themeChangerClass;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        baseColor = sprite.color;

        if (button == Buttons.Theme)
        {
            themeChangerClass = GetComponent<ThemeChanger>();
        }
    }

    public void ChangeColor(bool over)
    {
        sprite.color = over ? overColor : baseColor;
    }

    public void Press()
    {
        switch (button)
        {
            case Buttons.Play:
                GameManager.GameState = GameManager.State.Game;
                break;
            case Buttons.Theme:
                themeChangerClass.NextColor();
                break;
        }
    }
}