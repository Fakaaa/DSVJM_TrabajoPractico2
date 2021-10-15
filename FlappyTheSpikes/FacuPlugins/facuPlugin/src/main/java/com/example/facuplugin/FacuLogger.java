package com.example.facuplugin;

import android.util.Log;

public class FacuLogger {

    public static final FacuLogger _instance = new FacuLogger();

    public static FacuLogger GetInstance() {
        return _instance;
    }

    public  void SendLog(String msg){
        Log.d("RL=>", msg);
    }
}