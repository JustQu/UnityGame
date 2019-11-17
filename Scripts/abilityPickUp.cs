using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityPickUp : MonoBehaviour
{
    public Ability ability;
    public AbilityCoolDown aSlot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            aSlot.ability = ability;
        }
    }
}
