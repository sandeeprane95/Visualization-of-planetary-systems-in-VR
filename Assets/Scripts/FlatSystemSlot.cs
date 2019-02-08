using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatSystemSlot : MonoBehaviour {

    // Use this for initialization
    void Start() {
        FlatSlotManager.instance.ManageNewSlot(this.gameObject);
    }

    private void OnDestroy() {
        FlatSlotManager.instance.SlotWasDestroyed(gameObject.name);
    }

    public void MoveTo(Vector3 pos) {
        gameObject.transform.position = pos;
    }

    void MoveDelayed(Vector3 pos) {
        
    }
}
