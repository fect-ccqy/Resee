using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalUIPropWithMouseAnimation : NormalUIProp
{
    //按下动画与正常状态的动画
    [SerializeField] private GameObject downClip;
    [SerializeField] private GameObject normalClip;


    //被选中后，先调用父类选中方法，然后设置鼠标
    protected override void BeChosen()
    {
        base.BeChosen();
        MouseManager.mouseManagerInstance.SetMouseController_AnimationCallBackMode(this);
    }


    //取消选中后，先调用父类选中方法，然后设置鼠标
    protected override void CancelChosen()
    {
        base.CancelChosen();
        MouseManager.mouseManagerInstance.FreeControlOfMouse_AnimationCallBackMode();
    }

    //由mouseManager调用该方法。取消选中后，先调用父类取消选中方法，然后设置鼠标
    public virtual GameObject GetDownClip()
    {
        return downClip;

    }

    //由mouseManager调用该方法。将鼠标正常状态的动画obj传给MouseManager
    public virtual GameObject GetNormalClip()
    {

        return normalClip;
    }






}
