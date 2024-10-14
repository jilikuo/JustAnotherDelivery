using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TimeSystem : MonoBehaviour, ISaveable
{
    public BoolVariable isGamePausedRef; // Not owned by time system

    public float dayStartHour = 8f;
    public float dayEndHour = 18f;
    [SerializeField] private float framesPerHour = 60f;
    public TimeObject currentTime;
    public BoolVariable isTimeStopped;
    public UnityEvent timeoutEvent = new UnityEvent();

    [SerializeField] private float stopTime = 0f;
    [SerializeField] private bool hasTimer = false;

    private float dayDuration = 0f;
    public void Save(GameData gameData)
    {
        gameData.timeSystemData = new List<string>();
        var data = gameData.timeSystemData;
        ISaveable.AddKey(data, "dayStartHour", dayStartHour);
        ISaveable.AddKey(data, "dayEndHour", dayEndHour);
        ISaveable.AddKey(data, "framesPerHour", framesPerHour);
        ISaveable.AddKey(data, "currentTime", currentTime.GetTime());
        ISaveable.AddKey(data, "isTimeStopped", isTimeStopped.value);
        ISaveable.AddKey(data, "stopTime", stopTime);
        ISaveable.AddKey(data, "hasTimer", hasTimer);
        ISaveable.AddKey(data, "dayDuration", dayDuration);
    }

    public bool Load(GameData gameData)
    {
        foreach (var key_value in gameData.timeSystemData)
        {
            var parsed = ISaveable.ParseKey(key_value);
            string key = parsed[0];
            string value = parsed[1];
            //Debug.Log("Loading key: " + key + " value: " + value);
            switch (key)
            {
                case "dayStartHour":
                    dayStartHour = (float) Convert.ToDouble(value);
                    break;
                case "dayEndHour":
                    dayEndHour = (float) Convert.ToDouble(value);
                    break;
                case "framesPerHour":
                    framesPerHour = (float) Convert.ToDouble(value);
                    break;
                case "currentTime":
                    currentTime.SetTime((float) Convert.ToDouble(value));
                    break;
                case "isTimeStopped":
                    isTimeStopped.value = Convert.ToBoolean(value);
                    break;
                case "stopTime":
                    stopTime = (float) Convert.ToDouble(value);
                    break;
                case "hasTimer":
                    hasTimer = Convert.ToBoolean(value);
                    break;
            }
        }

        return true;
    }

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("TimeSystem");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        isTimeStopped.value = true; // Start in stopped state
        dayDuration = dayEndHour - dayStartHour;
    }

    void Update()
    {
        if (isGamePausedRef.value || isTimeStopped.value) return;
        float time = currentTime.GetTime() + Time.deltaTime / framesPerHour;
        if (hasTimer && (time > stopTime))
        {
            time = stopTime;
            isTimeStopped.value = true;
            hasTimer = false;
            timeoutEvent.Invoke();
        }
        currentTime.SetTime(time);
    }

    public void StartTime()
    {
        isTimeStopped.value = false;
    }

    public void StopTime()
    {
        isTimeStopped.value = true;
    }

    public void StopTimeAt(float time)
    {
        stopTime = time;
        hasTimer = true;
    }

    public void StopTimeAfter(float timeInc)
    {
        stopTime = currentTime.GetTime() + timeInc;
        hasTimer = true;
    }

    public void SetTime(float newTime)
    {
        currentTime.SetTime(newTime);
    }

    public void StartNextDay()
    {
        StopTime();
        currentTime.SetNextDay(dayStartHour);
        StopTimeAfter(dayDuration);
        StartTime();
    }

    public void RestartDay()
    {
        StopTime();
        currentTime.RestartDay(dayStartHour);
        StopTimeAfter(dayDuration);
        StartTime();
    }
}
