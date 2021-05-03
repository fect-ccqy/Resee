using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PropContent
{
    public string propName;
    public Vector2 startPosition;
    public Sprite thePropSprite;
}


//awake初始化依赖于propDataManager，propDataManager的awake初始化应在UIpropList的前面
public class UIPropList : MonoBehaviour,TaskMessenger,TaskProcessor<PropContent>
{
    //public static UIPropList uIPropListInstance;//提供给propUI实体，进行调用。SceneObj不应调用该属性




    [SerializeField] private PropDataManager thePropDataManager;//道具栏的数据

    //道具选择等相关控制
    private Dictionary<string, InteractiveObj> uiPropDic;

    private InteractiveObj nowProp;

    //private string nowChosenPropName="None";
    //private bool isPropChosen = false;


    [SerializeField] private GameObject uiPropListContent;//道具栏的展示窗




    //获得道具动画的相关逻辑
    [SerializeField] private PropAnimationController propAnimationController;//道具动画的控制器
    //[SerializeField] private InteractiveObj[] testInitObjList; 

    private bool isGettingProp = false;
    private string gettingProp;

    //回调相关属性
    private bool isCallBack = false;
    private TaskMessenger callBackMessenger;
    private string callBackEvent;



    //**************获得道具相关逻辑**************

    //动画播放结束后被调用，将prop实例化为UI
    public void CallBack(string EventName)
    {
        if (EventName == NormalTriggers.getPropAnimationFinish)
        {

            AddPropUI(gettingProp);

            gettingProp = "None";
            isGettingProp = false;
            if (isCallBack)
            {
                callBackMessenger.CallBack(callBackEvent);
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

            thePropDataManager.AddProp(gettingProp);//先在PropDataManager中加入新道具，等动画播放结束以后，在回调函数中将UI的实体进行实例化
            isGettingProp = true;
            AnimationTaskContent animationTaskContent;
            animationTaskContent.startPosition = theTaskContent.startPosition;
            animationTaskContent.thePropSprite = theTaskContent.thePropSprite;

            propAnimationController.AddTaskWithCallBack(animationTaskContent, this, NormalTriggers.getPropAnimationFinish);
            return 0;
        }

        else
        {
            return -1;
        }

    }


    //接收来自GameManager获得物体的调用
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
        GameObject tempInstantiate = Instantiate(thePropDataManager.GetPropPrefab(propName), uiPropListContent.transform);

        uiPropDic.Add(propName, tempInstantiate.GetComponent<InteractiveObj>());
        tempInstantiate.GetComponent<NormalUIProp>().SetUIPropList(this);
    }






    
    //***************道具栏互动相关逻辑****************


    //供propUI实体调用
    public bool GetIsPropChosen()
    {
        return thePropDataManager.GetIsPropChosen();

    }


    //供propUI实体调用
    public string GetNowChosenProp()
    {
        return thePropDataManager.GetNowChosenProp();
    }


    //propUI发生点击后，调用该方法。会回调选中与取消选中（先调用取消选中，然后再调用选中）
    public void MouseClickProp(string propName)
    {
        
        if (thePropDataManager.GetIsPropChosen())
        {
            if (thePropDataManager.GetNowChosenProp() == propName)
            {
                thePropDataManager.SetNowChosenState(false, "None");

                nowProp.ObjTrigger(NormalTriggers.cancelChosen);
                nowProp = null;

            }
            else
            {
                thePropDataManager.SetNowChosenState(true, propName);

                nowProp.ObjTrigger(NormalTriggers.cancelChosen);
                nowProp = uiPropDic[propName];
                nowProp.ObjTrigger(NormalTriggers.beChosen);

            }
        }
        else
        {
            thePropDataManager.SetNowChosenState(true, propName);

            nowProp = uiPropDic[propName];

            nowProp.ObjTrigger(NormalTriggers.beChosen);


        }

    }










    private void Awake()
    {

        uiPropDic = new Dictionary<string, InteractiveObj>();
        foreach(var propName in thePropDataManager.GetNowProps())
        {
            AddPropUI(propName);
        }
    }


}
