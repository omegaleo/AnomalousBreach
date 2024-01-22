using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    private void Start()
    {
        if (!Debug.isDebugBuild) Destroy(this.gameObject);
    }

    public void LevelUpPhishing() => Player.instance.LevelUpProfficiency(StatIdentifier.Phishing);
    public void LevelUpMalware() => Player.instance.LevelUpProfficiency(StatIdentifier.Malware);
    public void LevelUpSocialEngineering() => Player.instance.LevelUpProfficiency(StatIdentifier.Social_Engineering);
    public void LevelUpAuthExploits() => Player.instance.LevelUpProfficiency(StatIdentifier.Authentication_Exploits);
    public void LevelUpCodeInjection() => Player.instance.LevelUpProfficiency(StatIdentifier.Code_Injection);
    public void LevelUpZombieDDOS() => Player.instance.LevelUpProfficiency(StatIdentifier.Zombie_DDOS);
}
