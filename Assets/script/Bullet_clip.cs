using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class Bullet_clip : MonoBehaviour
{

	public List<Bullet_WB> list = new List<Bullet_WB>();
	public float every_delay = 0.1f;
	public float mid_delay = 1f;
	public float left_border = -10f;
	public float right_border = 10f;

	public void set_bullets(int num_w, int num_b)
	{
		int i;
		for (i = 0; i < num_w; i++)
		{
			list[i].set_bullet(Piece_Kind.White);
		}
		for (; i < num_w + num_b; i++)
		{
			list[i].set_bullet(Piece_Kind.Black);
		}
		for (; i < list.Count; i++)
		{
			list[i].set_bullet(Piece_Kind.None);
		}
		Sequence mySequence = DOTween.Sequence();
		for (i = num_w + num_b - 1; i >= 0; i--)
		{
			list[i].transform.position = new Vector3(left_border, 0, 0);
			mySequence.Append(list[i].transform.DOMoveX(i, every_delay));
		}

		// mySequence.SetDelay(mid_delay);
		mySequence.AppendInterval(mid_delay);  

		for (i = num_w + num_b - 1; i >= 0; i--)
		{
			mySequence.Append(list[i].transform.DOMoveX(right_border, every_delay));
		}
		mySequence.OnComplete(myCompleteFunction);

	}
	public void show_one_bullet(Piece_Kind kind, bool defaut_comp = true)
	{
		list[0].set_bullet(kind);
		Sequence mySequence = DOTween.Sequence();
		list[0].transform.position = new Vector3(left_border, 0, 0);
		mySequence.Append(list[0].transform.DOMoveX(0, every_delay));

		mySequence.AppendInterval(mid_delay);  

		mySequence.Append(list[0].transform.DOMoveX(right_border, every_delay));
		if (defaut_comp == true)
		{
			mySequence.OnComplete(myCompleteFunction);
		}

	}
	void myCompleteFunction()
	{
		Debug.Log("Bullet_clip. myCompleteFunction");
		game_man.Instance.set_states(game_man.GameState.OnMove);
	}

}
