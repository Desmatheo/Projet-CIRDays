using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public GameObject player;

    public GameObject particulesDegat;

    float nextAttackTime = 0f;
    public float attackRate = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox" && Time.time >= nextAttackTime)
        {
            //if (player.GetComponent<PlayerMouvement>().IsFacingRight())
            //{
            //    particulesDegat.GetComponent<Transform>().rotation = Quaternion.Euler(0f,90f,0f);
            //}
            //else
            //{
            //    particulesDegat.GetComponent<Transform>().rotation = Quaternion.Euler(180f, 90f,0f);
            //}

            var em = particulesDegat.GetComponent<ParticleSystem>().emission;

            em.enabled = true;
            particulesDegat.GetComponent<ParticleSystem>().Play();


            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
}
