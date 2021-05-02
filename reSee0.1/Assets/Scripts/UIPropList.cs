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
    public static UIPropList uIPropListInstance;

    private Dictionary<string, InteractiveObj> uiPropDic;

    private InteractiveObj nowProp;

    private string nowChosenPropName="None";

    private bool isPropChosen = false;

    [SerializeField] private GameObject uiPropListContent;

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

            PropAnimationController.thePropAnimationControllerInstance.AddTaskWithCallBack(animationTaskContent, this, NormalTriggers.GetPropAnimationFinish);
            return 0;
        }

        else
        {
            return -1;
        }



    }

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






    /*
     
    public void GetProp(string propName, AnimationTaskContent theContent)
    {
        gettingProp = propName;
        PropAnimationController.thePropAnimationControllerInstance.AddTask(theContent);
    }


     public int GetProp(AnimationTaskContent theContent,string propName,bool tisCallBack,InteractiveObj tcallBackObj)
    {
        if (isGettingProp == true)
        {
            return -1;
        }
        isCallBack = tisCallBack;
        callBackObj = tcallBackObj;
        GetProp(propName, theContent);
        return 0;
    }

     
     */
    


    private void AddPropUI(string propName)
    {
        GameObject tempInstantiate = Instantiate(PropDataManager.propDataManagerInstance.GetPropPrefab(propName), uiPropListContent.transform);

        uiPropDic.Add(propName, tempInstantiate.GetComponent<InteractiveObj>());

    }



    public bool GetIsPropChosen()
    {
        return isPropChosen;

    }

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


        /*
         
         for(int i = 0; i < testInitObjList.Length; i++)
        {
            uiPropDic.Add(testInitObjList[i].GetObjName(), testInitObjList[i]);
        }
         
         */
        


    }




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
