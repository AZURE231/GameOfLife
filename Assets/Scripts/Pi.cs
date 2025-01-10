using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Pi : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public enum State
    {
        Live,
        Die,
        Hurt,
        Sick
    }

    private State state = State.Die;
    private Image image;
    public int hurtCycleCount = 0;
    public int sickCycleCount = 0;
    private void Awake()
    {
        this.image = GetComponent<Image>();
        DOTween.Init();
    }

    public void updatePiPerCycle()
    {
        switch (this.state)
        {
            default:
            case State.Die:
                sickCycleCount = 0;
                hurtCycleCount = 0;
                break;
            case State.Live:
                sickCycleCount = 0;
                hurtCycleCount = 0;
                this.image.color = new Color(1f, 1f, 1f);
                break;
            case State.Hurt:
                hurtCycleCount++;
                this.image.color = new Color(1f, 0f, 0f);
                break;
            case State.Sick:
                sickCycleCount++;
                this.image.color = new Color(0f, 1f, 0f);
                break;
        }
    }

    public void setStateDie()
    {
        image.DOColor(Color.black, (1 / GameManager.instance.speed) / 2);
        transform.localScale = new Vector3(1f, 1f, 1f);
        this.state = State.Die;
    }

    public void setStateLive()
    {
        transform.DOShakeScale((1 / GameManager.instance.speed) / 2, .4f, 5);
        this.state = State.Live;
    }

    public void setStateHurt()
    {
        hurtCycleCount = 0;
        this.state = State.Hurt;
    }

    public void setStateSick()
    {
        Debug.Log("sick");
        this.state = State.Sick;
        transform.DOShakeScale((1 / GameManager.instance.speed) / 1.8f, .4f, 5);
    }

    public State getState()
    {
        return state;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.isEditing = true;
        setStateLive();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.isEditing = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.instance.isEditing)
        {
            setStateLive();
        }
    }

    private void toggleState()
    {
        if (state == State.Die)
        {
            state = State.Live;
        }
        else
        {
            state = State.Die;
        }
    }


}
