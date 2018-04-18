using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class loader : MonoBehaviour {

    public TextAsset coordFile;
    public GameObject checkPointPrefab;
    private float scale = 0.0254f;
    public List<GameObject> checkPoints;
    public static int totalNumCheckPoint = 0;
    public GameObject player;
    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start()
    {
        checkPoints = new List<GameObject>();

        string file = coordFile.text;
        string[] lines = file.Split('\n');
        string[] xyz = new string[3];
        int counter = 0;
        foreach (string next_line in lines)
        {
            ++counter;
            xyz = next_line.Split(' ');
            Vector3 coord = new Vector3(Int32.Parse(xyz[0]) * scale, Int32.Parse(xyz[1]) * scale, Int32.Parse(xyz[2]) * scale);
            GameObject new_checkpt = Instantiate(checkPointPrefab) as GameObject;
            new_checkpt.transform.position = coord;
            if (counter == 1)
            {
                player.transform.position = coord;
                

            }
            if (counter == 2)
            {
                new_checkpt.GetComponent<SphereCollider>().enabled = true;
                new_checkpt.GetComponent<AudioSource>().loop = true;
                new_checkpt.GetComponent<AudioSource>().Play();
            }
            else {
                new_checkpt.GetComponent<SphereCollider>().enabled = false;
            }

            checkPoints.Add(new_checkpt);

            
        }
    }
	// Update is called once per frame
	void Update () {
		
	}


}
