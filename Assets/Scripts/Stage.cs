using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour
{
    public GameObject[] playerSpawns;
    public GameObject[] chestSpawns;

    void Start ()
    {
        GameManager.o.LoadStage();
    }
	
    void Update ()
    {
	
    }
}
