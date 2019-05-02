using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class FaseAdm : MonoBehaviour {

    private List<Sensor> ListaSensores;
    private int acertos;
    private float velTempo;
    private float deltaTempo;
    private bool jogando;
    const int MAX = 2;
    private CameraScript cam;

    private static FaseAdm Adm;


    public AudioClip anda_porta;
    public AudioClip bub_acerto;
    public AudioClip bub_erro;
    public AudioClip click_but;
    public AudioClip giro_som;
    public AudioClip reset_som;

	// Use this for initialization
	void Awake () {

        if (Adm != null)
        {
            if (Adm != this)
                Destroy(this);
        }
        else
            Adm = this;
        

        jogando = false;

        RandomPlanetas();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0) && acertos == 8 && !jogando)
        {
            StartJogo();
            Adm.anda_porta = null;
        }
        else if (jogando && deltaTempo != 0)
        {
            deltaTempo += MAX * Time.deltaTime;
            if (deltaTempo < 0)
                velTempo += MAX + deltaTempo;
            else
                velTempo += -MAX + deltaTempo;

            if (deltaTempo == 0)
                deltaTempo = 0.01f;

            if (velTempo < 1 || deltaTempo > MAX)
            {
                deltaTempo = 0;
                velTempo = 1;
            }

        }
	}

    public static FaseAdm getAdm()
    {
        if (Adm == null)
        {
            Adm = GameObject.Find("SistemaSolar").AddComponent<FaseAdm>();
        }

        return Adm;

    }

    public static float getTempo()
    {
        return Adm.velTempo;
    }

    public static void zoomable(int i)
    {
        Adm.cam.resize(i);
    }

    public static void acelera(int i)
    {
        Adm.velTempo += i * 0.25f;

        if (Adm.velTempo > 2)
            Adm.velTempo = 2;
        else if (Adm.velTempo < 0)
            Adm.velTempo = 0;
    }


    public static void restart()
    {
        Adm.EndJogo();

        if (Adm.anda_porta != null)
        {
            AudioSource.PlayClipAtPoint(Adm.anda_porta, Camera.main.transform.position);
        }
    }

    public static void toca(int i)
    {
        switch(i){
            case 0:
                Adm.toca_certo();
                break;
            case 1:
                Adm.toca_errado();
                break;
            case 2:
                Adm.toca_click();
                break;
            case 3:
                Adm.toca_giro();
                break;
            case 4:
                Adm.toca_reset();
                break;
            }
    }


    private void toca_certo()
    {
        AudioSource.PlayClipAtPoint(bub_acerto, Camera.main.transform.position);
    }

    private void toca_errado()
    {
        AudioSource.PlayClipAtPoint(bub_erro, Camera.main.transform.position);
    }

    private void toca_click()
    {
        AudioSource.PlayClipAtPoint(click_but, Camera.main.transform.position);
    }

    private void toca_giro()
    {
        AudioSource.PlayClipAtPoint(giro_som, Camera.main.transform.position);
    }

    private void toca_reset()
    {
        AudioSource.PlayClipAtPoint(reset_som, Camera.main.transform.position);
        cam.reset();
    }

    public void numAcertos(int a)
    {
        acertos += a;

    }


    public void AddSensor(Sensor s)
    {

        if (ListaSensores == null)
            ListaSensores = new List<Sensor>();

        if(!ListaSensores.Contains(s))
            ListaSensores.Add(s);

    }

    public void set_Camera(CameraScript cs)
    {
        cam = cs;
    }
        

    private void StartJogo()
    {

        Adm.toca_giro();

        foreach (Sensor s in ListaSensores)
            s.jogando_mudou(true);

        velTempo = 1;
        deltaTempo = -MAX;
        jogando = true;
    }

    private void EndJogo()
    {

        Adm.toca_reset();

        foreach (Sensor s in ListaSensores)
            s.jogando_mudou(false);

        RandomPlanetas();

        jogando = false;
        acertos = 0;
    }

    private void RandomPlanetas()
    {
        List<Vector3> posicoes = new List<Vector3>();

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Planeta"))
        {
            Planeta p = go.GetComponent<Planeta>();

            posicoes.Add(p.get_init());
        }
            

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Planeta"))
        {
            Planeta p = go.GetComponent<Planeta>();

            int i = Random.Range(0, posicoes.Count);

            p.set_init(posicoes[i]);

            posicoes.RemoveAt(i);
        }
    }
}
