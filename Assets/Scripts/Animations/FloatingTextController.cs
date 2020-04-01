using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{

    private static FloatingText floatingText;
    private static GameObject canvas;

    // Start is called before the first frame update
    public static void Initialize()
    {

        floatingText = Resources.Load<FloatingText>("Prefab/FloatingTextParent");
        if(canvas == null)
            canvas = GameObject.Find("FloatingTextCanvas");
    }

    // Update is called once per frame
    public static void CreateFloatingText(string text, Color color, Transform location)
    {
        FloatingText textObject = Instantiate(floatingText);
        Vector2 position = Camera.main.WorldToScreenPoint(location.position);
        textObject.transform.SetParent(canvas.transform, false);
        textObject.transform.position = position;
        textObject.SetText(text, color);
    }
}
