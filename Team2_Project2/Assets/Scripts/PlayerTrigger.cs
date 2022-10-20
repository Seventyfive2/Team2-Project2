using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent triggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.name == "Player")
        {
            if (triggered != null)
            {
                triggered.Invoke();
            }

            GetComponent<Collider>().enabled = false;
        }
    }
}
