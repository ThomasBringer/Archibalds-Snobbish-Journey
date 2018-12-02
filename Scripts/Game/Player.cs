using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] smiles;

    public SpriteRenderer smile;

    public GameObject tear;

    public float cryTime = 5;

    public void Smile(int smileIndex = 0)
    {
        smile.sprite = smiles[smileIndex];
    }

    void Tear(bool enable)
    {
        tear.SetActive(enable);
    }

    public IEnumerator Cry()
    {
        Smile(1);
        Tear(true);

        yield return new WaitForSeconds(cryTime);

        Tear(false);
        Smile();
    }
}