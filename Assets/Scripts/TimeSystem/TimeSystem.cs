using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField] private float dayStartHour = 8f;
    [SerializeField] private float framesPerHour = 60f;
    public TimeObject currentTime;
    public BoolVariable isGamePaused;

    private float time = 0f;
    private float stopTime = 0f; // Start in stopped state

    // Start is called before the first frame update
    void Start()
    {
        currentTime.Set(dayStartHour);
        time = currentTime.GetTime();
    }

    // Update is called once per frame
    void Update()
    {
        if ((isGamePaused.value) || (time == stopTime)) return;
        time += Time.deltaTime / framesPerHour;
        if (time > stopTime) time = stopTime;
        currentTime.Set(time);
    }

    void StartTime()
    {
        stopTime = float.MaxValue;
    }

    void StopTime()
    {
        stopTime = time;
    }

    void StopTimeAt(float time)
    {
        stopTime = time;
    }

    void StopTimeAfter(float timeInc)
    {
        stopTime = time + timeInc;
    }
}
