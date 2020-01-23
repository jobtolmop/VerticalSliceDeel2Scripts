using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

    public BattleState state;

    [SerializeField] private List<GameObject> pokemonInBattle = new List<GameObject>();
    [SerializeField] private List<GameObject> turnOrder = new List<GameObject>();
    public List<GameObject> TurnOrder { get { return turnOrder; } set { turnOrder = value; } }

    private BattleUI bUI;

    private Pokemon currPokemon;
    private Move currMove;
    private float effective;
    private bool didCrit;

    public int ChosenMove { get; set; } = 0;

    void Start()
    {
        bUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<BattleUI>();
        state = BattleState.START;
        pokemonInBattle.AddRange(GameObject.FindGameObjectsWithTag("Pokemon"));
        pokemonInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        turnOrder = pokemonInBattle;
        turnOrder.Sort(SortBySpeed);
        turnOrder.Reverse();
    }

    public void CheckTurnOrder()
    {
        bUI.MovesToggle();
        if (turnOrder.Count > 1)
        {
            AttackStart(turnOrder[0], turnOrder[1]);    
        }
    }

    private void AttackStart(GameObject attackingPokemon, GameObject defendingPokemon)
    {
        bUI.Done = false;

        int moveIndex = ChosenMove;

        Pokemon pkmn1 = null;
        Pokemon pkmn2 = null;

        pkmn1 = attackingPokemon.GetComponent<Pokemon>();
        pkmn2 = defendingPokemon.GetComponent<Pokemon>();

        currPokemon = pkmn1;

        if (attackingPokemon.CompareTag("Enemy"))
        {
            moveIndex = Random.Range(0, 4);
        }

        currMove = pkmn1.Moves[moveIndex];

        float damage = Mathf.Floor(Mathf.Floor(((2 * pkmn1.Level) / 5 + 2) * currMove.BasePower * pkmn1.Attack) / pkmn2.Defense) / 50 + 2;
        float stab = 1;

        bool multiType = false;

        if (pkmn1.pokeType.Count > 1)
        {
            multiType = true;
        }

        if (multiType)
        {
            if ((int)currMove.moveType == (int)pkmn1.pokeType[0] || (int)currMove.moveType == (int)pkmn1.pokeType[1])
            {
                stab = 1.5f;
            }
        }
        else
        {
            if ((int)currMove.moveType == (int)pkmn1.pokeType[0])
            {
                stab = 1.5f;
            }
        }

        effective = TypeCheck(pkmn2, currMove.moveType);       
        float crit = Random.Range(0, 100);

        if (crit <= 6.25f)
        {
            crit = 2;
            didCrit = true;
        }
        else
        {
            crit = 1;
            didCrit = false;
        }

        damage *= stab * effective * crit;

        // nog x STAB / EFFECTIVENESS / CRIT CHANCE (6.25%) !!!!!!!
        Debug.Log("Pokemon: " + pkmn1.Name + " did " + damage + " damage with Move: " + pkmn1.Moves[moveIndex].MoveName);

        StartCoroutine(bUI.TakeDamage(damage, pkmn2, pkmn1, currMove, effective, didCrit));

        if (attackingPokemon == turnOrder[0])
        {
            bUI.Done = false;
            StartCoroutine(NextTurn(moveIndex));
        }
        else
        {
            bUI.Done = false;
            StartCoroutine(WaitForBattle());
        }
    }

    private float TypeCheck(Pokemon pkmn, type moveType)
    {
        float effective = 1;

        if (pkmn.weaknesses.Contains(moveType))
        {
            effective = 2;
        }
        else if (pkmn.resistances.Contains(moveType))
        {
            effective = 0.5f;
        }
        else if (pkmn.immunities.Contains(moveType))
        {
            effective = 0;
        }
        else if (pkmn.quadWeaknesses.Contains(moveType))
        {
            effective = 4;
        }
        else if (pkmn.quadResistances.Contains(moveType))
        {
            effective = 0.25f;
        }

        return effective;
    }

    private IEnumerator NextTurn(int moveIndex)
    {
        while (!bUI.Done)
        {
            yield return null;
        }

        bUI.Done = false;

        AttackStart(turnOrder[1], turnOrder[0]);
    }

    private IEnumerator WaitForBattle()
    {
        while (!bUI.Done)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        bUI.MovesToggle();
    }

    private int SortBySpeed(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<Pokemon>().Speed.CompareTo(p2.GetComponent<Pokemon>().Speed);
    }
}
