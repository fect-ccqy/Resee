using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;


//踩过的坑：
//读取clip中已有的curve为unityEditor中的提供的方法，最后会没办法打包。因此需要在代码里从零拼出一个clip
//用animation无法实现点击跳过动画至结束帧
//用animator无法实现在代码中修改clip，直接修改使用中的clip以后会报错“Animator does not have an AnimatorController”
//每次都从零拼出一个new Clip倒是可行，但是写起来太麻烦，倒不如自己手写插值了


//该文件弃用,已重新手写插值，在PropAnimationControllor中实现
public class ObjAnimationController : MonoBehaviour
{


    /*
     
     
    public static ObjAnimationController objAnimationControllerInstance;


    


    //[SerializeField] private Image theBackImage;//黑暗背景


    //用于实现播放的属性
    [SerializeField] private GameObject startAnimationObj, endAnimationObj;//播放获得道具的游戏物体的obj，获得道具动画分为前半段和后半段
    private Animation startAnimation, endAnimation;//播放获得道具的游戏物体的Animation
    [SerializeField]private AnimationClip startAnimationClip, endAnimationClip;
    //private SpriteRenderer startSpriteRenderer, endSpriteRenderer;//播放获得道具的游戏物体的SpriteRenderer
    private AnimatorOverrideController animatorOverrideController;
    private Animator theAnimator;


   // AnimationCurve posXCurve, posYCurve;
    EditorCurveBinding posXBinding, posYBinding;
    //Keyframe[] posXKeys, posYKeys;


    AnimationCurve posXCurve, posYCurve;
    AnimationCurve scaleXCurve, scaleYCurve;
    AnimationCurve colorACurve;

    Keyframe[] posXKeys, posYKeys;
    Keyframe[] scaleXKeys, scaleYKeys;
    Keyframe[] colorAKeys;


    Type transformType;



    //用于实现调用与控制的属性



    AnimationClip animationClip;









     
     */





    /*
     
     
    public override int AddTask(AnimationTaskContent theTaskContent)
    {
        
       //startAnimation.Play()
        //startAnimation.clip.SetCurve(,)
        return 0;
    }

    public void SkipAnimation()
    {

    }





    public void PlayFinish()
    {

        print("finish");



    }


    public override int AddTaskWithCallBack(AnimationTaskContent theTaskContent, InteractiveObj callBackObj)
    {


        return 0;
    }

     
    private void SetStartKeyFram(float posX,float posY)
    {
        AnimationCurve animationCurve = new AnimationCurve(posYKeys);
        animationClip = new AnimationClip();
        animationClip.SetCurve("", transformType,"m_LocalPosition.y", animationCurve);

        startAnimation.clip = animationClip;
        
         
        posXKeys[0].value = posX;
        posYKeys[0].value = posY;
        posXCurve.keys = posXKeys;
        posYCurve.keys = posYKeys;
         
         

        //AnimationUtility.SetEditorCurve(startAnimationClip, posXBinding, posXCurve);
        //AnimationUtility.SetEditorCurve(startAnimationClip, posYBinding, posYCurve);
        //startAnimationClip.SetCurve("", transformType, "m_LocalPosition.x", posXCurve);
        //startAnimationClip.SetCurve("", transformType, "m_LocalPosition.y", posYCurve);
        //animatorOverrideController["BaseGetObjAnimationStart"].SetCurve("", transformType, "m_LocalPosition.x", posXCurve);
        //animatorOverrideController["BaseGetObjAnimationStart"].SetCurve("", transformType, "m_LocalPosition.y", posYCurve);
        //theAnimator.runtimeAnimatorController = animatorOverrideController;


        //animatorOverrideController["BaseGetObjAnimationStart"] = startAnimationClip;
    }


     



    private void StartClipPreprocessing()
    {

        AnimationCurve tCurve;
        Keyframe[] tKeys;

        EditorCurveBinding[] binds = AnimationUtility.GetCurveBindings(startAnimationClip);
        foreach (var bind in binds)
        {
            Debug.Log("Path: " + bind.path);
            Debug.Log("Type:"+bind.type);
            Debug.Log("Property Name: " + bind.propertyName);


            tCurve = AnimationUtility.GetEditorCurve(startAnimationClip, bind);
            tKeys = tCurve.keys;
            for(int i = 0; i < tKeys.Length; i++)
            {
                Debug.Log("KeyIndex: " + i);
                Debug.Log("time: " + tKeys[i].time);
                Debug.Log("value:" + tKeys[i].value);
                Debug.Log("inTangent: " + tKeys[i].inTangent);
                Debug.Log("inWeight: " + tKeys[i].inWeight);
                Debug.Log("outTangent: " + tKeys[i].outTangent);
                Debug.Log("outWeight: " + tKeys[i].outWeight);
                Debug.Log("weightedMode: " + tKeys[i].weightedMode);
                Debug.Log("tangentMode: " + tKeys[i].tangentMode);

                
            }


            
             
             if (bind.propertyName == "m_LocalPosition.x") {

                posXCurve = AnimationUtility.GetEditorCurve(startAnimationClip, bind);
                posXKeys = posXCurve.keys;
                transformType = bind.type;
                posXBinding = bind;

            }

            if (bind.propertyName == "m_LocalPosition.y") {

                posYCurve = AnimationUtility.GetEditorCurve(startAnimationClip, bind);
                posYKeys = posYCurve.keys;
                transformType = bind.type;
                posYBinding = bind;
            }
             
             
            

        }
        
         posXKeys = new Keyframe[2];
        posXKeys[0].time =;
        posXKeys[0].value =;
        posXKeys[0].inTangent =;
        posXKeys[0].inWeight =;

        posXKeys[0].outTangent =;
        posXKeys[0].outWeight =;
        posXKeys[0].weightedMode =;
         
         









    }

     
     
     */


     


    /*
     
      void Awake()
    {
        objAnimationControllerInstance = this;

        startAnimation = startAnimationObj.GetComponent<Animation>();
        endAnimation = endAnimationObj.GetComponent<Animation>();

        // = GetComponent<Animator>();

        //animatorOverrideController= new AnimatorOverrideController(theAnimator.runtimeAnimatorController);

        //theAnimator.runtimeAnimatorController = animatorOverrideController;

        //theAnimator.runtimeAnimatorController()[""]

        //startSpriteRenderer = startAnimationObj.GetComponent<SpriteRenderer>();
        //endSpriteRenderer = startAnimationObj.GetComponent<SpriteRenderer>();
        startAnimationClip = startAnimation.clip;
        endAnimationClip = endAnimation.clip;

        StartClipPreprocessing();
       
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            startAnimation.Play();

            //SetStartKeyFram(-10, -10);
            //if (theAnimator.runtimeAnimatorController == null) print("SSSSS");
            //print(theAnimator.runtimeAnimatorController);
            //theAnimator.Play("BaseGetObjAnimationStart",0,0);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SetStartKeyFram(10, 10);
            //theAnimator.Play("BaseGetObjAnimationEnd");

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SetStartKeyFram(10, 10);
            //animatorOverrideController["BaseGetObjAnimationStart"] = endAnimationClip;
            //theAnimator.Play("BaseGetObjAnimationStart", 0, 0);
        }

    }

     
     
     */
   

    /*
     
     
     public void PropGet()
    {
        Color theColor = theBackImage.color;
        theColor.a = 0.3f;
        theBackImage.color = theColor;
    }

     
     */
    
    

}
