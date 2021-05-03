using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAnimationController : MonoBehaviour
{


    public void AnimationFinish()
    {
        MouseManager.mouseManagerInstance.FinishDownAnimation_AnimationCallBackMode();
    }

}
