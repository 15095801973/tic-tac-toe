using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class HP_board : MonoBehaviour
{

	public List<SpriteRenderer> list = new List<SpriteRenderer>();
	List<int> int_list = new List<int>();
	public int hp = 0;
	int max_hp = 0;
	public float every_delay = 0.5f;
	private void Start()
	{
		int_list.Clear();
		hp = list.Count;
		max_hp = list.Count;
		foreach (var sp in list)
		{
			sp.color = Color.white;
			int_list.Add(1);
		}
	}

	public void re_start(){
		int i;
		hp = max_hp;
		for (i = hp ; i < max_hp; i++)
		{
			list[i].color = Color.white;
		}
	}
	public void hurt(int damage, bool defaut_comp = true)
	{
		int i;
		int act_dmg = damage < hp ? damage : hp;
		hp = hp - act_dmg;
		Sequence mySequence = DOTween.Sequence();

		for (i = hp + act_dmg - 1; i >= hp; i--)
		{
			mySequence.Append(list[i].DOColor(Color.black, every_delay));
		}
		if (defaut_comp == true)
		{

			mySequence.OnComplete(myCompleteFunction);
		}
	}
	public void heal(int num, bool defaut_comp = true)
	{
		int i;
		int act_heal = hp + num > max_hp ? max_hp - hp : num;
		hp = hp + act_heal;
		Sequence mySequence = DOTween.Sequence();
		for (i = hp - act_heal; i < hp; i++)
		{
			mySequence.Append(list[i].DOColor(Color.white, every_delay));
		}
		if (defaut_comp == true)
		{

			mySequence.OnComplete(myCompleteFunction);
		}
	}
	void myCompleteFunction()
	{
		Debug.Log("HP_board. myCompleteFunction");
		game_man.Instance.set_states(game_man.GameState.OnMove);
	}

}
