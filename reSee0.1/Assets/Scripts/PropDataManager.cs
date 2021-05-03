using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDataManager : MonoBehaviour
{


    [SerializeField] private InteractiveObj[] propPrefabs;//保存prefab，用于初始化prefabDic


    //public static PropDataManager propDataManagerInstance;


    private Dictionary<string, GameObject> propPrefabDictionary;//道具栏道具的prefab


    //道具栏道具的状态，一般情况下，这个dic应该是空的，道具没什么需要存的状态变量(四舍五入等于这属性目前卵用没有)
    private Dictionary<string, object> propStateDictionary;

    
    private HashSet<string>  nowProps;


    private string nowChosenProp="None";

    private bool isPropChosen=false;

    /*
     
     public void SetNowChosenProp(string nowProp)
    {
        nowChosenProp = nowProp;

    }
     
     */
    

    public string GetNowChosenProp()
    {
        return nowChosenProp;
    }
    

    /*
     public void SetIsPropChosen(bool isChosen)
    {
        isPropChosen = isChosen;
    }

     
     */
    
    public void SetNowChosenState(bool isChosen,string propName)
    {
        isPropChosen = isChosen;
        nowChosenProp = propName;
    }


    public bool GetIsPropChosen()
    {
        return isPropChosen;

    }


    public GameObject GetPropPrefab(string propName)
    {

        return propPrefabDictionary[propName];
    }


    public void AddProp(string propName)
    {
        nowProps.Add(propName);
    }

    public HashSet<string> GetNowProps()
    {
        return nowProps;
    }


    private void Awake()
    {
        nowProps = new HashSet<string>();

        propPrefabDictionary = new Dictionary<string, GameObject>();
        propStateDictionary = new Dictionary<string, object>();//暂时没卵用

        for(int i=0;i< propPrefabs.Length; i++)
        {
            propPrefabDictionary.Add(propPrefabs[i].GetObjName(), propPrefabs[i].gameObject);
        }

    }


}