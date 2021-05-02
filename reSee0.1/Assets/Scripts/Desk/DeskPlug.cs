using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;




//用于描述自身的状态。注意，所有的命名都要带上场景，防止后面出现同名混淆
public enum DeskPlugState
{
    Off,//未插上 
    On//插上

}

public class DeskPlug : SceneObjWithState<DeskPlugState>
{




    [SerializeField] private Sprite offSpirit;//插头未插上时的素材

    [SerializeField] private Sprite onSpirit;//插头插上时的素材

    

    DeskPlugState tPlugStateValue;//在逻辑中需要被频繁创建的临时变量，用于暂存从字典中读取的状态。原则上类的内部不应保存物体自身的状态，所有状态变量都应从DataDic中读取




    

    private void Awake()
    {
        InitAddDicKeyStateValue(DeskPlugState.Off);

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
        if (GameManager.gameManagerInstance.GetIsGlobalObjRespondMouse() && theSceneManager.GetIsSceneObjRespondMouse())
        {
            ObjTrigger(NormalTriggers.mouseClick);
            SetSelfViewByState();

        }



    }




    
    protected override void SetSelfViewByState()
    {
        
        tPlugStateValue = GetDicStateValue();


        if (tPlugStateValue == DeskPlugState.Off)
        {
            GetComponent<SpriteRenderer>().sprite = offSpirit;
        }
        else if (tPlugStateValue == DeskPlugState.On)
        {
            GetComponent<SpriteRenderer>().sprite = onSpirit;
        }

        else
        {
            
        }

    }

    public  override void ObjTrigger(string eventName)
    {
        if (eventName==NormalTriggers.mouseClick)
        {
            tPlugStateValue = GetDicStateValue();

            switch (tPlugStateValue)
            {

                case DeskPlugState.Off:
                    SetDicStateValue(DeskPlugState.On);
                    break;


                case DeskPlugState.On:
                    break;


                default:
                    break;
            }

        }


    }



    /*
    private void OnMouseOver()
    {
        tDicPlugStateValue = GetDicPlugState();
        if (tDicPlugStateValue.theStateValue == DeskPlugState.Off)
        {
            MouseManager.theMouseInstance.MouseOvering();
        }
    }
    */

}
