using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class Explosion
{
    const int EXPLOSION_RADIUS = 7;
    public const int EFFECT_CYCLE_TIME = 3;

    public static void makeRandomExplosion(Pi[] pis)
    {
        int randomIndexToExplose = getRandomIndex();

        int rowOrigin = randomIndexToExplose / 50;
        int colOrigin = randomIndexToExplose % 50;
        for (int i = -EXPLOSION_RADIUS; i <= EXPLOSION_RADIUS; i++)
        {
            for (int j = -EXPLOSION_RADIUS; j <= EXPLOSION_RADIUS; j++)
            {
                int dieCellIndexRow = rowOrigin + i;
                int dieCellIndexCol = colOrigin + j;
                if (dieCellIndexRow >= 0 && dieCellIndexCol >= 0 && dieCellIndexCol < 50 && dieCellIndexRow < 50)
                {
                    int dieCellIndex = dieCellIndexRow * 50 + dieCellIndexCol;
                    pis[dieCellIndex].setStateHurt();
                }
            }
        }

        Camera.main.DOShakeRotation((1 / GameManager.instance.speed) / 2, .1f, 10);
        SoundManager.instance.Play("explode");
    }

    private static int getRandomIndex()
    {
        return Random.Range(0, 50 * 50);
    }
}
