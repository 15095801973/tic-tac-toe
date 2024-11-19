using UnityEngine;
using System.Collections;
using TMPro;

	public class UI : SingletonMonoBehaviour<UI>
	{
		public GameObject score_obj;
		public GameObject bullet_num_obj;
		public GameObject over_obj;


		public void set_score(int score){
			score_obj.GetComponent<TMP_Text>().text = score.ToString();
		}

		public void set_bullet_num(int num){
			bullet_num_obj.GetComponent<TMP_Text>().text = num.ToString();
		}
		public void show_over(){
			over_obj.SetActive(true);
		}
	}
