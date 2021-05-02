using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour,TaskMessenger
{
    public static GameManager gameManagerInstance;//单例模式

    private bool isGlobalObjRespondMouse = true;//场景中所有物体是否响应鼠标点击（场景中的物品与道具栏的道具都受其控制）（指流程上，播放动画与文字时是否应响应）

    [SerializeField] private TextController theTextController; 


    public bool GetIsGlobalObjRespondMouse()
    {
        return isGlobalObjRespondMouse;
    }
    public void SetIsGlobalObjRespondMouse(bool isRespond)
    {
        isGlobalObjRespondMouse = isRespond;
    }

    public void CallBack(string eventName)
    {
        if (eventName == NormalTriggers.TextFinishWork)
        {

            SetIsGlobalObjRespondMouse(true);
        }
        else if (eventName == NormalTriggers.PropListFinishWork)
        {
            SetIsGlobalObjRespondMouse(true);

        }
    }

    public int ShowText(TextTaskContent theContent)
    {
        SetIsGlobalObjRespondMouse(false);
        return theTextController.AddTaskWithCallBack(theContent,this,NormalTriggers.TextFinishWork);
    }
    public int AddProp(PropContent propContent)
    {
        SetIsGlobalObjRespondMouse(false);
        return UIPropList.uIPropListInstance.AddTaskWithCallBack(propContent, this, NormalTriggers.PropListFinishWork);
    }



    public int SetAllInterActive()
    {
        print("gameManager SetAllInterActive wait to finish");
        return 0;

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
