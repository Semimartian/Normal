using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : Button
{
    private bool isPressed;
    private SubmitButtonHelper helper;
    // private Button button;


    // Start is called before the first frame update
    protected override void Start()
    {
        helper = GetComponent<SubmitButtonHelper>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPressed && this.IsPressed())
        {
            helper.image.sprite = helper.pressedTexture;
            
        }
        else if (isPressed && !this.IsPressed())
        {
            helper.image.sprite = helper.unpressedTexture;
        }
        isPressed = this.IsPressed();
    }
}
