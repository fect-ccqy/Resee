using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    public enum MouseState
    {
        DefaultState,//默认状态
        ControlByObj_defaultCallBackMode,//选中物品后，按下，点击等受控于其他道具，检测到点击/抬起后调用其他物体的方法
        ControlByObj_AnimationCallBackMode,//选中物品后，按下，受控于其他道具，检测到点击后调用其他物体的方法播放动画，动画播放结束后调用MouseManager中的AniamtionPlayFinish方法。
        Replaced//现有框架无法实现特殊需求时，将鼠标调为该状态，由其他物体自行实现的鼠标替代该鼠标。（该功能在当前项目暂时用不上，就先没实现，只留了接口）

    }


    public static MouseManager mouseManagerInstance;
    
    [SerializeField] private GameObject theMouse;
    private Transform theMouseTransform;
    private Vector3 mouseWorldPosition;
    private SpriteRenderer theMouseSpriteRenderer;


    [SerializeField] private Sprite defaultMouseSprite;
    [SerializeField] private Color overColor;
    [SerializeField] private Color downColor;
    [SerializeField] private Color normalColor;
    private Color zeroColor=new Color(1f,1f,1f,0f);

    private MouseState mouseState = MouseState.DefaultState;


    
    
    private bool isMouseOverOnObj;
    private bool isMouseDown;

    //defaultCallBackMode              
    private bool isMouseManagerSetNormalColor = true;
    private bool isMouseManagerSetOverColor = true;
    private bool isMouseManagerSetDownColor = true;
    private NormalUIPropWithMouse tempMouseController;

    //AnimationCallBackMode
    private bool isPlayDownAnimation=false;
    //[SerializeField] private GameObject theAnimationMouse;
    //private Animation theMouseAnimation;
    private GameObject downAnimation;
    private GameObject normalAnimation;
    private NormalUIPropWithMouseAnimation normalUIPropControllerWithMouseAnimation;



    //private bool isMouseOverChange;
    //private bool isMouseDownChange;

    [SerializeField] AnimationClip testAnimationClip;





    private void Awake()
    {
        isMouseOverOnObj = false;
        isMouseDown = false;

        Cursor.visible = false;
        mouseManagerInstance = this;
        theMouseTransform = theMouse.transform;
        theMouseSpriteRenderer = theMouse.GetComponent<SpriteRenderer>();
    }



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

    public void CheckMouseDown()
    {
        isMouseDown = Input.GetMouseButton(0);

    }




    private void DefaultCallBackMode_toDown()
    {
        switch (mouseState)
        {
            case MouseState.DefaultState:

               

                break;


            case MouseState.ControlByObj_defaultCallBackMode:

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

    private void DefaultCallBackMode_toNormal()
    {
        switch (mouseState)
        {
            case MouseState.DefaultState:

                //theMouseSpriteRenderer.sprite = defaultMouseSprite;

                break;


            case MouseState.ControlByObj_defaultCallBackMode:
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
    private void DefaultCallBackMode_toOver()
    {
        switch (mouseState)
        {
            case MouseState.DefaultState:

                //theMouseSpriteRenderer.sprite = defaultMouseSprite;

                break;


            case MouseState.ControlByObj_defaultCallBackMode:
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





    public void SetMouseController_DefaultCallBackMode(NormalUIPropWithMouse mouseController, bool isSetNormalColor, bool isSetOverColor, bool isSetDownColor)
    {
        tempMouseController = mouseController;
        mouseState = MouseState.ControlByObj_defaultCallBackMode;
        isMouseManagerSetNormalColor = isSetNormalColor;
        isMouseManagerSetOverColor = isSetOverColor;
        isMouseManagerSetDownColor = isSetDownColor;


        SetSelf_DefaultCallBackMode();
    }

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


    public void FreeControlOfMouse_DefaultCallBackMode()
    {
        mouseState = MouseState.DefaultState;
        theMouseSpriteRenderer.sprite = defaultMouseSprite;
        isMouseManagerSetNormalColor = true;
        isMouseManagerSetOverColor = true;
        isMouseManagerSetDownColor = true;

        SetSelf_DefaultCallBackMode();
    }


    




    private void Update()
    {
        //defaultCallBackMode
        if (mouseState == MouseState.DefaultState|| mouseState == MouseState.ControlByObj_defaultCallBackMode)
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
        else if (mouseState == MouseState.ControlByObj_AnimationCallBackMode)
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



    private void PlayDownAnimation_AnimationCallBackMode()
    {
        isPlayDownAnimation = true;
        downAnimation.SetActive(true);
        normalAnimation.SetActive(false);


    }


    //提供给鼠标动画组件，用于回调
    public void FinishDownAnimation_AnimationCallBackMode()
    {
        ToNotDown_AnimationCallBackMode();

    }


    private void ToNotDown_AnimationCallBackMode()
    {

        isPlayDownAnimation = false;
        downAnimation.SetActive(false);
        normalAnimation.SetActive(true);

    }


    public void SetMouseController_AnimationCallBackMode(NormalUIPropWithMouseAnimation theMouseControllor)
    {

        normalUIPropControllerWithMouseAnimation = theMouseControllor;
        mouseState = MouseState.ControlByObj_AnimationCallBackMode;
        //theAnimationMouse.SetActive(true);
        theMouseSpriteRenderer.color = zeroColor;
        downAnimation = Instantiate(theMouseControllor.GetDownClip(), theMouse.transform);
        normalAnimation = Instantiate(theMouseControllor.GetNormalClip(), theMouse.transform);

        ToNotDown_AnimationCallBackMode();
    }


    public void FreeControlOfMouse_AnimationCallBackMode()
    {
        Destroy(downAnimation);
        Destroy(normalAnimation);
        mouseState = MouseState.DefaultState;
        theMouseSpriteRenderer.sprite = defaultMouseSprite;
        SetSelf_DefaultCallBackMode();

    }


    /*
     
     
    //public static Color overColor = new Color(1f, 0.8f, 0.6f, 1f);
    //public static Color downColor = new Color(0.6f, 0.4f, 0f, 0.8f);
    //public static Color normalColor = new Color(1f, 0.5f, 0f, 1f);

    


    //[SerializeField] private GameObject theMouse;
    //private Transform theMouseTransform;
    //private Vector3 mouseWorldPosition;
    //private SpriteRenderer theMouseSpriteRenderer;

    //[SerializeField] private Sprite defaultMouseSprite;


    //private MouseState mouseState = MouseState.DefaultState;
    private NormalUIPropWithMouse tempMouseController;
    private bool isMouseManagerSetNormalColor = true;
    private bool isMouseManagerSetOverColor = true;
    private bool isMouseManagerSetDownColor = true;


    //private bool isMousePressed;
    private bool tValue;
    //private bool isMouseOverOnObj;
    private bool isMouseOverChange;

    //private bool isMouseDown;
    private bool isMouseDownChange;
    */

    /*
    private void Awake()
    {
        isMouseOverOnObj = false;
        isMouseDown = false;

        Cursor.visible = false;
        mouseManagerInstance = this;
        theMouseTransform = theMouse.transform;
        theMouseSpriteRenderer = theMouse.GetComponent<SpriteRenderer>();
    }*/


    /*

    // Start is called before the first frame update
    void Start()
    {

    }


    public void AniamtionPlayFinish()
    {
        



    }

    private void CheckMouseOvering()
    {
        tValue = isMouseOverOnObj;
        isMouseOverChange = false;

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);


        if (hit.collider != null)
        {
            isMouseOverOnObj = true;

            // print("over");
        }
        else
        {
            isMouseOverOnObj = false;
        }

        if (tValue != isMouseOverOnObj)
        {
            isMouseOverChange = true;


        }



    }

    public void CheckMouseDown()
    {
        isMouseDownChange = false;
        tValue = isMouseDown;
        isMouseDown = Input.GetMouseButton(0);
        if (isMouseDown != tValue)
        {
            isMouseDownChange = true;
        }



    }




    private void ToDownMouse()
    {
        if (mouseState != MouseState.Replaced)
        {
            switch (mouseState)
            {
                case MouseState.DefaultState:

                    //theMouseSpriteRenderer.sprite = defaultMouseSprite;
                    
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


    }

    public void SetMouseController(NormalUIPropWithMouse mouseController,bool isSetNormalColor,bool isSetOverColor,bool isSetDownColor)
    {
        tempMouseController = mouseController;
        mouseState = MouseState.ControlByObj;
        isMouseManagerSetNormalColor = isSetNormalColor;
        isMouseManagerSetOverColor = isSetOverColor;
        isMouseManagerSetDownColor = isSetDownColor;


        SetSelf();
    }



    public void FreeControlOfMouse()
    {
        mouseState = MouseState.DefaultState;
        theMouseSpriteRenderer.sprite = defaultMouseSprite;
        isMouseManagerSetNormalColor = true;
        isMouseManagerSetOverColor = true;
        isMouseManagerSetDownColor = true;

        SetSelf();
    }







    private void ToOverObjMouse()
    {
        if (mouseState != MouseState.Replaced)
        {
            switch (mouseState)
            {
                case MouseState.DefaultState:

                    //theMouseSpriteRenderer.sprite = defaultMouseSprite;

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


    }

    //
    private void ToNormalMouse()
    {

        if (mouseState != MouseState.Replaced)
        {
            switch (mouseState)
            {
                case MouseState.DefaultState:

                    //theMouseSpriteRenderer.sprite = defaultMouseSprite;

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




    }

    private void SetSelf()
    {
        if (isMouseDown)
        {
            ToDownMouse();

        }

        else
        {
            if (isMouseOverOnObj)
            {
                ToOverObjMouse();
            }
            else
            {
                ToNormalMouse();


            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseState != MouseState.Replaced)
        {
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            theMouseTransform.position = mouseWorldPosition;


            CheckMouseOvering();
            CheckMouseDown();

            if (isMouseDownChange || isMouseOverChange)
            {
                SetSelf();
            }



        }



        switch (mouseState)
        {
            case MouseState.DefaultState:

                mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0;
                theMouseTransform.position = mouseWorldPosition;

                CheckMouseOvering();
                CheckMouseDown();

                if (isMouseDownChange || isMouseOverChange)
                {
                    SetSelf();
                }
                break;


            case MouseState.ControlByObj:
                tempMouseController.TheMouseOverObj(theMouse, theMouseTransform, theMouseSpriteRenderer);

                break;


            case MouseState.ControlByAnimationObj:
                tempMouseController.TheMouseOverObj(theMouse, theMouseTransform, theMouseSpriteRenderer);

                break;


            default:

                break;


        }



    }
     
     
     
     
     
     
     
     
     
     */


}
