using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct CameraMoveContent
{
    public Vector2 targetPosition;
    public float targetSize;
    public float moveSpeed;
}


public class CameraController : MonoBehaviour, TaskProcessor<CameraMoveContent>
{

    public static CameraController theCameraControllerInstance;

    private bool isCallBack = false;
    private TaskMessenger callBackMessenger;
    private string callBackEvent;

    private bool isWorking = false;

    [SerializeField] Camera theCamera;

    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private float defaultSize;
    [SerializeField] private float defaultSpeed;



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
    private void FinishWork()
    {
        isWorking = false;
        if (isCallBack)
        {
            callBackMessenger.CallBack(callBackEvent);
            isCallBack = false;
        }
    }

    public int AddTaskToDefault()
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

    public int AddTask(CameraMoveContent theTaskContent)
    {
        if (!isWorking)
        {
            isWorking = true;

            StartCoroutine(MoveCamera(theTaskContent.targetPosition, theTaskContent.targetSize, theTaskContent.moveSpeed));

            return 0;
        }
        else
        {
            return -1;
        }
    }

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

    private void Awake()
    {
        theCameraControllerInstance = this;
    }

}
