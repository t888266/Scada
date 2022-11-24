using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingCircle : MonoBehaviour
{
    Image circle;
    float time;
    private void Awake()
    {
        circle = GetComponent<Image>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        float lerpValue = Mathf.Lerp(0, 1, time);
        if (time >= 1)
        {
            time = 0;
        }
        circle.fillAmount = lerpValue;
    }
}
