using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public float dayStartHour = 8f;
    public float dayEndHour = 18f;
    [SerializeField] private float framesPerHour = 60f;
    public TimeObject currentTime;
    public BoolVariable isGamePaused;
    public BoolVariable isTimeStopped;

    [SerializeField] private float time = 0f;
    [SerializeField] private float stopTime = 0f;
    [SerializeField] private bool hasTimer = false;

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
        currentTime.SetTime(dayStartHour);
        time = currentTime.GetTime();
    }

    void Update()
    {
        if (isGamePaused.value || isTimeStopped.value) return;
        time += Time.deltaTime / framesPerHour;
        if (hasTimer && (time > stopTime))
        {
            time = stopTime;
            isTimeStopped.value = true;
            hasTimer = false;
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
        stopTime = time + timeInc;
        hasTimer = true;
    }

    public void SetTime(float newTime)
    {
        time = newTime;
        currentTime.SetTime(time);
    }

    public void StartNextDay()
    {
        currentTime.SetNextDay(dayStartHour);
        time = currentTime.GetTime();
    }
}
