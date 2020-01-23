using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type { ICE, GRASS, WATER, DARK, PSYCHIC, FAIRY, ROCK, GROUND, FIGHTING, FLYING, BUG, DRAGON, POISON, GHOST, FIRE, ELECTRIC, NORMAL, STEEL }

public class Pokemon : MonoBehaviour
{
    [SerializeField] private string pokemonName;
    public string Name { get { return pokemonName; } set { pokemonName = value; } }
    [SerializeField] private int lvl;
    public int Level { get { return lvl; } set { lvl = value; } }
    [SerializeField] private int hp;
    public int HitPoints { get { return hp; } set { hp = value; } }
    [SerializeField] private int maxHp;
    public int MaxHitPoints { get { return maxHp; } set { maxHp = value; } }   
    [SerializeField] private int atk;
    public int Attack { get { return atk; } set { atk = value; } }
    [SerializeField] private int def;
    public int Defense { get { return def; } set { def = value; } }
    [SerializeField] private int spAtk;
    public int SpAtk { get { return spAtk; } set { spAtk = value; } }
    [SerializeField] private int spDef;
    public int SpDef { get { return spDef; } set { spDef = value; } }
    [SerializeField] private int spe;
    public int Speed { get { return spe; } set { spe = value; } }
    [SerializeField] private int exp;
    public int Exp { get { return exp; } set { exp = value; } }
    [SerializeField] private int expToEarn;
    public int ExpToEarn { get { return expToEarn; } set { expToEarn = value; } }

    [SerializeField] private List<Move> moves = new List<Move>();
    public List<Move> Moves { get { return moves; } set { moves = value; } }

    public List<type> pokeType;
    public List<type> quadWeaknesses;
    public List<type> weaknesses;
    public List<type> immunities;
    public List<type> resistances;
    public List<type> quadResistances;
}
        