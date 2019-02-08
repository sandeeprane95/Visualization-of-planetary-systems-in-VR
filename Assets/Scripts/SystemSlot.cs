using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class SystemSlot : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            SlotManager.instance.ManageNewSlot(this.gameObject);
        }

        private void OnDestroy()
        {
            SlotManager.instance.SlotWasDestroyed(gameObject);
        }

        public void MoveTo(Vector3 pos)
        {
            transform.position = pos;
        }
    }
