using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class ContentObject : MonoBehaviour
{
    private Text text_Number;


    private void Awake()
    {
        text_Number = this.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        text_Number.text = canvasControl.instance.Get_objNumber().ToString();
    }

}
