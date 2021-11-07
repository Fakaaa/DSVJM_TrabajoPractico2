using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RithmAnimation : MonoBehaviour
{
    [SerializeField] float animSpeed;
    [SerializeField] float maxScaleReach;
    float t;

    Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (t < animSpeed)
        {
            if(transform.localScale.x < maxScaleReach)
                transform.localScale += new Vector3(t * 0.2f, t * 0.2f, t * 0.2f);
            t += Time.deltaTime;
        }
        else
        {
            transform.localScale = originalScale;
            t = 0;
        }
    }
}
