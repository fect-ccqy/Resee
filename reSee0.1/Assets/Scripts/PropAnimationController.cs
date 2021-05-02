using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public struct AnimationTaskContent
{
    public Vector2 startPosition;
    public Sprite thePropSprite;


}


//踩过的坑：
//读取clip中已有的curve为unityEditor中的提供的方法，最后会没办法打包。因此需要在代码里从零拼出一个clip
//用animation无法实现点击跳过动画至结束帧
//用animator无法实现在代码中修改clip，直接修改使用中的clip以后会报错“Animator does not have an AnimatorController”
//每次都从零拼出一个new Clip倒是可行，但是写起来太麻烦，倒不如自己手写插值了
public class PropAnimationController :MonoBehaviour, TaskProcessor<AnimationTaskContent>
{


    private enum PropAnimationControllerState
    {
        NotPlaying,     //当前未播放动画
        PlayingStart,   //当前正在播放开始动画
        MidWaitClick,   //当前开始播放以结束，等待鼠标点击进行结束播放
        PlayingEnd      //当前正在进行结束的播放（结束的播放完成以后，转为NotPlaying状态）
    }
    //public static PropAnimationController thePropAnimationControllerInstance;

    private PropAnimationControllerState propAnimationControllerState = PropAnimationControllerState.NotPlaying;

    private float theK = 0.4f;
    [SerializeField] private GameObject theProp;
    [SerializeField] private GameObject BackGroundCanvas;
    private SpriteRenderer thePropSpriteRenderer;
    private Transform thePropTransform;



    //尺寸，坐标，透明度
    private Vector2 startPosition;
    [SerializeField] private Vector2 startScale;
    [SerializeField] private Color startColora;

    [SerializeField] private Vector2 midPosition;
    [SerializeField] private Vector2 midScale;
    [SerializeField] private Color midColora;

    [SerializeField] private Vector2 endPosition;
    [SerializeField] private Vector2 endScale;
    [SerializeField] private Color endColora;


    private bool isCallBack = false;
    private TaskMessenger callBackMessenger;
    private string callBackEvent;

    private void Awake()
    {
        thePropSpriteRenderer = theProp.GetComponent<SpriteRenderer>();
        thePropTransform = theProp.transform;

        SetSelf();
    }

    private void SetSelf()
    {
        if (propAnimationControllerState == PropAnimationControllerState.NotPlaying)
        {
            theProp.SetActive(false);
            BackGroundCanvas.SetActive(false);
        }
        else
        {
            theProp.SetActive(true);
            BackGroundCanvas.SetActive(true);
        }

    }




    public int AddTask(AnimationTaskContent theTaskContent)
    {
        if (propAnimationControllerState == PropAnimationControllerState.NotPlaying)
        {

            propAnimationControllerState = PropAnimationControllerState.PlayingStart;
            thePropSpriteRenderer.sprite = theTaskContent.thePropSprite;
            startPosition = theTaskContent.startPosition;
            StartCoroutine(PlayGetPropAnimation());

        }

        else
        {
            return -1;
        }

        return 0;

    }

    public int AddTaskWithCallBack(AnimationTaskContent theTaskContent, TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {
        if (propAnimationControllerState == PropAnimationControllerState.NotPlaying)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;
            AddTask(theTaskContent);
        }

        else
        {
            return -1;
        }

        return 0;
    }



    IEnumerator PlayGetPropAnimation()
    {

        float startT = 0.7f;
        float endT = 0.4f;


        //k1*nt^3+k2*nt^2，满足f(nt=0)=0，f(nt=1)=1，f'(nt=0)=0，f'(nt=1)=0，
        float interpK1 = -2f;
        float interpK2 = 3f;

        //-k*nt^4+k*nt^2，满足f(nt=0)=0，f(nt=1)=0，用于添加视觉上的扰动项
        float disturbanceK = 0.35f;


        Vector2 deltaPosition;
        Vector2 deltaScale;
        Vector2 disturbancePos = new Vector2(0, 4);
        Color deltaColora;

        SetSelf();
        thePropTransform.position = startPosition;
        thePropTransform.localScale = startScale;
        thePropSpriteRenderer.color = startColora;



        deltaPosition = midPosition - startPosition;
        deltaScale = midScale - startScale;
        deltaColora = midColora - startColora;

        float normalize_t;

        float interp_normalize_k;

        float disturbance_normalize_k;

        for (float timer = 0; timer < startT; timer += Time.deltaTime)
        {
            normalize_t = timer / startT;
            interp_normalize_k = interpK1 * normalize_t * normalize_t * normalize_t + interpK2 * normalize_t * normalize_t;
            disturbance_normalize_k = -disturbanceK * normalize_t * normalize_t * normalize_t * normalize_t + disturbanceK * normalize_t * normalize_t;

            thePropTransform.position = startPosition + deltaPosition * interp_normalize_k + disturbancePos * disturbance_normalize_k;
            thePropTransform.localScale = startScale + deltaScale * interp_normalize_k;
            thePropSpriteRenderer.color = startColora + deltaColora * interp_normalize_k;


            if (Input.GetMouseButtonDown(0))
            {
                break;
            }

            yield return 0;
        }

        propAnimationControllerState = PropAnimationControllerState.MidWaitClick;

        thePropTransform.position = midPosition;
        thePropTransform.localScale = midScale;
        thePropSpriteRenderer.color = midColora;
        

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //yield return 0;
                break;
            }
            yield return 0;
        }
        yield return 0;

        propAnimationControllerState = PropAnimationControllerState.PlayingEnd;

        deltaPosition = endPosition - midPosition;
        deltaScale = endScale - midScale;
        deltaColora = endColora - midColora;


        for (float timer = 0; timer < endT; timer += Time.deltaTime)
        {
            normalize_t = timer / endT;
            interp_normalize_k = interpK1 * normalize_t * normalize_t * normalize_t + interpK2 * normalize_t * normalize_t;
            disturbance_normalize_k = -disturbanceK * normalize_t * normalize_t * normalize_t * normalize_t + disturbanceK * normalize_t * normalize_t;

            thePropTransform.position = midPosition + deltaPosition * interp_normalize_k + disturbancePos * disturbance_normalize_k;
            thePropTransform.localScale = midScale + deltaScale * interp_normalize_k;
            thePropSpriteRenderer.color = midColora + deltaColora * interp_normalize_k;

            /*
             if (Input.GetMouseButtonDown(0))
            {
                break;
            }

             
             */
            
            yield return 0;
        }


        thePropTransform.position = endPosition;
        thePropTransform.localScale = endScale;
        thePropSpriteRenderer.color = endColora;

        yield return 0;

        FinishWork();




    }


    private void FinishWork()
    {
        //if (isCallBack)
        //{

        //}
        propAnimationControllerState = PropAnimationControllerState.NotPlaying;
        SetSelf();
        if (isCallBack)
        {
            callBackMessenger.CallBack(callBackEvent);
            isCallBack = false;
        }

    }


}
