using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalUIProp : InteractiveObj
{
    public static Color chosenColor = new Color(1f, 1f, 1f, 0.7f);
    public static Color normalColor = new Color(1f, 1f, 1f, 0.4f);

    [SerializeField] private Image backGroundImg;
    //[SerializeField] private Image propGroundImg;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ObjTrigger(string eventName)
    {

        if (eventName == NormalTriggers.mouseClick)
        {
            UIPropList.uIPropListInstance.MouseClickProp(objName);

        }
        else if (eventName == NormalTriggers.beChosen)
        {
            BeChosen();
        }
        else if(eventName == NormalTriggers.cancelChosen)
        {

            CancelChosen();
        }

    }



    protected override void SetSelfViewByState()
    {
        if (UIPropList.uIPropListInstance.GetIsPropChosen()&& UIPropList.uIPropListInstance.GetNowChosenProp() == objName)
        {
            BeChosen();
        }
        else
        {
            CancelChosen();

        }
    }


    //由道具栏实体调用该方法。若选中道具发生改变，每次的操作顺序为先取消选中，然后再调用新道具的选中方法
    protected virtual void BeChosen()
    {

        backGroundImg.color = chosenColor;
    }

    protected virtual void CancelChosen()
    {
        backGroundImg.color = normalColor;
    }

    public void OnMouseClick()
    {

        //Debug.LogFormat("click happen");
        if (GameManager.gameManagerInstance.GetIsGlobalObjRespondMouse())
        {

            ObjTrigger(NormalTriggers.mouseClick);

        }
    }



}
