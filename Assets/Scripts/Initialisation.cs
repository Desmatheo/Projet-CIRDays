using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InitialisationPlayer : MonoBehaviour
{
    public Selection selection = new Selection();

    private GameObject player1Object;
    private GameObject player2Object;

    public GameObject player1 = null;
    public GameObject player2 = null;

    public GameObject parentPlayer;
    public GameObject[] arrayPlayer;

    public Sprite[] arraySprite;

    public HealthBar[] healthBarList;
    public UltBar[] ultBarList;

    public LayerMask[] playerLayer;
    public Transform[] wallChecks;

    public GameObject[] fleches;

    public Cinemachine.CinemachineTargetGroup cameraTargetGroup;
    public GameObject ecranVictoire;
    public GameObject[] imgVictoires;

    //public ParticleSystem[] particleSystems;
    public Transform floor;

    public int nbRoundP1 = 0;
    public int nbRoundP2 = 0;
    

    void Start()
    {
        init();
    }

    void Update(){
        CheckWin();
    }

    public void init(){
        string filepath = Application.persistentDataPath + "/SelectionSave.json";
        string data = System.IO.File.ReadAllText(filepath);
        selection = JsonUtility.FromJson<Selection>(data);

        //Player1 objet
        switch (selection.player1){
            case "Banquier":
                player1Object = arrayPlayer[0];
                break;
            case "Forgeron":
                player1Object = arrayPlayer[1];
                break;
            case "Bucheron":
                player1Object = arrayPlayer[2];
                break;
            default:
                player1Object = arrayPlayer[0];
                break;
        }

        //Player2 objet
        switch (selection.player2){
            case "Banquier":
                player2Object = arrayPlayer[0];
                break;
            case "Forgeron":
                player2Object = arrayPlayer[1];
                break;
            case "Bucheron":
                player2Object = arrayPlayer[2];
                break;
            default:
                player2Object = arrayPlayer[0];
                break;
        }

        //Maj UI
        imgVictoires[0].SetActive(false);
        imgVictoires[1].SetActive(false);
        ecranVictoire.SetActive(false);

        //Reinit Players 
        Destroy(player1);
        Destroy(player2);

        //Instantiate Players
        player1 = (GameObject)Instantiate(player1Object, new Vector3(-7, 0, 0), Quaternion.identity, parentPlayer.transform);
        player2 = (GameObject)Instantiate(player2Object, new Vector3(7, 0, 0), Quaternion.identity, parentPlayer.transform);

        //Set HealthBar
        player1.GetComponent<PlayerLife>().healthBar = healthBarList[0];
        player2.GetComponent<PlayerLife>().healthBar = healthBarList[1];

        //Set UltBar
        player1.GetComponentInChildren<PlayerAttack>().ultbar = ultBarList[0];
        player2.GetComponentInChildren<PlayerAttack>().ultbar = ultBarList[1];

        //Set TargetGroup
        cameraTargetGroup.m_Targets[0].target = player1.transform;
        cameraTargetGroup.m_Targets[1].target = player2.transform;

        //Set Mouvement dependences
        player1.GetComponent<PlayerMouvement>().enemy = player2;
        player2.GetComponent<PlayerMouvement>().enemy = player1;

        //Initialize layers for caracters
        player1.layer = LayerMask.NameToLayer("Player1");
        player2.layer = LayerMask.NameToLayer("Player2");

        //Set Jump problems dependences
        player1.GetComponentInChildren<JumpProblems>().enemy = player2;
        player1.GetComponentInChildren<JumpProblems>().self = player1;
        player2.GetComponentInChildren<JumpProblems>().enemy = player1;
        player2.GetComponentInChildren<JumpProblems>().self = player2;

        //Set Jump problems layer enemy
        player1.GetComponentInChildren<JumpProblems>().playerLayer = playerLayer[0];
        player2.GetComponentInChildren<JumpProblems>().playerLayer = playerLayer[1];

        //Set Animation flèches
        player1.GetComponent<PlayerCombat>().fleche = fleches[0].GetComponent<Animator>();
        player2.GetComponent<PlayerCombat>().fleche = fleches[1].GetComponent<Animator>();

        //Set Particle collisions
        player1.GetComponentInChildren<ParticleSystem>().collision.AddPlane(floor);
        player2.GetComponentInChildren<ParticleSystem>().collision.AddPlane(floor);

    }

    void CheckWin(){
        if ((player1.GetComponent<PlayerLife>().getHealth() <= 0) && (player2.GetComponent<PlayerLife>().getHealth() <= 0)){
            Debug.Log("tie");
        }
        else if (player1.GetComponent<PlayerLife>().isDead && Time.timeScale != 0)
        {
            ecranVictoire.SetActive(true);
            ecranVictoire.GetComponent<RoundMenu>().winnerP1 = true;
            player1.GetComponent<PlayerMouvement>().horizontal = 0;
            player2.GetComponent<PlayerMouvement>().horizontal = 0;

            player1.GetComponent<PlayerMouvement>().enabled = false;
            player2.GetComponent<PlayerMouvement>().enabled = false;

            player1.GetComponent<PlayerCombat>().enabled = false;
            player2.GetComponent<PlayerCombat>().enabled = false;

            player1.GetComponent<PlayerInput>().enabled = false;
            player2.GetComponent<PlayerInput>().enabled = false;

        }
        else if (player2.GetComponent<PlayerLife>().isDead && Time.timeScale != 0){
            ecranVictoire.SetActive(true);
            ecranVictoire.GetComponent<RoundMenu>().winnerP1 = true;
            player1.GetComponent<PlayerMouvement>().horizontal = 0;
            player2.GetComponent<PlayerMouvement>().horizontal = 0;

            player1.GetComponent<PlayerMouvement>().enabled = false;
            player2.GetComponent<PlayerMouvement>().enabled = false;

            player1.GetComponent<PlayerCombat>().enabled = false;
            player2.GetComponent<PlayerCombat>().enabled = false;

            player1.GetComponent<PlayerInput>().enabled = false;
            player2.GetComponent<PlayerInput>().enabled = false;

        }
    }

    public void setRound1(int n)
    {
        Debug.Log(n);
        nbRoundP1 = n;
    }
    public void setRound2(int n)
    {
        Debug.Log(n);
        nbRoundP2 = n;
    }

    public int getRound1()
    {
        return nbRoundP1;
    }

    public int getRound2()
    {
        return nbRoundP2;
    }

    void endGame(){
        SceneManager.LoadScene("Menu");
    }

}
