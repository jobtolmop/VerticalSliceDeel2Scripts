using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image enemyHealthBar;

    [SerializeField] private Animator myAnimator;

    [SerializeField] Color yellowColor;
    [SerializeField] Color lowColor;

    [SerializeField] private Text text;
    [SerializeField] private Text hpText;

    private BattleSystem BS;

    private Transform battleUI;
    private GameObject arrow;
    private int prevButton;
    private string sentence = ("You Couldn't Get Away!");
    private string bagSentence = ("Team Rocket Stole Your Bag!");
    //private string critSentence = ("It was a critical hit!");
    //private string superEffectiveSentence = ("It was super effective!");
    //private string notVeryEffectiveSentence = ("It was not very effective!");
    private string gainedExp = ("Your Pokémon gained Exp. Points!");
    private string faintedPokemon = ("The wild Klink fainted!");
    private string usedMove = ("");


    private Pokemon currPokemon;

    private int targetHP = 0;

    private bool damaged = false;
    public bool Done { get; set; } = false;
    public bool ShowingText { get; set; } = false;

    private bool showingBattleText = false;

    private void Start()
    {
        BS = GameObject.FindGameObjectWithTag("BS").GetComponent<BattleSystem>();
        battleUI = transform.GetChild(0);
        arrow = transform.GetChild(7).gameObject;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (transform.GetChild(1).gameObject.activeSelf)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(6).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                arrow.SetActive(false);

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(battleUI.GetChild(0).gameObject);
            }
            else if (transform.GetChild(5).gameObject.activeSelf)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(true);
                transform.GetChild(4).gameObject.SetActive(true);
                transform.GetChild(6).gameObject.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(false);
                arrow.SetActive(false);

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(battleUI.GetChild(1).gameObject);
            }
        }
    }

    public IEnumerator TakeDamage(float damage, Pokemon pkmn, Pokemon atkPkmn, Move move, float effective, bool didCrit)
    {
        atkPkmn.transform.GetChild(atkPkmn.transform.childCount - 1).gameObject.SetActive(false);
        pkmn.transform.GetChild(pkmn.transform.childCount - 1).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        transform.GetChild(8).gameObject.SetActive(true);
        showingBattleText = true;
        StartCoroutine(ShowText(text, atkPkmn.name + " used " + move.MoveName));

        while (showingBattleText)
        {
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(0.5f);
        transform.GetChild(8).gameObject.SetActive(false);

        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);

        pkmn.transform.GetChild(pkmn.transform.childCount - 1).gameObject.SetActive(true);

        currPokemon = pkmn;
        targetHP = currPokemon.HitPoints;

        targetHP -= Mathf.RoundToInt(damage);
       
        damaged = true;

        StartCoroutine("LoseHP");
        StartCoroutine(ShowBattleText(effective, didCrit, currPokemon, move));
    }

    public void Fight()
    {
        Transform moveUI = transform.GetChild(1);

        moveUI.gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(6).gameObject.SetActive(false);
        arrow.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(moveUI.GetChild(0).gameObject);
    }

    public void Pokemon2()
    {
        prevButton = 1;
        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(6).gameObject.SetActive(false);
    }

    public void Bag()
    {
        prevButton = 2;
        StartCoroutine(RunFromBattle());
        StartCoroutine(ShowText(text, bagSentence));
    }

    public void Run()
    {
        prevButton = 3;
        StartCoroutine(RunFromBattle());
        StartCoroutine(ShowText(text, sentence));
    }

    private IEnumerator RunFromBattle()
    {
        transform.GetChild(8).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(6).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        transform.GetChild(8).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(6).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(battleUI.GetChild(prevButton).gameObject);
    }

    private IEnumerator ShowText(Text text, string sentenceText)
    {
        text.text = "";
        foreach (char letter in sentenceText)
        {
            text.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
        showingBattleText = false;
    }

    private IEnumerator Delay()
    {       
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        text.text = "";
        transform.GetChild(8).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.75f);

        showingBattleText = true;
        StartCoroutine(ShowText(text, faintedPokemon));

        while (showingBattleText)
        {
            yield return null;
        }

        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        showingBattleText = true;

        StartCoroutine(ShowText(text, gainedExp));

        while (showingBattleText)
        {
            yield return null;
        }


        yield return new WaitForSeconds(3f);

    }

    private IEnumerator LoseHP()
    {
        float hp = currPokemon.HitPoints;
        float maxHp = currPokemon.MaxHitPoints;

        while (damaged)
        {
            if (hp > targetHP)
            {
                hp -= 0.3f;
                yield return null;
            }
            else if (hp <= targetHP)
            {
                hp = targetHP;
                currPokemon.HitPoints = targetHP;
                damaged = false;
            }            

            if (currPokemon.CompareTag("Enemy"))
            {
                enemyHealthBar.fillAmount = hp / maxHp;
                if (hp <= maxHp / 2)
                {
                    enemyHealthBar.color = yellowColor;
                }
                if (hp <= maxHp / 4)
                {
                    enemyHealthBar.color = lowColor;
                }
            }
            else
            {
                playerHealthBar.fillAmount = hp / maxHp;
                hpText.text = Mathf.Round(hp) + "/" + maxHp;
                if (hp <= maxHp / 2)
                {
                    playerHealthBar.color = yellowColor;
                }
                if (hp <= maxHp / 4)
                {
                    playerHealthBar.color = lowColor;
                }
            }
        }
    }

    public void MovesToggle()
    {
        if (transform.GetChild(1).gameObject.activeSelf)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(7).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(battleUI.GetChild(0).gameObject);
        }            
    }

    public IEnumerator ShowBattleText(float effectiveness, bool crit, Pokemon pkmn, Move move)
    {
        // nu mee bezig, hp van jezelf weg na het klikken op een aanval, eigen hp UI moet weg na een crit / tijdens move, elke andere message eigenlijk.
        //als hp 0 is dan naar victory scherm met "gainedExp" text
        //camera movement, camera naar standaard positie als je een move hebt geselecteerd

        while (damaged)
        {
            yield return null;
        }

        if (effectiveness != 1 || crit)
        {
            text.text = "";
            transform.GetChild(8).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }

        if (effectiveness != 1)
        {
            showingBattleText = true;

            switch (effectiveness)
            {
                case 2:
                    StartCoroutine(ShowText(text, "It was super effective!"));
                    break;
                case 4:
                     StartCoroutine(ShowText(text, "It was super effective!"));
                    break;
                case 0:
                    StartCoroutine(ShowText(text, "It had no effect!"));
                    break;
                case 0.5f:
                    StartCoroutine(ShowText(text, "It was not very effective!"));
                    break;
                case 0.25f:
                    StartCoroutine(ShowText(text, "It was not very effective!"));
                    break;
            }

            while (showingBattleText)
            {
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(1f);
        }

        if (crit)
        {
            text.color = Color.red;
            showingBattleText = true;
            StartCoroutine(ShowText(text, "A critical hit!"));

            while (showingBattleText)
            {
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(1f);
        }

        transform.GetChild(8).gameObject.SetActive(false);
        text.color = Color.white;

        if (targetHP <= 0)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(Delay());
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(true);
            Done = true;
            ShowingText = false;
        }        
    }
}
