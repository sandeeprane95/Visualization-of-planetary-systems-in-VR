using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour {

    public static SlotManager instance;

    static Vector3 step = new Vector3(0, -30, 0);
    static Vector3 location = new Vector3(0, 0, 0);

    List<GameObject> systemSlotObjs = new List<GameObject>();
    List<Vector3> coordinates = new List<Vector3> {
        location + step,
        location + step *2,
        location + step *3,
        location + step *4,
        location + step *5

    };

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool AreThereFreeSlots() {
        if (systemSlotObjs.Count < coordinates.Count-1) return true;
        else return false;
    }

    public void FreeAllSlots() {

        systemSlotObjs.Clear();
    }

    public void ManageNewSlot(GameObject slot) { 
        var receiver = slot;
        receiver.transform.position = (coordinates[systemSlotObjs.Count]);
        string msg = string.Format("Managing new slot, slotsFull: {0}, systemSlotObjs: {1}", systemSlotObjs.Count, systemSlotObjs.Count);
        systemSlotObjs.Add(slot);
        Debug.Log(msg);
        //ReorderSlots();
    }

    public void SlotWasDestroyed(GameObject name) {
        Debug.Log("Slot was destroyed.");
        systemSlotObjs.Remove(name);
        ReorderSlots();
    }

    void ReorderSlots() {
        Debug.Log("Reordering slots.");
        for (int i = 0; i < systemSlotObjs.Count; i++) {
            var receiver = systemSlotObjs[i];
            if (receiver != null) receiver.transform.position = coordinates[i];
        }
    }

    IEnumerator LateMessage(SystemSlot obj, string message, Vector3 coordinates) {
        yield return new WaitForSeconds(.01f);
        obj.SendMessage(message, coordinates);
    }


}
