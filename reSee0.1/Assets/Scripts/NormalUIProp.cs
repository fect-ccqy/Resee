using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalUIProp : InteractiveObj
{
    public static Color chosenColor = new Color(1f, 1f, 1f, 0.7f);//选中后的背景颜色
    public static Color normalColor = new Color(1f, 1f, 1f, 0.4f);//未选中的背景颜色

    //记录该道具UI实体从属的道具栏，在生成后该NormalUIProp对象被调用SetUIpropList方法，给该变量赋值
    private UIPropList theUIPropList;

    [SerializeField] private Image backGroundImg;//背景图片
    //[SerializeField] private Image propGroundImg;


    //对象被创建后该方法会被propList调用，设置该道具UI实体从属的道具栏
    public void SetUIPropList(UIPropList tUIPropList)
    {
        theUIPropList = tUIPropList;
    }


    public override void ObjTrigger(string eventName)
    {
        //该分支由OnMouseClick()在发生点击时调用，
        if (eventName == NormalTriggers.mouseClick)
        {
            theUIPropList.MouseClickProp(objName);

        }

        //该分支由UIList判断该物体被选中时调用，
        else if (eventName == NormalTriggers.beChosen)
        {
            BeChosen();
        }

        //该分支由UIList判断该物体被取消选中时调用，
        else if (eventName == NormalTriggers.cancelChosen)
        {

            CancelChosen();
        }

    }


    //这个方法貌似没什么卵用，没有被调用过。。。。
    protected override void SetSelfViewByState()
    {
        if (theUIPropList.GetIsPropChosen()&& theUIPropList.GetNowChosenProp() == objName)
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

    //由道具栏实体调用该方法。若选中道具发生改变，每次的操作顺序为先取消选中，然后再调用新道具的选中方法
    protected virtual void CancelChosen()
    {
        backGroundImg.color = normalColor;
    }


    //由面板上的eventTrigger调用，发生点击时被调用
    public void OnMouseClick()
    {

        //Debug.LogFormat("click happen");
        if (GameManager.gameManagerInstance.GetIsGlobalObjRespondMouse())
        {
            ObjTrigger(NormalTriggers.mouseClick);
        }
    }



}
