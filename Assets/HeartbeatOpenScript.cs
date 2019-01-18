using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class HeartbeatOpenScript : MonoBehaviour
{
    //Collection
    public AudioClip [] Systole;
    public AudioClip [] Diastole;

    //Frequency
    public float BPM = 70f;
    [SerializeField] private float Hertz;
    [SerializeField] private float PeriodT; //1/FrequencyHertz
    [SerializeField] private float remainPeriod;
    [SerializeField] private float remainPeriodMillisec;
    public float returnTime = .01f;
    [SerializeField] private float startReturnTime;
    [SerializeField] private float startReturnTimeMillisec;

    //State
    public float [] GlobalTimer = { 0, 0 };
    public float[] CatchGlobalTimer = { 0, 0 };
    [SerializeField] private bool Lub;
    public int stateIndex = 0;
    public int isBeating;

    //Limbs
    [SerializeField] private AudioSource Speaker;
    public Toggle debugToggle;

    //Variable
    public bool useAsync; //make hearbeat multithread. after a lub, heartbeat can lub again without wait for dubb from first thread.

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();
        Hertz = BPM * 60f;
        PeriodT = 1 / Hertz;
        startReturnTime = returnTime;
        startReturnTimeMillisec = startReturnTime * 1000;
        remainPeriod = PeriodT;
        remainPeriodMillisec = remainPeriod * 1000;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < GlobalTimer.Length; i++)
        {
        }
        Hertz = BPM / 60f;
        PeriodT = 1 / Hertz; //https://www.quora.com/How-do-you-convert-Hertz-to-seconds
        if (!Lub)
        {
            remainPeriod -= Time.deltaTime;
            remainPeriodMillisec -= Time.deltaTime * 1000;
            startReturnTime = returnTime;
            startReturnTimeMillisec = returnTime * 1000;
            if(remainPeriodMillisec <= 0f)
            {
                stateIndex = 1;
                if(debugToggle) debugToggle.isOn = true;
                if (Systole.Length > 0)
                {
                    for (int i = 0; i < Systole.Length; i++)
                    {
                        Speaker.PlayOneShot(Systole[i]);
                    }
                }
                Lub = true;
            }
        } else
        {
            remainPeriod = PeriodT;
            remainPeriodMillisec = PeriodT * 1000;
            startReturnTime -= Time.deltaTime;
            startReturnTimeMillisec -= Time.deltaTime * 1000;
            if(startReturnTimeMillisec <= 0f)
            {
                stateIndex = 0;
                if(debugToggle) debugToggle.isOn = false;
                if (Diastole.Length > 0)
                {
                    for (int i = 0; i < Diastole.Length; i++)
                    {
                        Speaker.PlayOneShot(Diastole[i]);
                    }
                }
                Lub = false;
            }
        }


    }
}
