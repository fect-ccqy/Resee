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
     *默认调用InteractiveObj.ObjTrigger(eventName),eventName建议在NormalTriggers中进行设置
     *若一切正常，返回0，若当前有正在处理的任务，返回-1。（目前暂不需实现序列功能，只需处理单个任务）
     *
     */
    //
    int AddTaskWithCallBack(T theTaskContent, TaskMessenger callBackMessenger,string callBackEvent);


}


