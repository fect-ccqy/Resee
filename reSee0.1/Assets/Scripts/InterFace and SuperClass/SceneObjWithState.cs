using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//场景中的物体，封装了对状态的操作（懒狗专用父类，用来少打点代码）
public abstract class SceneObjWithState<T>: InteractiveObj
{

    [SerializeField] protected SceneManager theSceneManager;

    //初始化，若已存在key，则不进行更改
    protected int InitAddDicKeyStateValue(T stateValue)
    {

        return DicDataReader.InitAddDicKeyStateValue<T>(DicDataReader.SceneObjDataDicName, objName, stateValue);

    }

    //从dic中获取状态变量
    protected T GetDicStateValue()
    {
        return DicDataReader.GetDicStateValue<T>(DicDataReader.SceneObjDataDicName, objName);
    }

    //设置dic中的状态变量
    protected void SetDicStateValue(T stateValue)
    {
        DicDataReader.SetDicStateValue<T>(DicDataReader.SceneObjDataDicName, objName, stateValue);
    }


}
