using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBulletScript : MonoBehaviour
{
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 20 *Time.deltaTime);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log(transform.name + ": ENEMY HIT!");
    //        // do some damage
    //        collision.gameObject.GetComponent<enemySoldierAI>().TakeDamage(damage);
    //        Destroy(this.gameObject);
    //    }
    //    else if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log(transform.name + ": PLAYER HIT!");
    //        collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
    //        Destroy(this.gameObject);
    //    }
    //    else
    //    {
    //        Debug.Log(transform.name + ": WALL HIT");
    //        Destroy(this.gameObject);
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log(transform.name + ": ENEMY HIT!");
            // do some damage
            other.GetComponent<enemySoldierAI>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            Debug.Log(transform.name + ": PLAYER HIT!");
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log(transform.name + ": WALL HIT");
            Destroy(this.gameObject);
        }
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }
}
