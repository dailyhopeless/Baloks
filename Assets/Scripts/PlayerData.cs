
using UnityEngine;


public enum PlayerData {
    SettingsMusic,
    SettingsSoundEffect,
    highscore,
    tempscore,
    speedsettings
}

[System.Serializable]
public struct CollectData {
    public PlayerData Data;
}