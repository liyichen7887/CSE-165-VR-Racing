using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_control : MonoBehaviour {

    public GameObject finger_tip;
    public GameObject other_finger_tip;
    public GameObject finger_joint;
    public loader load;
    public int current_checkpt;
    public Material line_mt;
    public Material line_mt_2;
    private LineRenderer lineRenderer;
    private GameObject target;
    private int checkpt_count;
    public Text countdown;
    public Text timer;
    private float countdown_timer;
    private float race_timer;
    public Text panelty_txt;
    private bool panelty;
    private float panelty_timer;
    private AudioSource clock_audio;
    private AudioSource checkpt_audio;
    private AudioSource crash_audio;
    private AudioSource cheer_audio;
    private AudioSource engine_audio;
    private bool played;
    private bool finished;
    private List<GameObject> lines_list;
    private bool line_gen;
    private bool blue_line_gen;
    public GameObject plane;

    // Use this for initialization
    void Start () {
        //old_pos = this.transform.position;
        current_checkpt =  2;
        target = new GameObject("target");
        lineRenderer = target.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(line_mt);
        lineRenderer.widthMultiplier = 1.0f;
        countdown_timer = 8.0f;
        race_timer = 0.0f;
        panelty = false;
        panelty_timer = 0.0f;
        clock_audio = GetComponents<AudioSource>()[0];
        checkpt_audio = GetComponents<AudioSource>()[1];
        crash_audio = GetComponents<AudioSource>()[2];
        cheer_audio = GetComponents<AudioSource>()[3];
        engine_audio = GetComponents<AudioSource>()[4];
        played = false;
        finished = false;
        lines_list = new List<GameObject>();
        line_gen = false;
        blue_line_gen = true;
    }
	
	// Update is called once per frame
	void Update () {
        countdown_timer -= Time.deltaTime;
        if (countdown_timer > 0.0) {
            countdown.text = "Countdown: " + countdown_timer;
            this.transform.LookAt(load.checkPoints[current_checkpt - 1].transform);

            if (line_gen == false)
            {
                for (int i = 1; i < load.checkPoints.Count; i++)
                {
                    GameObject obj = new GameObject("line");
                    LineRenderer lr = obj.AddComponent<LineRenderer>();
                    lr.material = new Material(line_mt_2);
                    lr.widthMultiplier = 1.0f;
                    lr.positionCount = 2;
                    lr.SetPosition(0, load.checkPoints[i - 1].transform.position);
                    lr.SetPosition(1, load.checkPoints[i].transform.position);
                    lines_list.Add(obj);
                }
                line_gen = true;
            }
            
            //plane.transform.position = load.checkPoints[0].transform.position + finger_tip.transform.forward * 5;
            if (Mathf.Abs(Vector3.Dot(Camera.main.transform.forward, new Vector3(0.0f, 1.0f, 0.0f)) / (Camera.main.transform.forward.magnitude * new Vector3(0.0f, 1.0f, 0.0f).magnitude)) >= Mathf.Cos((10 * Mathf.PI) / 180))
            {

                Debug.Log("test");
                foreach (GameObject line in lines_list)
                {
                    line.GetComponent<LineRenderer>().positionCount = 0;
                    
                }
                lineRenderer.positionCount = 0;
                blue_line_gen = false;
            }
        }
        else
        {
            if (current_checkpt <= load.checkPoints.Count)
            {
                if (played == false)
                {
                    clock_audio.loop = true;
                    clock_audio.Play();
                    played = true;
                    engine_audio.loop = true;
                    engine_audio.Play();
                }
               
                countdown.text = "Start!!!";
                race_timer += Time.deltaTime;
                timer.text = "Timer: " + race_timer;

                if (panelty == true) {
                    panelty_timer+= Time.deltaTime;
                    if (panelty_timer > 3.0f)
                    {
                        panelty = false;
                        panelty_timer = 0.0f;
                    }
                    panelty_txt.text = "Panelty Timer: "+ panelty_timer;
                }
                else
                {
                    float v = 20/(Vector3.Distance(other_finger_tip.transform.position, finger_tip.transform.position)+0.10f);

                    float audio_speed = 80.0f;
                    float p = v / audio_speed;
                    engine_audio.pitch = Mathf.Clamp(p, 0.1f, 4.0f);

                    //Debug.Log(v);
                    panelty_txt.text = "";
                    this.transform.position = this.transform.position + (finger_tip.transform.position - finger_joint.transform.position) * v;
                    //plane.transform.position = this.transform.position + finger_tip.transform.forward*5;
                    //plane.transform.position.z += 0.5f;
                    if (this.transform.position.y <= 0.0)
                    {
                        panelty = true;
                        this.transform.position = load.checkPoints[current_checkpt - 2].transform.position;
                        //Debug.Log("3");
                        crash_audio.Play();
                    }

                }
                
            }
            else {
                countdown.text = "Finish";
                timer.text = "Timer: " + race_timer;
                if(finished == false)
                {
                    clock_audio.Stop();
                    cheer_audio.Play();
                    engine_audio.Stop();
                    finished = true;
                }
                
            }
            
        }

        
        
        if (blue_line_gen == true)
        {
            //this.transform.LookAt(load.checkPoints[current_checkpt - 1].transform);
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, load.checkPoints[current_checkpt - 1].transform.position);
        }
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "checkpt(Clone)")
        {
            
            load.checkPoints[current_checkpt-1].GetComponent<SphereCollider>().enabled = false;
            load.checkPoints[current_checkpt - 1].GetComponent<MeshRenderer>().material = line_mt;
            load.checkPoints[current_checkpt - 1].GetComponent<AudioSource>().Stop();
            current_checkpt++;
            load.checkPoints[current_checkpt-1].GetComponent<SphereCollider>().enabled = true;
            load.checkPoints[current_checkpt - 1].GetComponent<AudioSource>().loop = true;
            load.checkPoints[current_checkpt - 1].GetComponent<AudioSource>().Play();
            checkpt_audio.Play();
            //Debug.Log("1");
        }
        else
        {
            panelty = true;
            this.transform.position = load.checkPoints[current_checkpt - 2].transform.position;
            //Debug.Log("2");
            crash_audio.Play();
        }
        
    }
}
