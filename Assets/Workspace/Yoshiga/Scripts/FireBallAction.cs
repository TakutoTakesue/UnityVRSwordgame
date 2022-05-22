using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAction : MonoBehaviour
{
    [Header("攻撃力 : int")]
    [SerializeField] private int power;
    [Header("爆発エフェクト : Object")]
    [SerializeField] private GameObject ExpEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerScript>().OnDamage(power);
        }
        Instantiate(ExpEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
