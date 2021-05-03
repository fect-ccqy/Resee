using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    public enum MouseState
    {
        DefaultState,//默认状态
        ControlByObj,//选中物品后，按下，点击等受控于其他道具，检测到点击/抬起后调用其他物体的方法
        //ControlByObj_AnimationCallBackMode,//选中物品后，按下，受控于其他道具，检测到点击后调用其他物体的方法播放动画，动画播放结束后调用MouseManager中的AniamtionPlayFinish方法。
        Replaced//现有框架无法实现特殊需求时，将鼠标调为该状态，由其他物体自行实现的鼠标替代该鼠标。（该功能在当前项目暂时用不上，就先没实现，只留了接口）

    }


    public enum MouseCallBackMode
    {
        defaultCallBackMode,
        AnimationCallBackMode,
        Replaced
    }


    public static MouseManager mouseManagerInstance;
    

    [SerializeField] private GameObject theMouse;//默认鼠标实体
    private Transform theMouseTransform;//默认鼠标实体的transfrom
    private Vector3 mouseWorldPosition;//暂时变量，因为需要频繁的创建销毁，就放到这里来了
    private SpriteRenderer theMouseSpriteRenderer;//鼠标实体的spriteRenderer

    //一些默认设置
    [SerializeField] private Sprite defaultMouseSprite;
    [SerializeField] private Color overColor;
    [SerializeField] private Color downColor;
    [SerializeField] private Color normalColor;
    private Color zeroColor=new Color(1f,1f,1f,0f);//隐藏默认鼠标时的透明颜色



    private MouseState mouseState = MouseState.DefaultState;
    private MouseCallBackMode mouseCallBackMode = MouseCallBackMode.defaultCallBackMode;



    private bool isMouseOverOnObj;
    private bool isMouseDown;

    //defaultCallBackMode              
    private bool isMouseManagerSetNormalColor = true;
    private bool isMouseManagerSetOverColor = true;
    private bool isMouseManagerSetDownColor = true;
    private NormalUIPropWithMouse tempMouseController;



    //AnimationCallBackMode
    private bool isPlayDownAnimation=false;
    private GameObject downAnimation;
    private GameObject normalAnimation;
    private NormalUIPropWithMouseAnimation normalUIPropControllerWithMouseAnimation;



    //初始化
    private void Awake()
    {
        isMouseOverOnObj = false;
        isMouseDown = false;

        Cursor.visible = false;
        mouseManagerInstance = this;
        theMouseTransform = theMouse.transform;
        theMouseSpriteRenderer = theMouse.GetComponent<SpriteRenderer>();
        theMouseSpriteRenderer.sprite = defaultMouseSprite;

    }



    //检查鼠标是否落在物体上
    private void CheckMouseOvering()
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);


        if (hit.collider != null)
        {
            isMouseOverOnObj = true;

        }
        else
        {
            isMouseOverOnObj = false;
        }


    }




    //检查当前鼠标是否为按下状态
    public void CheckMouseDown()
    {
        isMouseDown = Input.GetMouseButton(0);

    }











    //************************DefaultCallBackMode模式**************************
    //(down，over,normal之间切换时会调用对应函数，若此时受控于某道具，则调用道具的对应方法)


    //DefaultCallBackMode模式下，变为鼠标按下状态
    private void DefaultCallBackMode_toDown()
    {
        switch (mouseState)
        {
            case MouseState.DefaultState:

                break;


            case MouseState.ControlByObj:

                tempMouseController.TheMouseDown(theMouse, theMouseTransform, theMouseSpriteRenderer);

                break;

            default:

                break;


        }


        if (isMouseManagerSetDownColor)
        {
            theMouseSpriteRenderer.color = downColor;
        }

    }


    //DefaultCallBackMode模式下，变为鼠标的正常状态
    private void DefaultCallBackMode_toNormal()
    {
        switch (mouseState)
        {
            case MouseState.DefaultState:

                break;


            case MouseState.ControlByObj:
                tempMouseController.TheMouseToNormal(theMouse, theMouseTransform, theMouseSpriteRenderer);

                break;

            default:

                break;


        }


        if (isMouseManagerSetNormalColor)
        {
            theMouseSpriteRenderer.color = normalColor;
        }




    }

    
    //DefaultCallBackMode模式下，变为鼠标在物品上的状态
    private void DefaultCallBackMode_toOver()
    {
        switch (mouseState)
        {
            case MouseState.DefaultState:


                break;


            case MouseState.ControlByObj:
                tempMouseController.TheMouseOverObj(theMouse, theMouseTransform, theMouseSpriteRenderer);

                break;

            default:

                break;


        }


        if (isMouseManagerSetOverColor)
        {
            theMouseSpriteRenderer.color = overColor;
        }

    }


    //设置鼠标被道具控制，DefaultCallBackMode模式
    public void SetMouseController_DefaultCallBackMode(NormalUIPropWithMouse mouseController, bool isSetNormalColor, bool isSetOverColor, bool isSetDownColor)
    {
        tempMouseController = mouseController;
        mouseState = MouseState.ControlByObj;
        mouseCallBackMode = MouseCallBackMode.defaultCallBackMode;
        isMouseManagerSetNormalColor = isSetNormalColor;
        isMouseManagerSetOverColor = isSetOverColor;
        isMouseManagerSetDownColor = isSetDownColor;


        SetSelf_DefaultCallBackMode();
    }


    //在被DefaultCallBackMode控制，被DefaultCallBackMode释放，被AnimationCallBackMode释放时被调用
    private void SetSelf_DefaultCallBackMode()
    {
        if (isMouseDown)
        {
            DefaultCallBackMode_toDown();

        }

        else
        {
            if (isMouseOverOnObj)
            {
                DefaultCallBackMode_toOver();
            }
            else
            {
                DefaultCallBackMode_toNormal();


            }
        }
    }


    //释放DefaultCallBackMode下的控制，由控制鼠标的道具来调用
    public void FreeControlOfMouse_DefaultCallBackMode()
    {
        mouseState = MouseState.DefaultState;
        theMouseSpriteRenderer.sprite = defaultMouseSprite;
        isMouseManagerSetNormalColor = true;
        isMouseManagerSetOverColor = true;
        isMouseManagerSetDownColor = true;

        SetSelf_DefaultCallBackMode();
    }







    //************************AnimationCallBackMode模式**************************
    //在按下动画未播放状态下，鼠标按下会调用对应方法进行动画播放。播放结束调用对应方法恢复正常的未播放状态


    //播放按下动画
    private void PlayDownAnimation_AnimationCallBackMode()
    {
        isPlayDownAnimation = true;
        downAnimation.SetActive(true);
        normalAnimation.SetActive(false);


    }


    //按下动画结束后，回到正常状态
    private void ToNotDown_AnimationCallBackMode()
    {

        isPlayDownAnimation = false;
        downAnimation.SetActive(false);
        normalAnimation.SetActive(true);

    }


    //设置鼠标被道具控制，AnimationCallBackMode模式
    public void SetMouseController_AnimationCallBackMode(NormalUIPropWithMouseAnimation theMouseControllor)
    {

        normalUIPropControllerWithMouseAnimation = theMouseControllor;
        mouseState = MouseState.ControlByObj;
        mouseCallBackMode = MouseCallBackMode.AnimationCallBackMode;
        //theAnimationMouse.SetActive(true);
        theMouseSpriteRenderer.color = zeroColor;
        downAnimation = Instantiate(theMouseControllor.GetDownClip(), theMouse.transform);
        normalAnimation = Instantiate(theMouseControllor.GetNormalClip(), theMouse.transform);

        ToNotDown_AnimationCallBackMode();
    }


    //释放AnimationCallBackMode下的控制，由控制鼠标的道具来调用
    public void FreeControlOfMouse_AnimationCallBackMode()
    {
        Destroy(downAnimation);
        Destroy(normalAnimation);
        mouseState = MouseState.DefaultState;
        mouseCallBackMode = MouseCallBackMode.defaultCallBackMode;
        theMouseSpriteRenderer.sprite = defaultMouseSprite;
        SetSelf_DefaultCallBackMode();

    }



    //提供给鼠标动画组件，用于回调
    public void FinishDownAnimation_AnimationCallBackMode()
    {
        ToNotDown_AnimationCallBackMode();

    }




    private void Update()
    {
        //defaultCallBackMode
        if (mouseCallBackMode==MouseCallBackMode.defaultCallBackMode)
        {
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            theMouseTransform.position = mouseWorldPosition;

            bool oldOvering = isMouseOverOnObj;
            bool oldDown = isMouseDown;

            CheckMouseOvering();
            CheckMouseDown();

            if ((oldDown!=isMouseDown))
            {
                if (isMouseDown)
                {
                    //todown
                    DefaultCallBackMode_toDown();
                }
                else
                {
                    if (isMouseOverOnObj)
                    {
                        //toOver
                        DefaultCallBackMode_toOver();
                    }
                    else
                    {
                        //toNormal
                        DefaultCallBackMode_toNormal();
                    }
                }
            }
            else if ((!isMouseDown) && (oldOvering != isMouseOverOnObj))
            {
                if (isMouseOverOnObj)
                {
                    //toOver
                    DefaultCallBackMode_toOver();
                }
                else
                {
                    //toNormal
                    DefaultCallBackMode_toNormal();
                }

            }

        }

        //AnimationCallBackMode
        else if (mouseCallBackMode == MouseCallBackMode.AnimationCallBackMode)
        {
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            theMouseTransform.position = mouseWorldPosition;
            
            bool oldDown = isMouseDown;

            CheckMouseDown();
            if((!oldDown) && (isMouseDown) && (!isPlayDownAnimation))
            {
                PlayDownAnimation_AnimationCallBackMode();

            }



        }

        //default
        else
        {

        }


    }




    //解除鼠标任意模式的控制，回到默认状态。（这函数暂时用不上）
    public void FreeControlOfMouse_AnimationAndDefaultMode()
    {
        if (mouseCallBackMode == MouseCallBackMode.AnimationCallBackMode)
        {
            FreeControlOfMouse_AnimationCallBackMode();
        }
        else if(mouseCallBackMode == MouseCallBackMode.defaultCallBackMode)
        {
            FreeControlOfMouse_DefaultCallBackMode();
        }

    }


}
