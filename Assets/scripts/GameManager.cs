using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField]
    protected Transform player;
    


    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Win ()
    {
        //winning condition
        Debug.Log("WIN!");
    }

    public Transform GetPlayer()
    {
        return player;
    }
}
