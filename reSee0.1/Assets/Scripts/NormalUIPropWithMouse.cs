using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalUIPropWithMouse : NormalUIProp
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite overSprite;
    [SerializeField] private Sprite downSprite;

    [SerializeField] private bool isSetNormalSprite;
    [SerializeField] private bool isSetOverSprite;
    [SerializeField] private bool isSetDownSprite;

    [SerializeField] private bool isMouseManagerSetNormalColor;
    [SerializeField] private bool isMouseManagerSetOverColor;
    [SerializeField] private bool isMouseManagerSetDownColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected override void BeChosen()
    {
        base.BeChosen();
        MouseManager.mouseManagerInstance.SetMouseController_DefaultCallBackMode(this,isMouseManagerSetNormalColor,isMouseManagerSetOverColor,isMouseManagerSetDownColor);
    }

    protected override void CancelChosen()
    {
        base.CancelChosen();
        MouseManager.mouseManagerInstance.FreeControlOfMouse_DefaultCallBackMode();
    }


    public virtual void TheMouseDown(GameObject theMouse, Transform theMouseTransform, SpriteRenderer theMouseSpriteRenderer)
    {
        if (isSetDownSprite)
        {
            theMouseSpriteRenderer.sprite = downSprite;
        }
    }

    public virtual void TheMouseOverObj(GameObject theMouse, Transform theMouseTransform,SpriteRenderer theMouseSpriteRenderer)
    {
        if (isSetOverSprite)
        {
            theMouseSpriteRenderer.sprite = overSprite;
        }


    }
    public virtual void TheMouseToNormal(GameObject theMouse, Transform theMouseTransform, SpriteRenderer theMouseSpriteRenderer)
    {
        if (isSetNormalSprite)
        {
            theMouseSpriteRenderer.sprite = normalSprite;
        }


    }

}
