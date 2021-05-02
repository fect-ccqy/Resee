using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *
 * 
 *
 *工具类，实现对DataDic查询的封装
 *如果有加新的字典，可以将新的查询的操作封装在这里
 *
 *
 */
public abstract class DicDataReader : MonoBehaviour
{
    public static readonly string SceneObjDataDicName = "SceneObjDataDicName";


    //对键值对添加进行了封装，原则上该方法在游戏流程中应且只应在初始化（awake）的时候被调用一次。若dic不存在对应的key，则进行key-value的添加，返回0；若dic中已有对应的key，则返回1，不对dic中已有键值对进行修改
    public static int InitAddDicKeyStateValue<T>(string DicName,string objName,T stateValue)
    {

        //根据要查询的词典在进入对应分支（注，查了一下资料，c#的string可以直接用==比较，跟java不太一样）
        if (DicName==SceneObjDataDicName)
        {
             //如果不存在对应的key，则加入dic，完成状态的初始化
            if (!SceneObjDataManager.dataManagerInstance.CheckKeyExistInObjDataDictionary(objName))
            {
                SceneObjDataManager.dataManagerInstance.AddKeyValueDataDictionary(objName, stateValue);
                return 0;//正常初始化
            }

            //若存在对应的key，则说明本次在awake中被调用为读档操作，不应对dic中的key-value进行初始化覆盖存档数据
            else
            {
                print("The " + objName + " Key-Value is existed in " + DicName);
                return 1;//key-value已经存在
            }

        }
        return -1;

    }



    //封装了读取状态变量的操作
    public static T GetDicStateValue<T>(string DicName, string objName)
    {
        T theStateValue;


        if (DicName==SceneObjDataDicName)
        {
            theStateValue = (T)SceneObjDataManager.dataManagerInstance.GetFromObjDataDictionary(objName);
        }



        else
        {
            theStateValue = default(T);//从逻辑上不应该出现访问不存在的查询字典的情况，这个分支永远不应被执行
            print("Get value from " + DicName + ", wrong");

        }


        return theStateValue;




       
    }


    //对value设置进行了封装
    public static void SetDicStateValue<T>(string DicName, string objName, T stateValue)
    {
       


        if (DicName==SceneObjDataDicName)
        {
            SceneObjDataManager.dataManagerInstance.SetObjDataDictionary(objName, stateValue);

        }



        else
        {
            //从逻辑上不应该出现访问不存在的查询字典的情况，这个分支永远不应被执行
            print("Get value from " + DicName + ", wrong");

        }


    }






}
