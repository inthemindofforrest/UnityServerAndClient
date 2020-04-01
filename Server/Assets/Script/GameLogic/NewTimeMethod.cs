using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTimeMethod : MonoBehaviour
{
    public System.DateTime ServerStart;
    public System.DateTime TimeSinceStart;

    private void Start()
    {
        Time.timeScale = 52.0f;
        ServerStart = System.DateTime.Now;
    }

    private void Update()
    {
        Debug.Log("Hours: " + TimeInHoursSinceServerStart());
        Debug.Log("Miniutes: " + TimeInMinutesSinceServerStart());
    }

    public int TimeInHoursSinceServerStart()
    {
        System.TimeSpan Temptime = System.DateTime.Now - ServerStart;
        return Temptime.Hours;
    }
    public int TimeInMinutesSinceServerStart()
    {
        System.TimeSpan Temptime = System.DateTime.Now - ServerStart;
        return Temptime.Minutes;
    }
}
