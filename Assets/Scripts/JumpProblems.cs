using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class JumpProblems : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject enemy;
    public GameObject self;
    public GameObject playerCheck;
    public LayerMask playerLayer;
    [SerializeField] private Transform[] tab;
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float distanceTest = 4f;


    public float pushForce = 0.5f;
    public float sizeCircle = 0.6f;
    public float backpushForce = 0.5f;
    public float margeErreur = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        tab = new Transform[3];
        tab[0] = playerCheck.transform;
        tab[1] = playerCheck.transform.GetChild(0);
        tab[2] = playerCheck.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        reactIfEnemyBelow();
    }

    void reactIfEnemyBelow()
    {
        foreach (Transform check in tab)
        {
            pushEnemy(check);
        }
    }

    void pushEnemy(Transform valeur)
    {
        if (checkEnemyBelow(valeur, sizeCircle) && enemy.GetComponent<PlayerMouvement>().IsGrounded() && checkPosCondition())
        {
            enemy.transform.position = new Vector2(enemy.transform.position.x + ValueToAdd(valeur, 1f), enemy.transform.position.y);
        }
    }

    bool checkEnemyBelow(Transform objet, float circonference)
    {
        if (rb.velocity.y < 0)
        {
            return Physics2D.OverlapCircle(objet.position, circonference, playerLayer);
        }
        return false;
    }

    bool checkPosCondition()
    {
        return self.transform.position.y - enemy.transform.position.y > 1f && Physics2D.OverlapCircle(tab[0].position, margeErreur, playerLayer) || rb.velocity.x != 0;

    }

    float ValueToAdd(Transform valeur, float valeurPoussage)
    {
        float distance = enemy.transform.position.x - transform.position.x;
        float value = 1f;
        if (valeur.tag == "L")
        {
            value = 0.5f;
        }
        if (valeur.tag == "LL")
        {
            value = 0.25f;
        }
        float puissance = Mathf.Abs(pushForce);
        if (distance <= 0)
        {
            puissance = -puissance;
        }
        if (IsThereAWall())
        {
            puissance = -puissance;
        }

        return puissance * value;
    }

    public bool IsThereAWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, sizeCircle, wallLayer);
    }
}
