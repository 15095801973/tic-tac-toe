using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

public class Items : MonoBehaviour
{
	public Item_Kind kind = Item_Kind.Heal;
	public Vector3 org_position;
	public void pick()
	{
		Debug.Log("pick");
		transform.DOMove(game_man.Instance.pick_position, game_man.Instance.item_pack.pick_delay);
	}
	public void unpick()
	{
		Debug.Log("unpick");
		transform.DOMove(org_position, game_man.Instance.item_pack.pick_delay);
	}
	public void use()
	{
		Debug.Log("use");

		switch (kind)
		{
			case Item_Kind.Heal:
				game_man.Instance.self_hp_board.heal(1);
				game_man.Instance.cur_pick_item = null;
				GameObject.Destroy(this.gameObject);
				break;
			case Item_Kind.Lens:
				game_man.Instance.bullet_Clip.show_one_bullet(game_man.Instance.cur_piece.piece_kind);
				game_man.Instance.cur_pick_item = null;
				GameObject.Destroy(this.gameObject);
				break;
			case Item_Kind.Reload:
				game_man.Instance.reload_bullets();
				GameObject.Destroy(this.gameObject);
				break;
			case Item_Kind.Unload:
				game_man.Instance.unload_one_bullet();
				GameObject.Destroy(this.gameObject);
				break;
			case Item_Kind.Change:
				game_man.Instance.change_one_bullet();
				GameObject.Destroy(this.gameObject);
				break;
			case Item_Kind.Clear:
				game_man.Instance.clear_map();
				GameObject.Destroy(this.gameObject);
				break;
		}
	}
	// private void OnMouseDown() {
	// 	 pick();
	// }
}
public enum Item_Kind { None, Heal, Lens, Reload,Unload,Change, Clear }

