using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    private Vector2 camMov;
    public float smoothing = 3f;
    public Transform player;
    Vector3 offset;


    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 targetCamPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            transform.position = new Vector3(Random.Range(-0.5f, 0.5f) + transform.position.x, transform.position.y, Random.Range(-0.5f, 0.5f) + transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
