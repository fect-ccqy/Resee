using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TextTaskContent
{
    public string[] texts;
    
}


//对脚本加载的awake顺序无要求
public class TextController : MonoBehaviour,TaskProcessor<TextTaskContent>
{
    [SerializeField] private Text theText;
    [SerializeField] private GameObject theTextCanvas;
    [SerializeField] private Color defaultColor;
    [SerializeField] private int defaultTextSize;
    [SerializeField] private float showTextSpeed;
    [SerializeField] private float fadeInSpeed;

    private bool isWorking = false;
    private string[] nowWorkContent;

    private bool isCallBack = false;
    private TaskMessenger callBackMessenger;
    private string callBackEvent;

    private bool isShowTextFinish = false;
    private bool isFadeInFinish = false;

    //public static TextController theTextControllerInstance;

    public void SetTextSize(int textSize)
    {
        theText.fontSize = textSize;
    }
    public void SetTextColor(Color textColor)
    {
        theText.color = textColor;
    }

    public void SetToDefaultSize()
    {
        theText.fontSize = defaultTextSize;
    }

    public void SetToDefaultColor()
    {
        theText.color = defaultColor;
    }


    //打字机效果
    IEnumerator ShowOneSentenceText(string theSentence)
    {
        
        for(float timer = 0; timer < theSentence.Length; timer += showTextSpeed * Time.deltaTime)
        {
            theText.text = theSentence.Substring(0, (int)timer);
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }

            yield return 0;
        }

        theText.text = theSentence;
        yield return 0;
        isShowTextFinish = true;

    }
    
    //淡入效果
    IEnumerator ShowOneSentenceFadeIn()
    {
        Color tColor = theText.color;
        float theColora = tColor.a;
        for (float timer = 0; timer < theColora; timer += fadeInSpeed * Time.deltaTime)
        {
            tColor.a = timer;
            theText.color = tColor;
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return 0;
        }
        tColor.a = theColora;
        theText.color = tColor;
        yield return 0;
        isFadeInFinish = true;
    }


    //逐行显示各个句子
    IEnumerator ShowText()
    {
        for(int i = 0; i < nowWorkContent.Length; i++)
        {
            isShowTextFinish = false;
            isFadeInFinish = false;
            StartCoroutine(ShowOneSentenceText(nowWorkContent[i]));
            StartCoroutine(ShowOneSentenceFadeIn());
            while (true)
            {
                if(Input.GetMouseButtonDown(0)&&isShowTextFinish && isFadeInFinish)
                {
                    yield return 0;
                    break;
                }

                yield return 0;
            }

        }


        FinishWork();


    }


    //显示完成后会调用该方法
    private void FinishWork()
    {
        isWorking = false;
        SetSelf();
        if (isCallBack)
        {
            callBackMessenger.CallBack(callBackEvent);
            isCallBack = false;
        }
    }

    //在场景加载初始化，添加任务，以及任务结束后被调用
    private void SetSelf()
    {
        if (isWorking)
        {
            theTextCanvas.SetActive(true);
        }
        else
        {
            theTextCanvas.SetActive(false);
        }
    }


    //添加任务
    public int AddTask(TextTaskContent theContent)
    {
        if (!isWorking)
        {
            nowWorkContent = theContent.texts;
            isWorking = true;
            SetSelf();
            StartCoroutine(ShowText());
            return 0;
        }
        else
        {
            return -1;
        }
    }

    //添加任务，设置回调
    public int AddTaskWithCallBack(TextTaskContent theContent, TaskMessenger tcallBackMessenger, string tcallBackEvent)
    {
        if (!isWorking)
        {
            isCallBack = true;
            callBackMessenger = tcallBackMessenger;
            callBackEvent = tcallBackEvent;
            AddTask(theContent);

            return 0;
        }
        else
        {
            return -1;
        }
    }



    private void Awake()
    {
        SetSelf();
    }

}
