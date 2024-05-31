using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RoundMenu : MonoBehaviour
{
    public GameObject sauvegarde;
    public bool winnerP1;
    public Image[] test;
    public Image[] imgRoundsP2;

    public GameObject[] imgVictoire;

    int nbRoundP1 = 0;
    int nbRoundP2 = 0;

    private void Start()
    {
        nbRoundP1 = sauvegarde.GetComponent<InitialisationPlayer>().getRound1();
        nbRoundP2 = sauvegarde.GetComponent<InitialisationPlayer>().getRound2();
        if (winnerP1)
        {
            imgVictoire[0].SetActive(true);
            test[nbRoundP1].color = new Color(255, 194, 0);
            nbRoundP1++;
            sauvegarde.GetComponent<InitialisationPlayer>().setRound1(nbRoundP1);
            if (nbRoundP1 >= 2)
            {
                SceneManager.LoadScene("Menu");
            }
        }
        else
        {
            imgVictoire[1].SetActive(true);
            imgRoundsP2[nbRoundP2].color = new Color(255, 194, 0);
            nbRoundP2++;
            sauvegarde.GetComponent<InitialisationPlayer>().setRound2(nbRoundP2);
            if (nbRoundP2 >= 2)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    public void lauchNewRound(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            sauvegarde.GetComponent<InitialisationPlayer>().init();
        }
    }

}
