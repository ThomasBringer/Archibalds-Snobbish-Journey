using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager gameManagerClass;

    public enum State { Menu, Game, Over, Intro };
    static State gameState = State.Menu;
    public static State GameState
    {
        get { return gameState; }
        set
        {
            if (value == State.Game)
            {
                gameManagerClass.StartGame();
            }
            else
            {
                gameState = value;
                gameManagerClass.buttons.SetActive(true);
            }
            gameManagerClass.EnableAll();
        }
    }

    public enum Sources { Heart, Food, Coin, Tool };
    public Text console;

    GameObject underMouse;
    bool over;
    bool canWrite = true;

    public GameObject items;
    GameObject itemsInstance;
    public Balloon balloonClass;

    public string warningMessage;
    public float warningMessageTime;

    public static Color theme;
    public static Dictionary<Sources, float> allSources = new Dictionary<Sources, float>()
    {
        { Sources.Heart, 1 },
        { Sources.Food, 1 },
        { Sources.Coin, 1 },
        { Sources.Tool, 1 },
    };

    public string startMessage;

    public string dieMessage;
    public string dieHeartMessage;
    public string dieFoodMessage;
    public string dieCoinMessage;
    public string dieToolMessage;

    public GameObject buttons;

    ShowHide[] showHides;

    int score;
    int Score
    {
        get { return score; }
        set { score = value; scoreText.text = score > 1 ? score + " objects thrown" : score + " object thrown"; }
    }
    int highscore;

    public Text scoreText;

    Collider2D cldr;

    public string[] introTexts;

    void Awake()
    {
        gameManagerClass = this;
        cldr = GetComponent<Collider2D>();

        showHides = FindObjectsOfType<ShowHide>();

        EnableAll();

        highscore = PlayerPrefs.GetInt("high");
    }

    void EnableAll()
    {
        foreach (var showHide in showHides)
        {
            showHide.EnableAll();
        }
    }

    IEnumerator Intro()
    {
        GameState = State.Intro;
        canWrite = false;
        yield return null;
        foreach (var info in introTexts)
        {
            Write(info, true);
            while (!(MousePressed(true) || Input.GetKeyDown(KeyCode.Space)))
                yield return null;
            yield return null;
        }

        PlayerPrefs.SetInt("intro", 1);
        canWrite = true;
        StartGame();
    }

    void StartGame()
    {
        if (PlayerPrefs.GetInt("intro", 0) == 0)
        {
            StartCoroutine(Intro());
            return;
        }
        gameState = State.Game;
        EnableAll();

        cldr.enabled = true;

        Score = 0;

        buttons.SetActive(false);

        ChangeAllSources(0);
        balloonClass.StartGame();

        Write(startMessage);

        if (itemsInstance != null)
            Destroy(itemsInstance);
        itemsInstance = Instantiate(items, balloonClass.transform);
    }

    public static void ChangeAllSources(float value, bool relative = false)
    {
        ChangeSource(Sources.Heart, value, relative);
        ChangeSource(Sources.Food, value, relative);
        ChangeSource(Sources.Coin, value, relative);
        ChangeSource(Sources.Tool, value, relative);
    }

    public static void ChangeSource(Sources source, float value, bool relative = false)
    {
        allSources[source] = relative ? allSources[source] + value : value;
    }

    public static float GetSource(Sources source)
    {
        return allSources[source];
    }

    public static void Write(string message, bool force = false)
    {
        if (gameManagerClass.canWrite || force)
        {
            gameManagerClass.console.text = message;
        }
    }

    IEnumerator Write(string message, float time)
    {
        console.text = message;
        canWrite = false;
        yield return new WaitForSeconds(time);
        canWrite = true;
    }

    void Update()
    {
        if (!over)
        {
            underMouse = GameObjectUnderMouse();

            if (underMouse != null)
            {
                var item = underMouse.GetComponent<Item>();
                if (item != null)
                {
                    if (MousePressed(true))
                    {
                        if (Balloon.goingDown)
                            StartCoroutine(Drag(item));
                        else
                            StartCoroutine(Write(warningMessage, warningMessageTime));
                    }
                    else
                        StartCoroutine(Over(item));
                }
                else
                {
                    var button = underMouse.GetComponent<Button>();
                    if (button != null)
                    {
                        if (MousePressed(true))
                            button.Press();
                        else
                            StartCoroutine(Over(button));
                    }
                }
            }
        }

        CheckGameOver();
    }

    GameObject GameObjectUnderMouse()
    {
        Collider2D collider = Physics2D.Raycast(MousePos(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Default")).collider;
        return (collider == null ? null : collider.gameObject);
    }

    bool MousePressed(bool keyDown = false)
    {
        return keyDown ? Input.GetKeyDown(KeyCode.Mouse0) : Input.GetKey(KeyCode.Mouse0);
    }

    Vector2 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    IEnumerator Drag(Item item)
    {
        item.ChangeColor(Item.DifferentColors.Drag);
        item.SetRbToKinematic(true);
        Vector2 mouseOffset = (Vector2)item.transform.position - MousePos();
        over = true;
        while (MousePressed())
        {
            yield return null;
            item.transform.position = MousePos() + mouseOffset;
            if (!Balloon.goingDown)
                break;
        }
        over = false;
        item.ChangeColor(Item.DifferentColors.Base);
        item.SetRbToKinematic(false);
    }

    IEnumerator Over(Item item)
    {
        item.ChangeColor(Item.DifferentColors.Over);
        Write(item.itemName);
        yield return new WaitForEndOfFrame();
        item.ChangeColor(Item.DifferentColors.Base);
    }

    IEnumerator Over(Button button)
    {
        button.ChangeColor(true);
        Write(button.message);
        yield return new WaitForEndOfFrame();
        button.ChangeColor(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Basket"))
        {
            GameOver();
            return;
        }

        var item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            ChangeSource(Sources.Heart, item.sources[0], true);
            ChangeSource(Sources.Food, item.sources[1], true);
            ChangeSource(Sources.Coin, item.sources[2], true);
            ChangeSource(Sources.Tool, item.sources[3], true);

            Balloon.goingDown = false;
            Score++;
            Write(item.message);
        }

        Destroy(collision.gameObject);
    }

    void CheckGameOver()
    {
        if (GameState == State.Game)
        {
            CheckSource(Sources.Heart);
            CheckSource(Sources.Food);
            CheckSource(Sources.Coin);
            CheckSource(Sources.Tool);
        }
    }

    void CheckSource(Sources source)
    {
        if (GetSource(source) >= 1)
        {
            GameOver(source);
        }
    }

    void GameOver(Sources thisSource)
    {
        switch (thisSource)
        {
            case Sources.Heart:
                Write(dieHeartMessage);
                break;
            case Sources.Food:
                Write(dieFoodMessage);
                break;
            case Sources.Coin:
                Write(dieCoinMessage);
                break;
            case Sources.Tool:
                Write(dieToolMessage);
                break;
        }
        EndGame();
    }

    void GameOver()
    {
        Write(dieMessage);
        EndGame();
    }

    void EndGame()
    {
        GameState = State.Over;
        cldr.enabled = false;

        if (Score > highscore)
        {
            highscore = Score;
            scoreText.text = scoreText.text = Score > 1 ? Score + " objects thrown\nNew highscore!" : score + " object thrown\nNew highscore!";
            PlayerPrefs.SetInt("high", highscore);
        }
        else
        {
            scoreText.text = scoreText.text = Score > 1 ? Score + " objects thrown\nHighscore: " + highscore : Score + " object thrown\nHighscore: " + highscore;
        }
    }
}