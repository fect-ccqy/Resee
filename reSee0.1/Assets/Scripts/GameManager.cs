using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TaskType
{
    ShowText,
    AddProp,
    MoveCamera

}




public class GameManager : MonoBehaviour,TaskMessenger
{
    public static GameManager gameManagerInstance;//单例模式

    private bool isGlobalObjRespondMouse = true;//场景中所有物体是否响应鼠标点击（场景中的物品与道具栏的道具都受其控制）（指流程上，播放动画与文字时是否应响应）



    //一些组件
    [SerializeField] private TextController theTextController;
    [SerializeField] private UIPropList theUIPropList;
    [SerializeField] private CameraController theCameraController;
    [SerializeField] private PropDataManager thePropDataManager;



    //关于同时调用，添加多个task的问题。从逻辑上应该避免，他们之间是互斥的。
    private bool isProcessorWorking;//当前是否有taskProcessor正在处理任务
    private bool isCallBack;//是否回调调用Game Manger中方法的物体
    private TaskMessenger callBackMessenger;//存储回调的taskMessenger。
    private string callBackEvent;






    public bool GetIsGlobalObjRespondMouse()
    {
        return isGlobalObjRespondMouse;
    }


    private void SetIsGlobalObjRespondMouse(bool isRespond)
    {
        isGlobalObjRespondMouse = isRespond;
    }

   



    public void CallBack(string eventName)
    {
        if (eventName == NormalTriggers.textFinishWork)
        {

            //SetIsGlobalObjRespondMouse(true);
        }
        else if (eventName == NormalTriggers.propListFinishWork)
        {
           // SetIsGlobalObjRespondMouse(true);

        }
        else if(eventName== NormalTriggers.cameraMoveFinishWork)
        {

        }


        isProcessorWorking = false;
        SetIsGlobalObjRespondMouse(true);

        if (isCallBack)
        {
            callBackMessenger.CallBack(callBackEvent);
            isCallBack = false;
        }






    }









    //***************显示文本相关********************


    //显示文本
    public int ShowText(TextTaskContent theContent)
    {
        if (!isProcessorWorking)
        {
            isProcessorWorking = true;


            SetIsGlobalObjRespondMouse(false);
            theTextController.AddTaskWithCallBack(theContent, this, NormalTriggers.textFinishWork);


            return 0;
        }
        else
        {
            return -1;
        }

    }


    //显示文本，添加回调
    public int ShowTextWithCallBack(TextTaskContent theContent, TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {
        if (!isProcessorWorking)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;


            ShowText(theContent);
            

            return 0;
        }
        else
        {
            print("wrong isProcessorWorking=true");
            return -1;
        }

        
    }













    //******************道具相关******************
    //(获得道具的过程中，gamemanager只传递调用，不处理相关逻辑，不操作propData中的数据。具体逻辑与数据操作交由uiPropList实现)

    public int AddProp(PropContent propContent)
    {
        if (!isProcessorWorking)
        {
            isProcessorWorking = true;


            SetIsGlobalObjRespondMouse(false);
            theUIPropList.AddTaskWithCallBack(propContent, this, NormalTriggers.propListFinishWork);


            return 0;
        }
        else
        {
            return -1;
        }

    }

    //显示文本，添加回调
    public int AddPropWithCallBack(PropContent propContent, TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {
        if (!isProcessorWorking)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;

            AddProp(propContent);

            return 0;
        }
        else
        {
            print("wrong isProcessorWorking=true");
            return -1;
        }


    }


    //后面按照需求应该会添加一个删除道具的功能









    //******************移动相机相关******************
    //(获得道具的过程中，gamemanager只传递调用，不处理相关逻辑，不操作propData中的数据。具体逻辑与数据操作交由uiPropList实现)

    

    //移动相机
    public int CameraMove(CameraMoveContent theTaskContent)
    {
        if (!isProcessorWorking)
        {
            isProcessorWorking = true;


            SetIsGlobalObjRespondMouse(false);
            theCameraController.AddTaskWithCallBack(theTaskContent, this, NormalTriggers.cameraMoveFinishWork);


            return 0;
        }
        else
        {
            return -1;
        }

    }

    //移动相机，添加回调
    public int CameraMoveWithCallBack(CameraMoveContent theTaskContent, TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {
        if (!isProcessorWorking)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;

            CameraMove(theTaskContent);

            return 0;
        }
        else
        {
            print("wrong isProcessorWorking=true");
            return -1;
        }


    }


    //移动相机回到默认位置
    public int CameraMoveBackToDefault()
    {
        if (!isProcessorWorking)
        {
            isProcessorWorking = true;


            SetIsGlobalObjRespondMouse(false);
            theCameraController.MoveBackToDefaultPositionSizeWithCallBack(this, NormalTriggers.cameraMoveFinishWork);


            return 0;
        }
        else
        {
            return -1;
        }

    }

    //移动相机回到默认位置
    public int CameraMoveBackToDefaultWithCallBack(TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {
        if (!isProcessorWorking)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;

            CameraMoveBackToDefault();

            return 0;
        }
        else
        {
            print("wrong isProcessorWorking=true");
            return -1;
        }


    }










    //******************道具栏相关(开放给SceneObj的一些方法)******************

    //获得当前道具栏是否被选中的状态
    public bool GetIsPropChosen()
    {
        return thePropDataManager.GetIsPropChosen();
    }

    //获得当前道具栏选中的道具的名称
    public string GetNowChosenProp()
    {
        return thePropDataManager.GetNowChosenProp();
    }




    private void Awake()
    {
        gameManagerInstance = this;
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
