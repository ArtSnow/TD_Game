using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMovement : MonoBehaviour
{
    [SerializeField] private Animator UI_Top;
    [SerializeField] private Animator UI_Bottom;
    [SerializeField] private Animator UI_Stats;
    [SerializeField] private Animator UI_Opponent;


    public void Hide()
    {
        UI_Top.SetTrigger("Hide");
        UI_Bottom.SetTrigger("Hide");
        UI_Opponent.SetTrigger("Hide");
        UI_Stats.SetTrigger("Hide");
    }

    public void Show()
    {
        UI_Top.SetTrigger("Normal");
        UI_Bottom.SetTrigger("Normal");
        UI_Opponent.SetTrigger("Normal");
        UI_Stats.SetTrigger("Normal");
    }
}
