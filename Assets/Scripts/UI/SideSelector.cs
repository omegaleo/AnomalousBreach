using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class SideSelector : MonoBehaviour
{
    public void CyberSec()
    {
        AudioManager.instance.Play("Click");
        Player.instance.SetSide(PlayerSide.Defend);
        Destroy(this.gameObject);
    }

    public void Hacker()
    {
        AudioManager.instance.Play("Click");
        Player.instance.SetSide(PlayerSide.Attack);
        Destroy(this.gameObject);
    }
}
