using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Timer 
{

    public float TimeLeft { get; private set; }

    public delegate void TimerDelegate();
    public TimerDelegate TimerCallback;
    

    public void SetTimer(float time)
    {
        if (time > 0.0f)
            TimeLeft = time;
        else
            Debug.Log("Time is less than 0");
    }

    public IEnumerator Tick()    {
        
        while(TimeLeft > 0.0f)
        {            
            TimeLeft -= Time.deltaTime;
            yield return null;
        }

        TimerCallback.Invoke();

    }
}
