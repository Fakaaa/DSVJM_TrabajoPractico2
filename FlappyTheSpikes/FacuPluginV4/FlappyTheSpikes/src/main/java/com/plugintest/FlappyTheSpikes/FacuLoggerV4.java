package com.plugintest.FlappyTheSpikes;

import android.util.Log;

public class FacuLoggerV4 {

    public static final FacuLoggerV4 _instance = new FacuLoggerV4();
    public  static final String LOGTAG = "FacuPluginV4";

    public static FacuLoggerV4 GetInstance() {
        return _instance;
    }

    private long startTime;

    private  FacuLoggerV4()
    {
        Log.i(LOGTAG , "Created plugin.");
        startTime = System.currentTimeMillis();
    }

    public  double GetElapsedTime()
    {
        return  (System.currentTimeMillis()-startTime) / 1000.0f;
    }

    public  void SendLog(String msg){
        Log.d(LOGTAG, msg);
    }

}
