using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CameraScript : MonoBehaviour, Sensor {

    bool jogando;
    bool movendo;
    bool chegou;

    private float max_x;
    private float max_y;
    private float min_x;
    private float min_y;

    Vector3 ponto_final = new Vector3(-18, 0, -20);

    Vector3 mouse_lastpos;
   



    // -18, 0

	// Use this for initialization
	void Start () {
	    
        chegou = true;
        FaseAdm.getAdm().AddSensor(this);
        FaseAdm.getAdm().set_Camera(this);

        max_x = 10.4f;
        max_y = 24.9f;
        min_x = -46;
        min_y = -24.9f;


	}
	
	// Update is called once per frame
	void Update () {
		


        if (!chegou)
        {
            transform.position = Vector3.MoveTowards(transform.position, ponto_final, 20 * Time.deltaTime);
            if (transform.position == ponto_final)
                chegou = true;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y,ponto_final.z), 5 * Time.deltaTime);
            return;
        }

        if (!jogando)
            return;
        

        if (movendo)
        {
            Vector3 mouse_pos = Input.mousePosition;
            Vector3 novo = transform.position + (mouse_pos - mouse_lastpos) * (-0.15f);
            mouse_lastpos = mouse_pos;

            if (novo.x < min_x)
                novo.x = min_x;
            else if (novo.x > max_x)
                novo.x = max_x;

            if (novo.y < min_y)
                novo.y = min_y;
            else if (novo.y > max_y)
                novo.y = max_y;

            transform.position = novo;
        }

        ChecarClick();
	}

    public void jogando_mudou(bool jog)
    {
        jogando = jog;

        chegou = false;

        if (jogando)
        {
            ponto_final = new Vector3(-18, 0, -20);
        }
        else
        { 
            ponto_final = new Vector3(0, 0, -10);
        }
    }

    public void resize(int i)
    {
        if (!jogando)
            return;

        if (i > 0 && transform.localScale.x == 2)
            return;

        if (i < 0 && transform.localScale.x == 0.5f)
            return;


        max_x += i * -11.5f;
        max_y += i * -9.3f;
        min_x += i * 11.5f;
        min_y += i * 6.95f;

        GetComponent<Camera>().orthographicSize += i * 10;

        transform.localScale += new Vector3(i * 0.5f, i * 0.5f);

        Vector3 novo = transform.position;

        if (novo.x < min_x)
            novo.x = min_x;
        else if (novo.x > max_x)
            novo.x = max_x;

        if (novo.y < min_y)
            novo.y = min_y;
        else if (novo.y > max_y)
            novo.y = max_x;

        transform.position = novo;

    }


    public void reset()
    {
        if (transform.position.z == -80)
            return;

        GetComponent<Camera>().orthographicSize = 20;

        transform.localScale = new Vector3(1, 1, 1);

        max_x = 10.4f;
        max_y = 24.9f;
        min_x = -46;
        min_y = -24.9f;


    }

    // - 11.5   -3

    // -23 *0.5  18.6 * 0.5
    // - 11.5   + 9.3

    // - 23    6.3
    // - 34.5  15.6
    // - 46    24.9
    // - 57.5   34.2


    // +11.5   - 6.95

    // - 12.6  -13
    // -1.1   - 18.95
    // 10.4  -24.9


    void ChecarClick(){

        if (Input.GetMouseButtonDown(0))
        {
            movendo = true;
            mouse_lastpos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            movendo = false;

        }

    }



}
