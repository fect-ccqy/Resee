using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneObj : MonoBehaviour,TaskMessenger
{
    int stateValue=0;

    [SerializeField] private Sprite[] testSprite;


    public void CallBack(string EventName)
    {
        print("test CallBack");
    }



    private void OnMouseUpAsButton()
    {
        if (GameManager.gameManagerInstance.GetIsGlobalObjRespondMouse()) {


            if (stateValue == 0)
            {

                if (!GameManager.gameManagerInstance.GetIsProcessorWorking())
                {

                    TextTaskContent textTaskContent;
                    string[] testStrings = { "测试文本", "插头被插上了", "嘤嘤嘤嘤嘤嘤嘤嘤" };
                    textTaskContent.texts = testStrings;

                    if (GameManager.gameManagerInstance.ShowText(textTaskContent) == 0)
                    {

                        stateValue++;
                    }
                }

            }

            else if (stateValue == 1)
            {
                PropContent propContent;
                propContent.propName = PropName.TestDeskCuKey;
                propContent.startPosition = new Vector2(5,5);
                propContent.thePropSprite = testSprite[0];

                GameManager.gameManagerInstance.AddProp(propContent);

                stateValue++;
            }
            else if (stateValue == 2)
            {
                PropContent propContent;
                propContent.propName = PropName.TestProp1;
                propContent.startPosition = new Vector2(-5, 5);
                propContent.thePropSprite = testSprite[1];

                GameManager.gameManagerInstance.AddProp(propContent);

                stateValue++;
            }
            else if (stateValue == 3)
            {

                PropContent propContent;
                propContent.propName = PropName.TestProp2;
                propContent.startPosition = new Vector2(-5, -5);
                propContent.thePropSprite = testSprite[2];

                GameManager.gameManagerInstance.AddPropWithCallBack(propContent,this,"TestCallBackEvent");

                stateValue++;
            }
            else if (stateValue == 4)
            {
                TextTaskContent textTaskContent;
                string[] testStrings = { "测试文本", "插头被插上了", "嘤嘤嘤嘤嘤嘤嘤嘤" };
                textTaskContent.texts = testStrings;
                GameManager.gameManagerInstance.ShowText(textTaskContent);

                stateValue++;
            }
            else if (stateValue == 5)
            {
                if(GameManager.gameManagerInstance.GetNowChosenProp()== PropName.TestProp1)
                {
                    if (GameManager.gameManagerInstance.DeleteProp(PropName.TestProp1) == 0)
                    {
                        stateValue++;
                    }
                }


                //stateValue++;
            }

            else if (stateValue == 6)
            {
                if (GameManager.gameManagerInstance.GetNowChosenProp() == PropName.DeskCuKey)
                {
                    if (GameManager.gameManagerInstance.DeleteProp(PropName.DeskCuKey) == 0)
                    {
                        stateValue++;
                    }
                }


                //stateValue++;
            }

        }
        



    }
}
