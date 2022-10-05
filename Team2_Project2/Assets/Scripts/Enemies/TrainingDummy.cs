using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour, IDamagable
{

    [SerializeField] private Animator dummyAnim;

    // Start is called before the first frame update
    void Start()
    {
        dummyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        dummyAnim.SetBool("TakeDamage", true);
    }
public void Heal(int amount)
    {

    }
}
