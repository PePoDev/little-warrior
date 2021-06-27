using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 
public class GameManager : MonoBehaviour
{
	public Transform flyPoint, groundPoint;
	
	public Level[] enemysQueue;
	
	public TextMeshProUGUI infoText;
	public GameObject tutorial;
	
	private int selectedLevel;
	private int currentSpawn;
	private int totalEnemy;
	private float spawnTime;
	
	[Serializable]
	public struct Level {
		public Enemy[] enemys;
	}
	
	[Serializable]
	public struct Enemy {
		public GameObject enemy;
		public float time;
		public bool isBoss;
		public bool isFly;
	}
	
	private void Start()
    {
	    selectedLevel = PlayerPrefs.GetInt("SelectedLevel");
	    if (selectedLevel == 1) {
	    	tutorial.SetActive(true);
	    }
	    totalEnemy = enemysQueue[selectedLevel].enemys.Length;
	    infoText.text = $"<b>level</b> {selectedLevel + 1}\n<b>Remaining</b> {totalEnemy}";
    }

	private void Update()
	{
		if (totalEnemy == 0) {
			return;
		}
		
	    spawnTime += Time.deltaTime;
	    var currentSpawnTime = enemysQueue[selectedLevel].enemys[currentSpawn].time;
	    
	    if (spawnTime > currentSpawnTime) {
	    	spawnTime -= currentSpawnTime;
	    	var enemy = enemysQueue[selectedLevel].enemys[currentSpawn].enemy;
	    	var spawnPoint = enemysQueue[selectedLevel].enemys[currentSpawn].isFly ? flyPoint : groundPoint;
	    	GameObject.Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	    }
	}
    
	public void EnemyDied() {
		totalEnemy--;
		if (totalEnemy == 0) {
			// Win
		}
	}
}
