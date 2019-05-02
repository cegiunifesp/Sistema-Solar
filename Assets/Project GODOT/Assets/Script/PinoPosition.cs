using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PinoPosition : MonoBehaviour, Sensor {

    public string planeta;
    private bool jogando;

    private GameObject ocupado;

	// Use this for initialization
	void Start () {
		
        ocupado = null;
        jogando = false;

        FaseAdm.getAdm().AddSensor(this);
	}
	
	// Update is called once per frame
	void Update () {
		

        if (jogando)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            transform.localScale = new Vector3(1.5f, 1.5f);
        }

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (jogando)
            return;
        
        if (ocupado != null || Planeta.planeta_atual == null)
            return;

        if (other.gameObject.name == planeta)
        {
            Planeta p = other.gameObject.GetComponent<Planeta>();
            if (p.acertou((Vector2)transform.position))
                ocupado = other.gameObject;
        }
        transform.localScale = transform.localScale + other.transform.GetChild(0).transform.localScale * 3;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (jogando)
            return;
        
        transform.localScale = new Vector3(1.5f, 1.5f);
        Planeta p = other.gameObject.GetComponent<Planeta>();
        if (p.errou((Vector2)transform.position))
            ocupado = null;
    }

    void OnMouseEnter()
    {
        if (jogando)
            return;
        
        if(ocupado == null && Planeta.planeta_atual == null)
            transform.localScale = new Vector3(3, 3);
    }

    void OnMouseExit()
    {
        if (jogando)
            return;
        
        if (ocupado == null && Planeta.planeta_atual == null)
            transform.localScale = new Vector3(1.5f, 1.5f);
    }

    public void jogando_mudou(bool jog)
    {
        jogando = jog;

        if (jogando)
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            ocupado = null;
        }
    }


}
