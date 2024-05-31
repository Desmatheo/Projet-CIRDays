using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAttack : MonoBehaviour
{
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public int attackDamage = 25;
    public int ultimeLimit = 50;
    float ultimeValue = 0;

    public float timerAnimation = 0;
    public float animationSpeed;
    public float testValeur = 0.2f;
    public float typeDegat;

    public GameObject enemy;
    public GameObject player;
    public UltBar ultbar;


    private void Start()
    {
        ultbar.SetMaxUlt(ultimeLimit);
    }

    private void Update()
    {
        ultbar.SetUlt(ultimeValue);
        UpdateTestValeur();
        UpdateSpeedAnimation();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "HurtBox" && Time.time >= nextAttackTime)
        {
            float degat = player.GetComponent<PlayerCombat>().nbDegat;
            float reductionDegat = collision.gameObject.GetComponentInParent<PlayerCombat>().reductionDamage;
            typeDegat = player.GetComponent<PlayerCombat>().typeDegat;
            animationSpeed = player.GetComponent<PlayerCombat>().vitesseAnimation;

            ultimeValue += 5 * reductionDegat * typeDegat;
            nextAttackTime = Time.time + 1f / attackRate;
            enemy = collision.gameObject;
            float distance = enemy.transform.position.x - player.transform.position.x;
            timerAnimation = Time.time;
            
            collision.gameObject.GetComponentInParent<PlayerLife>().TakeDamage(degat * reductionDegat, animationSpeed, distance);
            enemy.GetComponentInParent<PlayerCombat>().canAttack = false;
        }
    }

    public void UpdateTestValeur()
    {
        if (typeDegat == 0.5f)
        {
            testValeur = 0.13f;
        }else if(typeDegat == 0.6f)
        {
            testValeur = 0.15f;
        }else if(typeDegat == 0.8f)
        {
            testValeur = 0.20f;
        }else if(typeDegat == 0.9f)
        {
            testValeur = 0.25f;
        }else if(typeDegat == 1f)
        {
            testValeur = 0.3f;
        }
    }

    public void UpdateSpeedAnimation()
    {
        if (enemy != null)
        {
            if (Time.time - timerAnimation > animationSpeed * testValeur && enemy.GetComponentInParent<Animator>().speed != 1)
            {
                enemy.GetComponentInParent<Animator>().speed = 1;
                enemy.GetComponentInParent<PlayerCombat>().canAttack = true;
            }
        }
    }

    public bool canIUseUlt()
    {
        return ultimeValue >= ultimeLimit;
    }

    public void setUltValue(int value)
    {
        ultimeValue = value;
    }
}
