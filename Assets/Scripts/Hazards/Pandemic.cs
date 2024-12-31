using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pandemic : MonoBehaviour
{
    public static int NUM_SICK = 3;
    public static void makeRandomPandemic(Pi[] pis)
    {
        List<Pi> livePis = new List<Pi>();
        for (int i = 0; i < pis.Length; i++)
        {
            if (pis[i].getState() == Pi.State.Live)
            {
                livePis.Add(pis[i]);
            }
        }

        if (livePis.Count <= 3) return;

        for (int i = 0; i < NUM_SICK; i++)
        {
            Pi randomSickPi = livePis[Random.Range(0, livePis.Count)];
            randomSickPi.setStateSick();
            livePis.Remove(randomSickPi);
        }
    }

    //private static void getRandomLivePi(Pi[] pis)
    //{

    //}

}
