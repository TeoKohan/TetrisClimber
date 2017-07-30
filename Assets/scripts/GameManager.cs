using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private List<Transform> milestones;
    [SerializeField]
    private Transform victoryGoal;


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
        //tbd winning condition
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public List<Transform> GetMilestones()
    {
        return milestones;
    }

}
