using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneObjDataManager : MonoBehaviour
{
    
    public static SceneObjDataManager dataManagerInstance;//单例
    private Dictionary<string, object> objDataDictionary;//描述所有场景物品状态的变量
    //private Dictionary<string, object> checkerDictionary;//描述所有触发器的变量

    private void Awake()
    {
        dataManagerInstance = this;
        objDataDictionary = new Dictionary<string, object>();
    }


    //检查key是否存在
    public bool CheckKeyExistInObjDataDictionary(string theKey)
    {
        return objDataDictionary.ContainsKey(theKey);
    }

    //读取key对应的value
    public object GetFromObjDataDictionary(string theKey)
    {
        return objDataDictionary[theKey];
    }


    //添加键值对
    public void AddKeyValueDataDictionary(string theKey, object theValue)
    {
        objDataDictionary.Add(theKey, theValue);
    }

    //设置键值对
    public void SetObjDataDictionary(string theKey, object theValue)
    {
        objDataDictionary[theKey]=theValue;
    }



}
