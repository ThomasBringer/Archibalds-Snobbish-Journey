using System.Collections;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float downSmoothTime = .9f;
    public float upSmoothTime = 1;
    float smoothTime;
    public float rate = 10;

    public Transform down;
    public Transform superDown;
    public static bool goingDown;

    public Vector2 target;
    Vector2 currentVelocity;

    public string warningMessage;

    public GameObject careful;
    GameObject carefulInstance;

    public Player player;

    void Update()
    {
        transform.position = Vector2.SmoothDamp(transform.position, target, ref currentVelocity, smoothTime);

        switch (GameManager.GameState)
        {
            case GameManager.State.Menu:
                transform.position = Vector2.zero;
                target = Vector2.zero;
                player.Smile();
                break;
            case GameManager.State.Over:
                smoothTime = upSmoothTime;
                target = superDown.position;
                if (carefulInstance != null)
                {
                    StartCoroutine(RemoveCareful());
                }
                player.Smile();
                break;
        }
    }

    public void StartGame()
    {
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        player.Smile();
        transform.position = Vector2.zero;
        for (; ; )
        {
            target = Vector2.zero;
            smoothTime = upSmoothTime;
            yield return new WaitForSeconds(rate);
            if (GameManager.GameState != GameManager.State.Game)
            {
                break;
            }
            goingDown = true;
            target = down.position;
            smoothTime = downSmoothTime;

            player.Smile(2);

            GameManager.Write(warningMessage);
            carefulInstance = Instantiate(careful);
            while (goingDown)
            {
                yield return null;
            }
            StartCoroutine(RemoveCareful());
            StartCoroutine(player.Cry());
        }
    }

    IEnumerator RemoveCareful()
    {
        carefulInstance.GetComponent<Animator>().SetTrigger("Hide");
        yield return new WaitForSeconds(.5f);
        Destroy(carefulInstance);
    }
}