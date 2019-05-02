using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class AutoMover : MonoBehaviour, Sensor {

    public float velx;
    public float vely;

    private Vector3 alvo;

    private float timer;

    public static bool play = false;

    void Start()
    {
        FaseAdm.getAdm().AddSensor(this);
        alvo = new Vector3(velx, vely, 0);
        timer = 0;
    }

    void Update()
    {
        if (play)
        {
            transform.position += alvo * Time.deltaTime;
            timer += Time.deltaTime;

            if (timer > 5)
                Destroy(gameObject);
        }

    }

    public void jogando_mudou(bool jog)
    {
        play = !jog;
    }



}