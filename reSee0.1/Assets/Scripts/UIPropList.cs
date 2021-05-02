using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PropContent
{
    public string propName;
    public Vector2 startPosition;
    public Sprite thePropSprite;
}

public class UIPropList : MonoBehaviour,TaskMessenger,TaskProcessor<PropContent>
{
    public static UIPropList uIPropListInstance;//提供给propUI实体，进行调用。SceneObj不应调用该属性

    private Dictionary<string, InteractiveObj> uiPropDic;

    private InteractiveObj nowProp;

    private string nowChosenPropName="None";

    private bool isPropChosen = false;

    [SerializeField] private GameObject uiPropListContent;
    [SerializeField] private PropAnimationController propAnimationController;
    //[SerializeField] private InteractiveObj[] testInitObjList; 



    private bool isGettingProp = false;
    private string gettingProp;

    private bool isCallBack = false;
    private TaskMessenger callBackMessenger;
    private string callBackEvent;

    //动画播放结束后被调用
    public void CallBack(string EventName)
    {
        if (EventName == NormalTriggers.GetPropAnimationFinish)
        {

            AddPropUI(gettingProp);

            gettingProp = "None";
            isGettingProp = false;
            if (isCallBack)
            {
                callBackMessenger.CallBack(callBackEvent);//回调当时发出任务的物体（一般来说，propList用不上这个分支）
                isCallBack = false;
            }
        }
        else
        {
            print("uiproplist callback wrong");
        }

    }


    //接收来自GameManager的获得物体的调用
    public int AddTask(PropContent theTaskContent)
    {
        if (!isGettingProp)
        {
            gettingProp = theTaskContent.propName;

            PropDataManager.propDataManagerInstance.AddProp(gettingProp);
            isGettingProp = true;
            AnimationTaskContent animationTaskContent;
            animationTaskContent.startPosition = theTaskContent.startPosition;
            animationTaskContent.thePropSprite = theTaskContent.thePropSprite;

            propAnimationController.AddTaskWithCallBack(animationTaskContent, this, NormalTriggers.GetPropAnimationFinish);
            return 0;
        }

        else
        {
            return -1;
        }

    }


    //接收来自GameManager的调用
    public int AddTaskWithCallBack(PropContent theTaskContent, TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {
        if (!isGettingProp)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;
            AddTask(theTaskContent);

            return 0;
        }

        else
        {
            return -1;
        }
    }



    //在列表中添加道具实体
    private void AddPropUI(string propName)
    {
        GameObject tempInstantiate = Instantiate(PropDataManager.propDataManagerInstance.GetPropPrefab(propName), uiPropListContent.transform);

        uiPropDic.Add(propName, tempInstantiate.GetComponent<InteractiveObj>());

    }


    //供propUI实体调用
    public bool GetIsPropChosen()
    {
        return isPropChosen;

    }

    //供propUI实体调用
    public string GetNowChosenProp()
    {
        return nowChosenPropName;
    }


    /*

    public void SetIsPropChosen(bool chosenState)
    {
        isPropChosen = chosenState;
    }


   

    public void SetNowChosenProp(string chosenProp,InteractiveObj nowProp)
    {
        nowChosenProp = chosenProp;
        nowChosenProp = nowProp;
    }
     
     
     */


    //propUI发生点击后，调用该方法
    public void MouseClickProp(string propName)
    {
        
        if (isPropChosen)
        {
            if (nowChosenPropName == propName)
            {

                isPropChosen=false;
                PropDataManager.propDataManagerInstance.SetIsPropChosen(false);
                nowChosenPropName = "None";
                PropDataManager.propDataManagerInstance.SetNowChosenProp("None");

                nowProp.ObjTrigger(NormalTriggers.cancelChosen);
                nowProp = null;

            }
            else
            {
                isPropChosen = true;

                PropDataManager.propDataManagerInstance.SetIsPropChosen(true);

                nowChosenPropName = propName;

                PropDataManager.propDataManagerInstance.SetNowChosenProp(propName);

                nowProp.ObjTrigger(NormalTriggers.cancelChosen);
                nowProp = uiPropDic[propName];
                nowProp.ObjTrigger(NormalTriggers.beChosen);

            }
        }
        else
        {
            isPropChosen = true;

            PropDataManager.propDataManagerInstance.SetIsPropChosen(true);
            nowChosenPropName = propName;

            PropDataManager.propDataManagerInstance.SetNowChosenProp(propName);

            nowProp = uiPropDic[propName];

            nowProp.ObjTrigger(NormalTriggers.beChosen);


        }

    }



    private void Awake()
    {
        uIPropListInstance = this;
        uiPropDic = new Dictionary<string, InteractiveObj>();
        
    }


}
