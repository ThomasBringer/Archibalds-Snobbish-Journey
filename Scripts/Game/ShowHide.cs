using UnityEngine;
using UnityEngine.UI;

public class ShowHide : MonoBehaviour
{
    public bool menu;
    public bool game;
    public bool over;
    public bool intro;

    SpriteRenderer sprite;
    Text text;

    bool isSprite = true;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (sprite == null)
        {
            text = GetComponent<Text>();
            isSprite = false;
        }
    }

    public void EnableAll()
    {
        switch (GameManager.GameState)
        {
            case GameManager.State.Menu:
                Enable(menu);
                break;
            case GameManager.State.Game:
                Enable(game);
                break;
            case GameManager.State.Over:
                Enable(over);
                break;
            case GameManager.State.Intro:
                Enable(intro);
                break;
        }
    }

    void Enable(bool enable)
    {
        if (isSprite)
            sprite.enabled = enable;
        else
            text.enabled = enable;
    }
}