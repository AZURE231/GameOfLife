using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class Laser
{
    public const int EFFECT_CYCLE_TIME = 2;

    public static void makeRandomLaser(Pi[] pis)
    {
        if (Random.Range(0, 2) == 0)
        {
            laserRow(pis);
        }
        else
        {
            laserCol(pis);
        }
        Camera.main.DOShakePosition((1 / GameManager.instance.speed) / 2, .05f, 5);
        SoundManager.instance.Play("laser");
    }

    private static void laserRow(Pi[] pis)
    {
        int randomRowToLaser = getRandomIndex();

        for (int i = 0; i < 50; i++)
        {
            int index = randomRowToLaser * 50 + i;
            pis[index].setStateHurt();
        }
    }

    private static void laserCol(Pi[] pis)
    {
        int randomColToLaser = getRandomIndex();

        for (int i = 0; i < 50; i++)
        {
            int index = i * 50 + randomColToLaser;
            pis[index].setStateHurt();
        }
    }

    private static int getRandomIndex()
    {
        return Random.Range(0, 50);
    }
}
