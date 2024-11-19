using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class piece : MonoBehaviour
{

	public string filterTag;
	// public event Action Piece_Covered;
	public Piece_State piece_state = Piece_State.None;
	public Piece_Kind piece_kind = Piece_Kind.White;
	public SpriteRenderer fake_img;
	public SpriteRenderer inner_img;
	// public Sprite white_sprite;
	// public Sprite black_sprite;
	public bool build_able = true;
	public void state_to_moving()
	{
		piece_state = Piece_State.moving;
		// gameObject.GetComponent<SpriteRenderer>().sprite = fake_img;
		inner_img.enabled = false;
		fake_img.enabled = true;
	}

	public void move_to(float x, float y)
	{
		// transform.position = new Vector3(transform.position.x + x, transform.position.y + y, 0);
		transform.position = new Vector3(x, y, 0);
	}
	public void set_func()
	{
		piece_state = Piece_State.set;
		// gameObject.GetComponent<SpriteRenderer>().sprite = inner_img;
		fake_img.enabled = false;
		inner_img.enabled = true;
	}
	public void change_self()
	{
		if (piece_kind == Piece_Kind.Black)
		{
			piece_kind = Piece_Kind.White;
			// inner_img.sprite = white_sprite;
			inner_img.color = Color.white;
		}
		else
		{
			piece_kind = Piece_Kind.Black;
			inner_img.color = Color.black;
			// inner_img.sprite = black_sprite;
		}
	}


	void Start()
	{
		// Piece_Covered += Piece_Covered_Action;
		check_local();
	}
	public void move_by_mouse()
	{
		Vector3 mousePositionInScreen = Input.mousePosition;
		// 假设相机在Z轴上的位置是-10（这取决于你的场景设置）  
		// float cameraZ = Camera.main.transform.position.z;  
		// 将屏幕坐标转换为世界坐标  
		Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePositionInScreen.x, mousePositionInScreen.y, 0));
		int int_x = Mathf.RoundToInt(mousePositionInWorld.x - map.Instance.self_offset.x);
		int int_y = Mathf.RoundToInt(mousePositionInWorld.y - map.Instance.self_offset.y);
		int cur_x = Mathf.RoundToInt(transform.position.x - map.Instance.self_offset.x);
		int cur_y = Mathf.RoundToInt(transform.position.y - map.Instance.self_offset.y);
		if (int_x != cur_x | int_y != cur_y)
		{
			move_to(int_x + map.Instance.self_offset.x, int_y + map.Instance.self_offset.y);
			check_local();
		}

	}
	public void check_local()
	{
		Vector3 origin = transform.position + new Vector3(0, 0, -1);

		// 确定射线的方向，这里假设是物体正前方  
		// Vector3 direction = transform.forward;
		Vector3 direction = new Vector3(0, 0, 1);
		Physics2D.queriesHitTriggers = true;
		RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);
		bool has_ground = false;
		bool has_cover = false;
		if (hits.Length != 0)
		{
			foreach (var hit in hits)
			{
				if (hit.collider.gameObject == this.gameObject)
				{
					Debug.Log("hit self");
					continue;
				}
				if (hit.collider.CompareTag("Ground"))
				{
					Debug.Log("hit.collider.CompareTag : Ground " + hit.collider.name);
					has_ground = true;
				}
				if (hit.collider.CompareTag("Building"))
				{
					has_cover = true;
				}
				// 如果检测到碰撞，可以在这里处理它，比如打印碰撞的物体名称  
				Debug.Log("Hit: " + hit.collider.name);
				// 也可以在这里绘制射线以可视化  
			}
		}
		else
		{
			Debug.Log("Hit: none ");
		}

		if (has_ground & !has_cover)
		{
			build_able = true;
			fake_img.color = Color.white;
		}
		else
		{
			build_able = false;
			fake_img.color = Color.red;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
	public void Piece_Reset_Action()
	{
		Debug.Log(" Piece_Reset_Action" + gameObject.name);
		build_able = false;
		GetComponent<SpriteRenderer>().color = Color.white;
	}
	public void Piece_Covered_Action()
	{
		Debug.Log(" Piece_Covered_Action" + gameObject.name);
		build_able = true;
		GetComponent<SpriteRenderer>().color = Color.red;
	}
	private void OnTriggerEnter2D(Collider2D otherCollider)
	// void OnCollisionEnter2D(Collision2D otherCollider)
	{
		// Debug.Log(" OnTriggerEnter2D");

		// if(collision.collider.CompareTag(filterTag)
		// 	|| !filterByTag)
		// {
		// 	Piece_Covered_Action(collision.gameObject);
		// }
		// otherCollider.gameObject.GetComponent<piece>()?.Piece_Covered();
		// if (piece_state == Piece_State.moving)
		// {
		// 	Piece_Covered();
		// }
	}
	private void OnTriggerExit2D(Collider2D otherCollider)
	// void OnCollisionEnter2D(Collision2D otherCollider)
	{
		// Debug.Log(" OnTriggerEnter2D");

		// if(collision.collider.CompareTag(filterTag)
		// 	|| !filterByTag)
		// {
		// 	Piece_Covered_Action(collision.gameObject);
		// }
		// otherCollider.gameObject.GetComponent<piece>()?.Piece_Covered();
		// if (piece_state == Piece_State.moving)
		// {
		// 	Piece_Reset_Action();
		// }
	}
}
public enum Piece_State { None, moving, set }
public enum Piece_Kind { None, Black, White, Versatile }

