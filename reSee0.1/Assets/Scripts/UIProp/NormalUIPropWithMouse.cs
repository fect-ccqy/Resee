using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalUIPropWithMouse : NormalUIProp
{

    //一些设置

    //正常状态，落在物体上以及按下时的素材
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite overSprite;
    [SerializeField] private Sprite downSprite;


    //是否设置素材
    [SerializeField] private bool isSetNormalSprite;
    [SerializeField] private bool isSetOverSprite;
    [SerializeField] private bool isSetDownSprite;

    //是否交由mouseManager设置按下，正常与over状态下鼠标图标的颜色
    [SerializeField] private bool isMouseManagerSetNormalColor;
    [SerializeField] private bool isMouseManagerSetOverColor;
    [SerializeField] private bool isMouseManagerSetDownColor;



    //被选中后，先调用父类选中方法，然后设置鼠标
    protected override void BeChosen()
    {
        base.BeChosen();
        MouseManager.mouseManagerInstance.SetMouseController_DefaultCallBackMode(this,isMouseManagerSetNormalColor,isMouseManagerSetOverColor,isMouseManagerSetDownColor);
    }

    //取消选中后，先调用父类取消选中方法，然后设置鼠标
    protected override void CancelChosen()
    {
        base.CancelChosen();
        MouseManager.mouseManagerInstance.FreeControlOfMouse_DefaultCallBackMode();
    }


    //由mouseManager调用该方法。将鼠标设置为down状态
    public virtual void TheMouseDown(GameObject theMouse, Transform theMouseTransform, SpriteRenderer theMouseSpriteRenderer)
    {
        if (isSetDownSprite)
        {
            theMouseSpriteRenderer.sprite = downSprite;
        }
    }


    //由mouseManager调用该方法。将鼠标设置为down状态
    public virtual void TheMouseOverObj(GameObject theMouse, Transform theMouseTransform,SpriteRenderer theMouseSpriteRenderer)
    {
        if (isSetOverSprite)
        {
            theMouseSpriteRenderer.sprite = overSprite;
        }


    }

    //由mouseManager调用该方法。将鼠标设置为down状态
    public virtual void TheMouseToNormal(GameObject theMouse, Transform theMouseTransform, SpriteRenderer theMouseSpriteRenderer)
    {
        if (isSetNormalSprite)
        {
            theMouseSpriteRenderer.sprite = normalSprite;
        }


    }

}
