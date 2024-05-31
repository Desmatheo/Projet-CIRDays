using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Threading;
using UnityEngine.Networking;


public class PlayerCombat : MonoBehaviour
{
    //Animators pour la flèche bourse
    public Animator animator;
    public Animator fleche;

    //Variables pour l'ult
    private float previusValue = 0;
    private float actualValue = 0;
    private float diffValue;
    public bool isUltOn = false;

    //Valeurs pour les attaques
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    float nextGuardTime = 0f;
    float maxHoldGarde = 0f;
    public int[] tabCoup;
    float timerSignature = 0f;
    public int[] attaquesSpéciales;
    public bool outOfGuard = false;

    //Valeurs pour changer dêgats infligés et reçu
    public float nbDegat = 1f;
    public float typeDegat = 1f;
    public float reductionDamage = 1f;

    //Valeurs pour la bourse
    float bourseValue;
    float timeBourse = 0;
    public string symbol;

    public float vitesseAnimation;
    public bool canAttack = true;
    public int typeCoup;

    private void Start()
    {
        tabCoup = new int[3];
        tabCoup[0] = 0;
        tabCoup[1] = 0;
        tabCoup[2] = 0;
        canAttack = true;
        isUltOn = false;
    }
    void Update(){
        StartCoroutine(Bourse());
        UpdateGarde();
    }

    IEnumerator Bourse(){
        if (Time.time >= timeBourse){
            timeBourse = Time.time + 15f;
            var client = new HttpClient();
            var url = "https://www.boursorama.com/bourse/action/graph/ws/UpdateCharts?symbol=" + symbol + "&period=-1";
            
            // Récupération des données boursières

            using (UnityWebRequest request = UnityWebRequest.Get(url)) {
                yield return request.SendWebRequest();
                var responseString = request.downloadHandler.text;
                BourseResponse bourseResponse = JsonUtility.FromJson<BourseResponse>(responseString);
                bourseValue = bourseResponse.d[0].c;
            }
            previusValue = actualValue;
            actualValue = bourseValue;
            diffValue = actualValue - previusValue;
            if (diffValue > 0){
                fleche.SetFloat("State", 2);
            }
            else if (diffValue < 0){
                fleche.SetFloat("State", -2);
            }
            else if (diffValue == 0){
                fleche.SetFloat("State", 0);
            }
        }
    }
    
    public void PetitPoing(InputAction.CallbackContext ctx){
        if (Time.time >= nextAttackTime || outOfGuard) {
            if (ctx.action.triggered && canAttack){
                if (GetComponent<PlayerMouvement>().IsGrounded()){

                    checkTypeOfHit(1, 5, 1f, 0.5f, "Petit_poing", 4f);
                    typeCoup = 0;
                }
                else
                {
                    setStats(8, 1.6f, 0.8f, "Gros_poing", 3.25f);
                    typeCoup = 1;
                }
            }
            outOfGuard = false;
        }
    }

    public void PetitPied(InputAction.CallbackContext ctx){
        if (Time.time >= nextAttackTime || outOfGuard) {
            if (ctx.action.triggered && canAttack)
            {
                if (GetComponent<PlayerMouvement>().IsGrounded())
                {
                    checkTypeOfHit(2, 5, 1.2f, 0.6f, "Petit_pied", 3.75f);
                    typeCoup = 0;
                }
                else
                {
                    setStats(8, 1.8f, 0.8f, "Gros_pied", 3.25f);
                    typeCoup = 1;
                }
            }
            outOfGuard = false;
        }
    }

    public void GrosPoing(InputAction.CallbackContext ctx){
        if (Time.time >= nextAttackTime || outOfGuard) {
            if (ctx.action.triggered && canAttack)
            {
                checkTypeOfHit(3, 10, 1.6f, 0.9f, "Gros_poing",2.75f);
                typeCoup = 2;
            }
            outOfGuard = false;
        }
    }

    public void GrosPied(InputAction.CallbackContext ctx){
        if (Time.time >= nextAttackTime || outOfGuard) {
            if (ctx.action.triggered && canAttack)
            {
                checkTypeOfHit(4, 10, 1.8f, 1f, "Gros_pied", 2.75f);
                typeCoup = 2;
            }
            outOfGuard = false;
        }
    }

    public void Garde(InputAction.CallbackContext ctx)
    {
        if (Time.time >= nextGuardTime){
            if (ctx.action.triggered){
                animator.SetBool("Garde", true);
                maxHoldGarde = Time.time;
                reductionDamage = 0.1f;
                nextGuardTime = Time.time + 1f / attackRate * 2f;
                outOfGuard = true;
            }
        }
    }

    public void UpdateGarde()
    {
        if (animator.GetBool("Garde"))
        {
            if (Input.GetKeyUp("x") || getJoystickButton() || Time.time - maxHoldGarde > 1.5f) // demander mathéo pour manette
            {
                animator.SetBool("Garde", false);
                reductionDamage = 1f;
            }
        }
    }

    public bool getJoystickButton()
    {
        if (Input.GetKeyUp(KeyCode.JoystickButton4) || Input.GetKeyUp(KeyCode.JoystickButton5)) {
            return true;
        }
        return false;
    }

    public void Ultime(InputAction.CallbackContext ctx){
        if (Time.time >= nextAttackTime) {
            if (ctx.action.triggered && (gameObject.GetComponentInChildren<PlayerAttack>().canIUseUlt())){
                if (diffValue == 0){
                    nbDegat = 50f * 1f;
                }
                else if (diffValue > 0){
                    nbDegat = 50f * 1.3f;
                }
                else if (diffValue < 0){
                    nbDegat = 50f * 0.7f;
                }

                animator.SetTrigger("Ultime");
                gameObject.GetComponentInChildren<PlayerAttack>().setUltValue(0);
            }
        }
    }

    public float getDamage(){
        return nbDegat;
    }

    public bool checkSignature(int valeurCoup)
    {
        for(int i = 0; i < 3; i++)
        {
            if (tabCoup[i] == 0)
            {
                if (i != 0){
                    if(Time.time - timerSignature > 4f) {
                        tabCoup[0] = valeurCoup;
                        tabCoup[1] = 0;
                        tabCoup[2] = 0;
                        timerSignature = Time.time;
                        return false; // Dans le cas ou la distance de temps est trop grande, reset le tableau
                    }  
                }
                tabCoup[i] = valeurCoup;
                timerSignature = Time.time;
                if(i == 2) {
                    ComboValide(); //fonction qui vérifie si le coup est valide et l'effectue
                }
                return false;
            }
            else //Cas ou tableau plein et j'ai déjà effectué la signature
            {
                if(i == 2 && tabCoup[i] !=0)
                {
                    tabCoup[0] = valeurCoup;
                    tabCoup[1] = 0;
                    tabCoup[2] = 0;
                }
            }
        }
        return false;
    }

    bool ComboValide(){
        int tmp = 0;
        for (int i = 0; i < 3; i++){
            tmp += tabCoup[i] * (int)Math.Pow(10, i);
        }
        for (int i = 1; i < 12; i +=4){
            if (tmp == attaquesSpéciales[i]){
                nbDegat = attaquesSpéciales[i+1];
                animator.SetTrigger("Signature" + attaquesSpéciales[i-1].ToString());
                typeDegat = 0.1f * attaquesSpéciales[i+2];
                return true;
            }
        }
        tabCoup[0] = tabCoup[1];
        tabCoup[1] = tabCoup[2];
        tabCoup[2] = 0;
        return false;
    }


    public void setStats(float degats, float vitesseAttaque, float DegatModifier, string nomAttaque,float animationSpeed)
    {
        animator.SetTrigger(nomAttaque);
        nbDegat = degats;
        nextAttackTime = Time.time + 1f / attackRate * vitesseAttaque;
        typeDegat = DegatModifier;
        vitesseAnimation = animationSpeed;
    }

    public void checkTypeOfHit(int typeDeCoup, float nbDegats, float vitesseAttaque, float degatModifier, string nomAttaque, float animationSpeed)
    {
        if (checkSignature(typeDeCoup))
        {
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else
        {
            setStats(nbDegats, vitesseAttaque, degatModifier, nomAttaque, animationSpeed);
        }
    }
}

[System.Serializable]
public class Quote
{
    public long d;
    public float o;
    public float h;
    public float l;
    public float c;
    public int v;
}

[System.Serializable]
public class DataItem
{
    public float o;
    public float h;
    public float l;
    public float c;
    public int v;
    public float var;
    public Quote[] qt;
}

[System.Serializable]
public class BourseResponse
{
    public DataItem[] d;
}
