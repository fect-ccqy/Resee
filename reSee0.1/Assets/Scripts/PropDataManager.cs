using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDataManager : MonoBehaviour
{


    [SerializeField] private InteractiveObj[] propPrefabs;//保存prefab，用于初始化prefabDic


    public static PropDataManager propDataManagerInstance;
    private Dictionary<string, GameObject> propPrefabDictionary;//道具栏道具的prefab
    private Dictionary<string, object> propStateDictionary;//道具栏道具的状态，一般情况下，这个dic应该是空的，道具没什么需要存的状态变量

    private HashSet<string>  nowProps;

    private string nowChosenProp="None";

    private bool isPropChosen=false;


    public void SetNowChosenProp(string nowProp)
    {
        nowChosenProp = nowProp;

    }

    public string GetNowChosenProp()
    {
        return nowChosenProp;
    }

    public void SetIsPropChosen(bool isChosen)
    {
        isPropChosen = isChosen;
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

    private void Awake()
    {
        nowProps = new HashSet<string>();

        propDataManagerInstance = this;
        propPrefabDictionary = new Dictionary<string, GameObject>();
        propStateDictionary = new Dictionary<string, object>();

        for(int i=0;i< propPrefabs.Length; i++)
        {
            propPrefabDictionary.Add(propPrefabs[i].GetObjName(), propPrefabs[i].gameObject);
        }

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
    /*
    public bool GetIsPropChosen()
    {
        return isPropChosen;

    }

    public void SetIsPropChosen(bool chosenState)
    {
        isPropChosen = chosenState;
    }


    public string GetNowChosenProp()
    {
        return nowChosenProp;
    }

    public void SetNowChosenProp(string chosenProp)
    {
        nowChosenProp = chosenProp;
    }


     
     */
   

