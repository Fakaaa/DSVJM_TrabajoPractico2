using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacuLoggerPlug : MonoBehaviour
{
    const string pathName = "com.FacundoPonce.FlappyTheRythm";
    const string loggerClassName = "FacuLogger";

    static AndroidJavaClass fLoggerClass = null;
    static AndroidJavaObject fLoggerInstance = null;

    public static AndroidJavaClass FLogger
    {
        get
        {
            if (fLoggerClass != null)
                return fLoggerClass;
            else
            {
                InitJavaClass();
                return fLoggerClass;
            }
        }
    }

    public static AndroidJavaObject FLoggerInstance
    {
        get
        {
            if (fLoggerInstance != null)
                return fLoggerInstance;
            else
            {
                InitInstanceObj();
                return fLoggerInstance;
            }
        }
    }

    void Start()
    {
        Debug.Log("Elapsed time: " + GetElapsedTime());
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= 5)
        {
            elapsedTime -= 5;
            GameManager.Instance.gmReference.ParseCommandLineOnConsole("Tick: " + GetElapsedTime());
        }
    }

    static void InitJavaClass()
    {
        fLoggerClass = new AndroidJavaClass(pathName + "." + loggerClassName);
    }

    static void InitInstanceObj()
    {
        fLoggerInstance = FLogger.CallStatic<AndroidJavaObject>("GetInstance");
    }

    float elapsedTime = 0;
    double GetElapsedTime()
    {
        if (Application.platform == RuntimePlatform.Android)
            return FLoggerInstance.Call<double>("GetElapsedTime");
        GameManager.Instance.gmReference.ParseCommandLineOnConsole("Trying access on method at WrongPlatform"+ gameObject.name);
        return 0;
    }

    public static void SendLog(string msg)
    {
        if (FLoggerInstance == null)
        {
            InitJavaClass();
            InitInstanceObj();
        }

        FLoggerInstance.Call("SendLog", msg);
    }
}
