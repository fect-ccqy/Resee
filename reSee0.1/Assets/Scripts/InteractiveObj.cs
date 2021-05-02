using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObj : MonoBehaviour,TaskMessenger
{

    [SerializeField] protected string objName;

    //protected bool isRespondControlByGlobal = true;//是否受控于全局控制响应鼠标的变量，一般来说都是true




    //处理状态转换的方法（可以理解为状态机），发生事件后调用该方法，进行状态的变化。调用该方法后，通常需要调用SetSelfViewByState，来让物体在场景中的实体与dic中的状态同步
    public abstract void ObjTrigger(string eventName);


    //根据状态/其他数据对物体自身视图进行处理，不进行逻辑上的处理。在awake中会被调用进行初始化。在ObjTrigger被调用以后通常也需要调用SetSelfViewByState来实现场景中实例与dic中状态的一致
    protected abstract void SetSelfViewByState();


    public string GetObjName()
    {

        return objName;
    }

    public void CallBack(string EventName)
    {
        ObjTrigger(EventName);
    }



    /*
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
     */




    /*
     
    public bool GetIsRespondMouse()
    {
        return isRespondMouse;
    }
    public void SetIsRespondMouse(bool isRespondMouse)
    {
        this.isRespondMouse = isRespondMouse;
    }

     
     */






}
