using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Botao : MonoBehaviour, Sensor {

    //Game settings
    public bool zoomin;
    public bool zoomout;
    public bool acel;
    public bool freia;
    public bool restart;
    public bool oneclick;

    Vector3 scale_filho_fin = new Vector3(1, 1, 1);
    Vector3 scale_filho_ini = new Vector3(0, 0, 1);
    Vector3 scale_filho_del = new Vector3(1, 1, 0);

    Transform filho;

    bool jogando;

    void Start()
    {
        FaseAdm.getAdm().AddSensor(this);

        filho = transform.GetChild(0);

        jogando = false;
    }


    void Update()
    {
        if (!jogando)
        {
            if (filho.localScale != scale_filho_ini)
            {
                filho.localScale -= scale_filho_del * Time.deltaTime;
                if (filho.localScale.x < 0)
                    filho.localScale = scale_filho_ini;
            }
        }
        else
        {
            if (filho.localScale != scale_filho_fin)
            {
                filho.localScale += scale_filho_del * Time.deltaTime;
                if (filho.localScale.x > 1)
                    filho.localScale = scale_filho_fin;
            }

        }
    }

    public void jogando_mudou(bool jog)
    {
        jogando = jog;
    }

    void OnMouseDown()
    {
        FaseAdm.toca(2);

        if (zoomin)
            FaseAdm.zoomable(1);
        else if (zoomout)
            FaseAdm.zoomable(-1);
        else if (acel)
            FaseAdm.acelera(1);
        else if (freia)
            FaseAdm.acelera(-1);
        else if (restart)
            FaseAdm.restart();

        if (oneclick)
            Destroy(gameObject);
    }


}
