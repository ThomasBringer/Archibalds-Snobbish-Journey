using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameManager.Sources source;

    Vector2 currentVelocity;
    public float smoothTime;

    Vector2 TargetScale
    {
        get { return Vector2.one * Mathf.Clamp((1 - GameManager.GetSource(source)) * 2, 0, 2); }
    }

    void LateUpdate()
    {
        transform.localScale = Vector2.SmoothDamp(transform.localScale, TargetScale, ref currentVelocity, smoothTime);
    }
}