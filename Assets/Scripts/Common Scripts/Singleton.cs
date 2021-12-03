using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                /*
                if (instance == null) {
                    Debug.LogError (typeof(T) + "is nothing");
                }
                */

                if (instance == null)
                {
                    instance = new GameObject("SingletonMonoBehaviour<" + typeof(T).Name + ">").AddComponent<T>();
                }
                /*
                */
            }

            return instance;
        }
    }

    // シーン切り替えで死なないフラグ
    [SerializeField]
    public bool _DontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        if (CheckInstance())
        {
            if (_DontDestroyOnLoad)
            {
                // シーン切替で死なない
                DontDestroyOnLoad(this.gameObject);
            }
            Init();
        }
    }

    // 初期化関数、派生クラスでAwakeは使わずこっちをつかうこと
    protected virtual void Init() { }

    protected bool CheckInstance()
    {
        /*
        if( this == Instance){ return true;}
        Destroy(this);
        return false;
        */

        if (instance != null)
        {
            if (this == instance) { return true; }
            GameObject obj = this.gameObject;
            Destroy(this);
            Destroy(obj);
            return false;
        }

        instance = this as T;

        return true;
    }

    /*
    // 強制インスタンス削除
    public void InstanceDestroy()
    {
        Destroy(instance);
    }
    */

    static public bool IsAlive()
    {
        return instance != null;
    }
}