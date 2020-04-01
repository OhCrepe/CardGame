using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{

    public Animator anim;
    private Text textComponent;

    public void OnEnable(){

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        textComponent = anim.GetComponent<Text>();
    }

    public void SetText(string text, Color color){
        textComponent.text = text;
        textComponent.color = color;
    }

}
