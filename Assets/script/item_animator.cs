using UnityEngine;
using System.Collections;
using UnityEngine;
using DG.Tweening;



	public class item_animator : MonoBehaviour
	{
		int move_state = 0;
		public float loopTime = 1.2f;
		public SpriteRenderer sp;
		public Tweener _sequence = null;
		public bool rotate_z_y = true;

		// public void Start()
		// {
		// 	sp = gameObject.GetComponent<SpriteRenderer>();
		// }
		public void Awake()
		{
			sp = gameObject.GetComponent<SpriteRenderer>();
		}
		public void set_stop()
		{
			move_state = 0;
		}
		public void Process_move(Vector3 vector3)
		{

		}
		private void OnEnable()
		{
			if (_sequence != null)
			{
				_sequence.Kill();
			}
				
					sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1.0f);
					_sequence = DOTween.ToAlpha(
				   () => sp.color,
				   color => sp.color = color,
				   0.0f,                                // 最終的なalpha値
				   loopTime
				   ).OnComplete(set_dis);
				   //.SetLoops(2, LoopType.Yoyo)
		}
		void set_dis(){
			this.gameObject.SetActive(false);
		}
		private void OnDisable()
		{
			if (_sequence != null)
			{
				_sequence.Kill();
			}
		}
		
	}
