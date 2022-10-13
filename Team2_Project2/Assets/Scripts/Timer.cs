using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityEvent OnTimerFinished;

    [SerializeField] private float timer = 60;
    private float time;

    public void Start()
    {
        time = timer;
    }

    public void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        if(time < 0)
        {
            if (OnTimerFinished != null)
            {
                OnTimerFinished.Invoke();
            }
        }
    }
}
