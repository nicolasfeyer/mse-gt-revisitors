using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllCheckpoint : MonoBehaviour
{
    List<Checkpoint> checkpoints;

    private void Awake() {
        checkpoints = GetComponentsInChildren<Checkpoint>().ToList();
        InitCheckpoints();
    }

    public List<Checkpoint> GetCheckpoints() {
        return checkpoints;
    }

    public Checkpoint GetCheckpoint(int id) {
        return checkpoints[id];
    }

    private void InitCheckpoints() {
        int counter = 1;
        bool firstDone = false;

        checkpoints.ForEach(c => {
            if(c.isStartingPoint) {
                if(firstDone) {
                    Debug.LogError("Multiple stating checkpoints");
                    c.Id = counter;
                    counter++;
                }
                else {
                    c.Id = 0;
                    firstDone = true;
                }
            }
            else {
                c.Id = counter;
                counter++;
            }
        });

        
        checkpoints.Sort(delegate (Checkpoint x, Checkpoint y) {
            return x.Id.CompareTo(y.Id);
        });
    }
}
