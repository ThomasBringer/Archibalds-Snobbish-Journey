using System.Collections;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject Tree;

    public Transform minSpawnPosition;
    public Transform maxSpawnPosition;
    public float rate;

    void Awake()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (; ; )
        {
            Instantiate(Tree, RandomVector2(minSpawnPosition.position, maxSpawnPosition.position), Quaternion.identity);
            yield return new WaitForSeconds(rate);
        }
    }

    Vector2 RandomVector2(Vector2 min, Vector2 max)
    {
        return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }
}