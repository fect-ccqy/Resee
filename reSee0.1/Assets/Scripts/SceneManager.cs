using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    [SerializeField] private InteractiveObj[] sceneObjList;
    private bool isSceneObjRespondMouse = true;//场景中物体是否应响应鼠标点击（指流程上，播放动画与文字时是否应响应）


    public bool GetIsSceneObjRespondMouse()
    {
        return isSceneObjRespondMouse;
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
