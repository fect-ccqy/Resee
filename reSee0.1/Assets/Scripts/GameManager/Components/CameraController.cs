using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct CameraMoveContent
{
    public Vector2 targetPosition;
    public float targetSize;//targetSize=-1时认为是移动回默认位置
    public float moveSpeed;//moveSpeed=-1时认为是移动回默认位置(两者有一个即可)
}


public class CameraController : MonoBehaviour, TaskProcessor<CameraMoveContent>
{

    //public static CameraController theCameraControllerInstance;

    private bool isCallBack = false;
    private TaskMessenger callBackMessenger;
    private string callBackEvent;

    private bool isWorking = false;

    [SerializeField] Camera theCamera;

    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private float defaultSize;
    [SerializeField] private float defaultSpeed;


    //实现给定目标尺寸，速度，目标位置，移动至该位置/尺寸的协程
    IEnumerator MoveCamera(Vector2 targetPosition,float targetSize,float moveSpeed)
    {
        Vector2 startPosition = transform.position;

        Vector2 deltaPosition = targetPosition - startPosition;

        float startSize = theCamera.orthographicSize;
        float deltaSize = targetSize - startSize;

        float theFtk;

        Vector2 tPosition;


        for (float timer = 0; timer < 1; timer += Time.deltaTime * moveSpeed)
        {
            theFtk = -2f * timer * timer * timer + 3f * timer * timer;
            tPosition = startPosition + deltaPosition * theFtk;

            transform.position = new Vector3(tPosition.x,tPosition.y,transform.position.z);
            theCamera.orthographicSize = startSize + deltaSize * theFtk;
            yield return 0;
        }

        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        theCamera.orthographicSize = targetSize;
        yield return 0;



    }
    
    
    //协程结束后调用该方法
    private void FinishWork()
    {
        isWorking = false;
        if (isCallBack)
        {
            callBackMessenger.CallBack(callBackEvent);
            isCallBack = false;
        }
    }


    //由GameManager调用该方法，进行移动任务的派发
    public int AddTask(CameraMoveContent theTaskContent)
    {
        if (!isWorking)
        {
            isWorking = true;
            if (theTaskContent.moveSpeed == -1 || theTaskContent.targetSize == -1)
            {
                StartCoroutine(MoveCamera(defaultPosition, defaultSize, defaultSpeed));
            }
            else {

                StartCoroutine(MoveCamera(theTaskContent.targetPosition, theTaskContent.targetSize, theTaskContent.moveSpeed));

            }

            return 0;
        }
        else
        {
            return -1;
        }
    }

    //由GameManager调用该方法，进行移动任务的派发
    public int AddTaskWithCallBack(CameraMoveContent theTaskContent,TaskMessenger tcallBackMessenger,string tcallBackEvent)
    {

        if (!isWorking)
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


    //移动回默认位置与尺寸。由GameManager调用该方法，进行移动任务的派发。
    public int MoveBackToDefaultPositionSize()
    {
        if (!isWorking)
        {
            isWorking = true;
            StartCoroutine(MoveCamera(defaultPosition, defaultSize, defaultSpeed));
           
            return 0;
        }
        else
        {
            return -1;
        }
    }


    //移动回默认位置与尺寸。由GameManager调用该方法，进行移动任务的派发
    public int MoveBackToDefaultPositionSizeWithCallBack(TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {

        if (!isWorking)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;
            MoveBackToDefaultPositionSize();

            return 0;
        }
        else
        {
            return -1;
        }
    }


}
