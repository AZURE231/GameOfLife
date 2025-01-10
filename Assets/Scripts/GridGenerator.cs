using System;
using UnityEngine;


public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject piPrefab;
    [SerializeField] private Transform gridContainer;

    private Pi[] pis = new Pi[ROW * COL];
    private Pi.State[] nextStateList = new Pi.State[ROW * COL];
    private float drawTimer;
    private float drawTimerMax = 1f;

    const int ROW = 50;
    const int COL = 50;

    public event EventHandler OnLose;

    void Start()
    {
        this.clearGrid();
        generatePiList();
        initEmptyNewGridPi();
    }

    private void generatePiList()
    {
        for (int i = 0; i < ROW * COL; i++)
        {
            GameObject newPi = Instantiate(piPrefab, gridContainer);
            pis[i] = newPi.GetComponent<Pi>();
        }
    }

    public void initRandomNewGridPi()
    {
        for (int i = 0; i < ROW * COL; i++)
        {
            if (UnityEngine.Random.Range(0, 5) == 1)
            {
                pis[i].setStateLive();
            }
            else
            {
                pis[i].setStateDie();
            }
        }
        drawPis();
    }

    public void initEmptyNewGridPi()
    {
        for (int i = 0; i < ROW * COL; i++)
        {
            pis[i].setStateDie();
        }
        drawPis();
    }

    private void clearGrid()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (!GameManager.instance) return;
        if (!GameManager.instance.isPlaying) return;
        drawTimer -= Time.deltaTime;
        if (drawTimer <= 0)
        {
            checkLose();
            GameManager.instance.cycle++;
            if (GameManager.instance.isBombing) createExplosion();
            if (GameManager.instance.isLasering) createLaser();
            updateGridPi();
            if (GameManager.instance.isPandemic) createPandemic();
            drawTimer = drawTimerMax;
        }
    }

    private void checkLose()
    {
        if (isLose())
        {
            OnLose?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool isLose()
    {
        int numLivePi = 0;
        foreach (Pi pi in pis)
        {
            if (pi.getState() == Pi.State.Live) numLivePi++;
        }
        return numLivePi == 0;
    }

    private void updateGridPi()
    {
        for (int i = 0; i < pis.Length; i++)
        {
            int numNeighbor = countNeighbor(i);
            bool isSick = hasSickNeighbor(i);
            determineNextStateList(i, numNeighbor, isSick);
        }
        for (int i = 0; i < pis.Length; i++)
        {
            if (this.nextStateList[i] == Pi.State.Live)
            {
                this.pis[i].setStateLive();
            }
            else if (this.nextStateList[i] == Pi.State.Die)
            {
                this.pis[i].setStateDie();
            }
            else if (this.nextStateList[i] == Pi.State.Sick)
            {
                this.pis[i].setStateSick();
            }
        }
        drawPis();

    }

    private void drawPis()
    {
        for (int i = 0; i < pis.Length; i++)
        {
            this.pis[i].updatePiPerCycle();
        }
    }

    private void createExplosion()
    {
        if (UnityEngine.Random.Range(0, 10) == 0)
        {
            Explosion.makeRandomExplosion(pis);
        }
    }

    private void createLaser()
    {
        if (UnityEngine.Random.Range(0, 3) == 0)
        {
            Laser.makeRandomLaser(pis);
        }
    }

    private void createPandemic()
    {

        Pandemic.makeRandomPandemic(pis);

    }

    private int countNeighbor(int index)
    {
        int numNeighbor = 0;
        int row = index / COL;
        int col = index % COL;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int neighborRow = row + i;
                int neighborCol = col + j;

                if (neighborCol >= 0 && neighborRow >= 0 && neighborCol < COL && neighborRow < ROW)
                {
                    int neighborIndex = neighborRow * COL + neighborCol;

                    if (pis[neighborIndex].getState() == Pi.State.Live || pis[neighborIndex].getState() == Pi.State.Sick)
                    {
                        numNeighbor++;
                    }
                }
            }
        }

        return numNeighbor;
    }

    private bool hasSickNeighbor(int index)
    {
        int row = index / COL;
        int col = index % COL;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int neighborRow = row + i;
                int neighborCol = col + j;

                if (neighborCol >= 0 && neighborRow >= 0 && neighborCol < COL && neighborRow < ROW)
                {
                    int neighborIndex = neighborRow * COL + neighborCol;

                    if (pis[neighborIndex].getState() == Pi.State.Sick)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void determineNextStateList(int index, int numNeighbor, bool isSick)
    {
        // Live cell rules
        if (this.pis[index].getState() == Pi.State.Live || this.pis[index].getState() == Pi.State.Sick)
        {
            if (numNeighbor == 2 || numNeighbor == 3)
            {
                // Cell keep alive or sick if have 2 or 3 neighbor
                if (this.pis[index].getState() == Pi.State.Sick)
                {
                    // If pi sick for 3 cycle, it die
                    if (this.pis[index].sickCycleCount == 3)
                        this.nextStateList[index] = Pi.State.Die;
                    else this.nextStateList[index] = Pi.State.Sick;
                }
                else this.nextStateList[index] = Pi.State.Live;
            }
            else
            {
                // Otherwise it dies
                this.nextStateList[index] = Pi.State.Die;
            }

            if (isSick && this.nextStateList[index] == Pi.State.Live)
            {
                this.nextStateList[index] = Pi.State.Sick;
            }

        }
        else if (this.pis[index].getState() == Pi.State.Die)
        {
            if (numNeighbor == 3)
            {
                // Dead cell become alive if have 3 neighbor
                this.nextStateList[index] = Pi.State.Live;
            }
            else
            {
                // Dead cell remain dead
                this.nextStateList[index] = Pi.State.Die;
            }
        }
        else if (this.pis[index].getState() == Pi.State.Hurt)
        {
            this.nextStateList[index] = Pi.State.Hurt;
            // If hurt for 3 cycle, it dies
            if (this.pis[index].hurtCycleCount == 3) this.nextStateList[index] = Pi.State.Die;
        }
    }

    public void updateDrawTime(float sliderValue)
    {
        drawTimerMax = 1 / sliderValue;
    }

}

