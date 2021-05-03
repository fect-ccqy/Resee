using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//除获得道具播放动画的组件外，目前其余TaskProcessor都应封装在GameManager中
public interface TaskProcessor<T>
{
    //添加任务到序列中，默认不进行回调。
    //若一切正常，返回0，若当前有正在处理的任务，返回-1（目前暂不需实现序列功能，只需处理单个任务）
    int AddTask(T theTaskContent);

    /**
     *添加任务到任务序列中，设置回调函数，
     *默认调用InteractiveObj.ObjTrigger(eventName),且默认eventName=NormalTriggers.XXXProcessorFinishWork
     *若一切正常，返回0，若当前有正在处理的任务，返回-1。（目前暂不需实现序列功能，只需处理单个任务）
     *
     */
    //
    int AddTaskWithCallBack(T theTaskContent, TaskMessenger callBackMessenger,string callBackEvent);


}


/*
 
 
 
public abstract class TaskProcessor<T> : MonoBehaviour
{


    //添加任务到序列中，默认不进行回调。
    //若一切正常，返回0，若当前有正在处理的任务，返回-1（目前暂不需实现序列功能，只需处理单个任务）
    public abstract int AddTask(T theTaskContent);

    /**
     *添加任务到任务序列中，设置回调函数，
     *默认调用InteractiveObj.ObjTrigger(eventName),且默认eventName=NormalTriggers.XXXProcessorFinishWork
     *若一切正常，返回0，若当前有正在处理的任务，返回-1。（目前暂不需实现序列功能，只需处理单个任务）
     *
     */
    //
    //public abstract int AddTaskWithCallBack(T theTaskContent, InteractiveObj callBackObj);






    //下面这些方法暂时用不上，就先注释掉了



    //添加任务到任务序列中，设置多个回调函数，默认调用InteractiveObj.ObjTrigger(eventName),默认eventName=NormalTriggers.XXXProcessorFinishWork
    //public abstract int AddTaskWithCallBackList(T theTaskContent, InteractiveObj[] callBackObj);



    //添加任务到任务列表中，设置回调函数与回调时的eventName，默认调用InteractiveObj.ObjTrigger(eventName),eventName自定义
    //public abstract int AddTaskWithCallBackAndEventName(T theTaskContent, InteractiveObj callBackObj,string eventName);




    //添加任务到任务列表中，设置多个回调函数，设置回调函数与回调时的eventName，默认调用InteractiveObj.ObjTrigger(eventName),eventName自定义
    //public abstract int AddTaskWithCallBackListAndEventName(T theTaskContent, InteractiveObj[] callBackObj, string eventName);




    // Start is called before the first frame update

/*
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

 
 
 
 */


