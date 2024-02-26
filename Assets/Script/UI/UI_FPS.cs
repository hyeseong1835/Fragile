using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_FPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;
    public float update = 0.1f;
    public int display = 0;
    float time = 0;
    int frame = 0;
    private void Update()
    {
        time += Time.deltaTime;
        frame++;

        if (time >= update)
        {
            string str = ((1f / (time / frame))).ToString();
            int _display;
            
            if (display <= 0) _display = display;
            else _display = display + 1;
            if (str.Length > str.IndexOf('.') + _display) _display = str.IndexOf('.') + _display;
            else _display = str.Length;
            string fps = str.Substring(0, _display);
            fpsText.text = "FPS: " + fps;
            time = 0;
            frame = 0;
        }
    }
}
