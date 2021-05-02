using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{


    public static Color overColor = new Color(1f, 0.8f, 0.6f, 1f);
    public static Color downColor = new Color(0.6f, 0.4f, 0f, 0.8f);
    public static Color normalColor = new Color(1f, 0.5f, 0f, 1f);

    public enum MouseState
    {
        DefaultState,//默认状态
        ControlByObj,//选中物品后，按下，点击等受控于其他道具，检测到点击/抬起后调用其他物体的方法
        Replaced//现有框架无法实现特殊需求时，将鼠标调为该状态，由其他物体自行实现的鼠标替代该鼠标。（该功能在当前项目暂时用不上，就先没实现，只留了接口）

    }

    public static MouseManager mouseManagerInstance;

    [SerializeField] private GameObject theMouse;
    private Transform theMouseTransform;
    private Vector3 mouseWorldPosition;
    private SpriteRenderer theMouseSpriteRenderer;

    [SerializeField] private Sprite defaultMouseSprite;

    private MouseState mouseState = MouseState.DefaultState;
    private NormalUIPropWithMouse tempMouseController;
    private bool isMouseManagerSetNormalColor = true;
    private bool isMouseManagerSetOverColor = true;
    private bool isMouseManagerSetDownColor = true;


    //private bool isMousePressed;
    private bool tValue;
    private bool isMouseOverOnObj;
    private bool isMouseOverChange;

    private bool isMouseDown;
    private bool isMouseDownChange;

    private void Awake()
    {
        isMouseOverOnObj = false;
        isMouseDown = false;

        Cursor.visible = false;
        mouseManagerInstance = this;
        theMouseTransform = theMouse.transform;
        theMouseSpriteRenderer = theMouse.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void CheckMouseOvering()
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


            // print("tValue" + tValue);
            // print("isMouseOveringOnObj" + isMouseOveringOnObj);
            /*

              if (tValue)
             {
                 isMouseChangeToNotOvering = true;

             }
             else
             {
                 isMouseChangeToOvering = true;
             }


             */

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




    }
}
