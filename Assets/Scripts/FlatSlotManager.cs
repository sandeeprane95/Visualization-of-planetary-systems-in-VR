using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatSlotManager : MonoBehaviour {

    public static FlatSlotManager instance;

    static Vector3 step = new Vector3(0, -7, 0);
    static Vector3 location = new Vector3(30, 70, 50);
    [SerializeField]
    List<GameObject> systemFlatSlotObjs = new List<GameObject>();
    List<Vector3> coordinates = new List<Vector3> {
        location + step,
        location +step *2,
        location +step *3,
        location +step *4,
        location +step *5,
        location +step *6,
        location +step *7,
        location +step *8,
        location +step *9

    };

    int slotsFull;

    // Use this for initialization
    void Start() {
        instance = this;
    }

    public bool AreThereFreeSlots() {
        if (slotsFull < coordinates.Count -1) return true;
        else return false;
    }

    public void FreeAllSlots() {

        slotsFull = 0;
    }

    public void ManageNewSlot(GameObject slot) {
        systemFlatSlotObjs.Add(slot);
        slot.transform.position = coordinates[slotsFull];
        slotsFull++;
    }

    public void SlotWasDestroyed(string name) {
        for (int i = 0; i < systemFlatSlotObjs.Count; i++) {
            // By the time this is called the calling object still exists, so we're not looking for null, we compare names instead.
            if (systemFlatSlotObjs[i].name == name)
            {
                systemFlatSlotObjs.RemoveAt(i);
                Debug.Log("null Flat slot removed.");
            }

        }
        ReorderSlots();
        slotsFull--;
    }

    void ReorderSlots() {
        for (int i = 0; i < systemFlatSlotObjs.Count; i++) {
            var receiver = systemFlatSlotObjs[i].GetComponent<FlatSystemSlot>();
            if (receiver != null) receiver.transform.position = coordinates[i];
        }
    }
}
