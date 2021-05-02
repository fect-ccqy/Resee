using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;//单例模式

    private bool isGlobalObjRespondMouse = true;//场景中所有物体是否响应鼠标点击（场景中的物品与道具栏的道具都受其控制）（指流程上，播放动画与文字时是否应响应）


    public bool GetIsGlobalObjRespondMouse()
    {
        return isGlobalObjRespondMouse;
    }




    private void Awake()
    {
        gameManagerInstance = this;
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
