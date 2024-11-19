using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Joker : MonoBehaviour
{
	public SpriteRenderer sp;
	public SpriteRenderer oversp;
	public float loopTime;
	public float oversp_loopTime;
		public Tweener _sequence = null;

	public void hurt()
	{
		sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1.0f);
		_sequence = DOTween.ToAlpha(
	   () => sp.color,
	   color => sp.color = color,
	   0.0f,                                // 最終的なalpha値
	   loopTime
	   ).SetLoops(2, LoopType.Yoyo);
	}
	public void over()
	{
		oversp.gameObject.SetActive(true);
		oversp.color = new Color(oversp.color.r, oversp.color.g, oversp.color.b, 1.0f);
		_sequence = DOTween.ToAlpha(
	   () => oversp.color,
	   color => oversp.color = color,
	   0.0f,                                // 最終的なalpha値
	   oversp_loopTime
	   );
	}
		public void re_start()
	{
		oversp.gameObject.SetActive(false);
	}
}
