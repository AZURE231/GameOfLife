using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isPlaying = false;
    public bool isEditing = false;
    public float speed = 5f;

    public int cycle = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
