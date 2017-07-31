using System.Collections.Generic;
using UnityEngine;

public static class GameManager {

    private static List<Player> players;
    private static List<TowerController> towercontrollers;

    public static void initialize() {

        towercontrollers = new List<TowerController>();

        foreach (TowerController TC in Object.FindObjectsOfType<TowerController>()) {
            TC.initialize();
        }

        players = new List<Player>();

        foreach (Player P in Object.FindObjectsOfType<Player>()) {
            P.initialize();
        }
    }

    public static void update() {
        foreach (Player P in players) {
            P.update();
        }
    }

    public static void checkWin () {
        //winning condition => win
       
    }

    private static void win() {
        Debug.Log("WIN!");
    }

    public static void addPlayer(Player player) {
        Debug.Log(player.transform.name);
        players.Add(player);
    }

    public static void addTower(TowerController tower) {
        Debug.Log("ADD");
        towercontrollers.Add(tower);
    }

    public static Player getPlayer() {
        return players[0];
    }

    public static Player getPlayer(int index) {
        return players[index];
    }

    public static Vector3 getPlayerPosition() {
        return players[0].transform.position;
    }

    public static Vector3 getPlayerPosition(int index) {
        return players[index].transform.position;
    }

    public static TowerController getTowerController()
    {
        Debug.Log(towercontrollers.Count);
        return towercontrollers[0];
    }

    public static TowerController getTowerController(int index)
    {
        return towercontrollers[index];
    }
}
