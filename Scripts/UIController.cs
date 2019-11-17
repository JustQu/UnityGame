using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController instance;
	public Slider bossHealth;
	public Slider playerHealth;
	
	private void Awake()
	{
		if (instance == null){
			instance = this;
		} else {

		}
	}

	public void ChangeBossHealth(float health)
	{
		bossHealth.value = health;
	}

	public void ChangePlayerHealth(float health)
	{
		playerHealth.value = health;
	}
}
