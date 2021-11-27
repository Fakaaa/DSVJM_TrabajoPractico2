using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacuLoggerPlug : MonoBehaviour
{
    const string pathName = "com.plugintest.FlappyTheSpikes";
    const string loggerClassName = "FacuLoggerV4";

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

    public void InitPlugin()
    {
        InitJavaClass();
        InitInstanceObj();
    }

    private void Start()
    {
        InitPlugin();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 5)
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
        AndroidJavaClass unityJava = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityJava.GetStatic<AndroidJavaObject>("currentActivity");
        Debug.Log("Activity " + activity.ToString());
        FLogger.SetStatic("activity", activity);
    }

    float elapsedTime = 0;
    double GetElapsedTime()
    {
        if (Application.platform == RuntimePlatform.Android)
            return FLoggerInstance.Call<double>("GetElapsedTime");
        GameManager.Instance.gmReference.ParseCommandLineOnConsole("Trying access on method at WrongPlatform" + gameObject.name);
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

    public void SaveRecordScore(int value)
    {
        if (FLoggerInstance == null)
        {
            InitJavaClass();
            InitInstanceObj();
        }

        Debug.Log("Saving new record score with native plugin for android!");
        FLoggerInstance.Call("SaveScore", value);
    }

    public void SaveLogLine(string logLine)
    {
        if (FLoggerInstance == null)
        {
            InitJavaClass();
            InitInstanceObj();
        }

        FLoggerInstance.Call("SaveLog", logLine);
    }

    public int GetSavedScore()
    {
        if (FLoggerInstance == null)
        {
            InitJavaClass();
            InitInstanceObj();
        }

        Debug.Log("Getting record score from file opened with native plugin for android!");
        return FLoggerInstance.Call<int>("GetScore");
    }

    public string GetSavedLogLine()
    {
        if (FLoggerInstance == null)
        {
            InitJavaClass();
            InitInstanceObj();
        }

        return FLoggerInstance.Call<string>("GetLogFromFile");
    }
}
