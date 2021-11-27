package com.plugintest.FlappyTheSpikes;

import java.io.File;

import android.app.Activity;
import android.util.Log;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;

public class FacuLoggerV4 {

    public static final FacuLoggerV4 _instance = new FacuLoggerV4();
    public  static final String LOGTAG = "FacuPluginV4";
    public static Activity activity;

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

    public  void SaveLog(String msg)
    {
        File pathWhereSave = activity.getFilesDir();
        File fileCreated = new File(pathWhereSave, "savedLogs.text");
        try
        {
            FileOutputStream stream = new FileOutputStream(fileCreated);
            try
            {
                stream.write(msg.getBytes());
            }
            finally
            {
                stream.close();
            }
        }
        catch (IOException exp)
        {
            Log.e("Exception","Failed to save log line, File couldnt be saved" + exp.toString());
        }
    }

    public void SaveScore(int val)
    {
        File pathWhereSave = activity.getFilesDir();
        File fileCreated = new File(pathWhereSave, "recordScore.dat");

        Log.i("Saving:","RecordScore:" + val);

        try
        {
            FileOutputStream stream = new FileOutputStream(fileCreated);
            try
            {
                stream.write(Integer.toString(val).getBytes());
            }
            finally
            {
                stream.close();
            }
        }
        catch (IOException exp)
        {
            Log.e("Exception","Failed to save record score, File couldnt be saved" + exp.toString());
        }
    }

    public String GetLogFromFile()
    {
        File pathWhereSearch = activity.getFilesDir();
        File file = new File(pathWhereSearch, "savedLogs.text");
        if(!file.exists())
            return  null;

        int lenght = (int)file.length();
        byte[] bytes = new byte[lenght];

        try
        {
            FileInputStream stream = new FileInputStream(file);
            try
            {
                stream.read(bytes);
            }
            finally
            {
                stream.close();
            }
        }
        catch(IOException exp)
        {
            Log.e("Exception","Error getting logs from file savedLogs.text" + exp.toString());
        }
        String logsGetedFromFile = new String(bytes);
        return  logsGetedFromFile;
    }

    public  int GetScore()
    {
        File pathWhereSearch = activity.getFilesDir();
        File file = new File(pathWhereSearch, "recordScore.dat");
        if(!file.exists())
            return  0;

        int lenght = (int)file.length();
        byte[] bytes = new byte[lenght];

        try
        {
            FileInputStream stream = new FileInputStream(file);
            try
            {
                stream.read(bytes);
            }
            finally
            {
                stream.close();
            }
        }
        catch(IOException exp)
        {
            Log.e("Exception","Error getting scores from file recordScore.dat" + exp.toString());
        }
        String recordScoreGetted = new String(bytes);

        int returnedScore = Integer.parseInt(recordScoreGetted);
        Log.i("Loading:","RecordScore:" + returnedScore);

        return  returnedScore;
    }
}