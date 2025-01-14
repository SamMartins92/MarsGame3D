﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour {

	public float fullHealth;
	public GameObject deathFX;
	public AudioClip playerHurt;

	public restartGame theGameManager;

	float currentHealth;

	PlayerController controlMovement;

	public AudioClip playerDeathSound;
	AudioSource playerAS;

	//HUD variables
	public Slider healthSlider;
	public Image damageScreen;
	public Text gameOverScreen;
	public Text winGameScreen;

	bool damaged = false;
	Color damageColour = new Color(0f, 0f, 0f,1f);
	float smoothColour = 5f;

	// Use this for initialization
	void Start () {
		currentHealth = fullHealth;

		controlMovement = GetComponent<PlayerController> ();

		//HUD Initialization
		healthSlider.maxValue = fullHealth;
		healthSlider.value = fullHealth;

		damaged = false;

		playerAS = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (damaged) {
			damageScreen.color = damageColour;
		} else {
			damageScreen.color = Color.Lerp (damageScreen.color, Color.clear, smoothColour * Time.deltaTime);
		}
		damaged = false;
		
	}

	public void addDamage (float damage){
		if (damage <= 0)return;
		currentHealth -= damage;

		playerAS.clip = playerHurt;
		playerAS.Play ();

		healthSlider.value = currentHealth;
		damaged = true;

		if (currentHealth <= 0) {
			makeDead ();
		}
	}

	public void addHealth (float healthAmount){
		currentHealth += healthAmount;
		if (currentHealth > fullHealth)	currentHealth = fullHealth;
		healthSlider.value = currentHealth;
	}

	public void makeDead(){
		Instantiate (deathFX, transform.position, transform.rotation);
		Destroy (gameObject);
		AudioSource.PlayClipAtPoint (playerDeathSound, transform.position);
		damageScreen.color = damageColour;

		Animator gameOverAnimator = gameOverScreen.GetComponent<Animator> ();
		gameOverAnimator.SetTrigger ("gameOver");
		theGameManager.restartTheGame ();
	}

	public void winGame(){
		Destroy (gameObject);
		theGameManager.restartTheGame ();
		Animator winGameAnimator = winGameScreen.GetComponent<Animator> ();
		winGameAnimator.SetTrigger ("gameOver");

	}
}
