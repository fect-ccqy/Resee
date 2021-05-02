using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TextTaskContent
{
    public string[] texts;
    
}

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

    public static TextController theTextControllerInstance;

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
        theTextControllerInstance = this;
        SetSelf();
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
