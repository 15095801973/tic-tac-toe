using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class Item_pack : MonoBehaviour
{

	public List<Transform> trans_list = new List<Transform>();
	public List<Items> items_list = new List<Items>();
	public List<Items> rd_list = new List<Items>();
	public float pick_delay = 0.1f;
	public int max_vol;
	public void clear_items()
	{
		for (int i = 0; i < trans_list.Count; i++)
		{
			var obj = items_list[i];
			if (obj != null)
			{
				GameObject.Destroy(obj.gameObject);
				items_list[i] =null;
			}
		}
	}
	private void Start()
	{
		max_vol = trans_list.Count;
		items_list.Clear();
		for (int i = 0; i < trans_list.Count; i++)
		{
			items_list.Add(null);
		}
	}
	public Items rd_gen_item()
	{
		int space_num = 0;
		for (int i = 0; i < items_list.Count; i++)
		{
			var obj = items_list[i];
			if (obj == null)
			{
				space_num +=1;
			}
		}
		if (space_num == 0)
		{
			return null;
		}
		Items one_fab = rd_list[Random.Range(0, rd_list.Count)];
		Items one = Instantiate(one_fab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Items>() as Items;
		one.transform.SetParent(transform);
		for (int i = 0; i < items_list.Count; i++)
		{
			var obj = items_list[i];
			if (obj == null)
			{
				items_list[i] = one;
				one.transform.position = trans_list[i].position;
				one.org_position = trans_list[i].position;
				break;
			}
		}
		return one;
	}


	void myCompleteFunction()
	{
		game_man.Instance.set_states(game_man.GameState.OnMove);
	}

}
