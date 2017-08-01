using System.Collections.Generic;

public class IDManager {

    private Queue<int> availableIDs;
    private int highestID;

    public void initialize() {
        availableIDs = new Queue<int>();
        highestID = 0;
    }

    public int getID() {
        if (availableIDs.Count > 0) {
            return availableIDs.Dequeue();
        }
        return highestID++;
    }

    public void returnID(int id) {
        availableIDs.Enqueue(id);
    }

    public void reset() {
        highestID = 0;
        availableIDs.Clear();
    }
}
