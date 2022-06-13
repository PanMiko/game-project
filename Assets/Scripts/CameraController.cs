using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform;

    public TextMeshProUGUI text;
    private float timer;

    void Awake()
    {
    }

    void Update()
    {
        timer += Time.deltaTime;
        var minutes = (int)timer / 60;
        var seconds = (int)timer % 60;
        if (seconds < 10)
        {
            var textSeconds = $"0{seconds}";
            text.text = $"{minutes}:{textSeconds}";
        }
        else
        {
            var textSeconds = seconds.ToString();
            text.text = $"{minutes}:{textSeconds}";
        }
        
    }

    void LateUpdate()
    {
        Vector3 temp = transform.position;

        temp.x = PlayerTransform.position.x;

        transform.position = temp;

    }
}
