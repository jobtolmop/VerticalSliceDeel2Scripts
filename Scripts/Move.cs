using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move")]
public class Move : ScriptableObject
{
    [SerializeField] private string moveName;
    public string MoveName { get { return moveName; } }
    [SerializeField] private int powerPoints;
    public int PowerPoints { get { return powerPoints; } }
    [SerializeField] private int basePower;
    public int BasePower { get { return basePower; } }
    [SerializeField] private int accuracy;
    public int Accuracy { get { return accuracy; } }
    [SerializeField] private float critChance;
    public float CritChance { get { return critChance; } }

    //public enum type {ICE, GRASS, WATER, DARK, PSYCHIC, FAIRY, ROCK, GROUND, FIGHTING, FLYING, BUG, DRAGON, POISON, GHOST, FIRE, ELECTRIC, NORMAL, STEEL}

    public type moveType;
    [SerializeField] private bool physical = true;
    public bool Physical { get { return physical; } }

    [SerializeField] private int statusChance = 0;
    public int Status { get { return statusChance; } }

    public enum statusType {NONE, SLEEP, PARALYSIS, POISONED, BURNED, FROZEN, CONFUSED, FLINCHED, TRAPPED}
    public statusType typeOfStatus;
}
