using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	public class Bullet_WB :MonoBehaviour
	{
		public GameObject white_bullet;
		public GameObject black_bullet;
		public GameObject any_bullet;

		public void set_bullet(Piece_Kind piece_Kind){
			any_bullet.SetActive(false);
			white_bullet.SetActive(false);
			black_bullet.SetActive(false);
			if ( piece_Kind == Piece_Kind.White){
				white_bullet.SetActive(true);
			}
			else if( piece_Kind == Piece_Kind.Versatile){
				any_bullet.SetActive(true);
			}
			else if( piece_Kind == Piece_Kind.Black){
				black_bullet.SetActive(true);
			}
			else{
				
			}
			
		}
	}
