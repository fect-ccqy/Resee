using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalUIPropWithMouseAnimation : NormalUIProp
{

    [SerializeField] private GameObject downClip;
    [SerializeField] private GameObject normalClip;


    protected override void BeChosen()
    {
        base.BeChosen();
        MouseManager.mouseManagerInstance.SetMouseController_AnimationCallBackMode(this);
    }

    protected override void CancelChosen()
    {
        base.CancelChosen();
        MouseManager.mouseManagerInstance.FreeControlOfMouse_AnimationCallBackMode();
    }


    public virtual GameObject GetDownClip()
    {
        return downClip;

    }

    public virtual GameObject GetNormalClip()
    {

        return normalClip;
    }






}
