using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//用于描述自身的状态。注意，所有的命名都要带上场景，防止后面出现同名混淆
public enum DeskCalendarState
{
    HaveKey,//钥匙还没有被取走
    WithOutKey//钥匙被取走

}

public class DeskCalendar : SceneObjWithState<DeskCalendarState>
{

    DeskCalendarState tDeskCalendarState;
    [SerializeField]private Sprite keySprite;
    [SerializeField] private Vector2 keyPosition;
    private void Awake()
    {
        InitAddDicKeyStateValue(DeskCalendarState.HaveKey);

        SetSelfViewByState();

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUpAsButton()
    {

        if (GameManager.gameManagerInstance.GetIsGlobalObjRespondMouse())
        {
            ObjTrigger(NormalTriggers.mouseClick);
            SetSelfViewByState();

        }

    }



    public override void ObjTrigger(string eventName)
    {
        if (eventName == NormalTriggers.mouseClick)
        {
            tDeskCalendarState = GetDicStateValue();

            switch (tDeskCalendarState)
            {


                case DeskCalendarState.HaveKey:
                    PropContent propContent;
                    // propContent.propName = PropName.DeskCuKey;
                    propContent.propName = PropName.TestProp;
                    propContent.startPosition = keyPosition;
                    propContent.thePropSprite = keySprite;
                    //获得钥匙

                    GameManager.gameManagerInstance.AddProp(propContent);

                    SetDicStateValue(DeskCalendarState.WithOutKey);
                    break;


                case DeskCalendarState.WithOutKey:
                    break;


                default:
                    break;

            }

        }


    }


    //日历视图在流程上不会发生变化
    protected override void SetSelfViewByState()
    {
        


    }


}
