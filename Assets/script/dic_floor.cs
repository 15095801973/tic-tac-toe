using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dic_floor : MonoBehaviour
{
    // Start is called before the first frame update
    public int id = 0;
    public char cur_piece = ' ' ;
    public GameObject icon_x;
    public GameObject icon_o;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseEnter()
    {
        dic_board.Instance.cur_floor = this;

    }
    void OnMouseExit()
    {
        dic_board.Instance.cur_floor = null;
    }
    public void SetAs(char piece){
        cur_piece = piece;
        if (cur_piece == 'X'){
            // icon_o.s
           SpriteRenderer sp = icon_o.GetComponent<SpriteRenderer>();
            sp.enabled = false;
            sp = icon_x.GetComponent<SpriteRenderer>();
            sp.enabled = true;
        }
        if (cur_piece == 'O'){
           SpriteRenderer sp = icon_o.GetComponent<SpriteRenderer>();
           sp.enabled = true;
            sp = icon_x.GetComponent<SpriteRenderer>();
            sp.enabled = false;
        }
        if (cur_piece ==  ' '){
             SpriteRenderer sp = icon_o.GetComponent<SpriteRenderer>();
            sp.enabled = false;
            sp = icon_x.GetComponent<SpriteRenderer>();
            sp.enabled = false;
        }
    }

}
