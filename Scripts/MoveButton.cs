using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    private BattleSystem BS;

    // Start is called before the first frame update
    void Start()
    {
        BS = GameObject.FindGameObjectWithTag("BS").GetComponent<BattleSystem>();
    }

    // Update is called once per frame
    public void GetMove(int moveIndex)
    {
        BS.ChosenMove = moveIndex;
        BS.CheckTurnOrder();        
    }
}
