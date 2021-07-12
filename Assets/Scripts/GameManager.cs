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
	public TextMeshProUGUI startTitleText;
	public TextMeshProUGUI skillPointText;
	public TextMeshProUGUI hpPointText;
	public TextMeshProUGUI damagePointText;
	public TextMeshProUGUI speedPointText;
	public GameObject tutorial;
	public GameObject GameOverPanel;
	public GameObject WinPanel;
	
	public GameObject NextButton;
	
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
	    totalEnemy = enemysQueue[selectedLevel - 1].enemys.Length;
	    infoText.text = $"<b>Remaining</b> {totalEnemy}";
	    startTitleText.text = $"<b>Level</b> {selectedLevel}";
    }

	private void Update()
	{
		if (totalEnemy == 0 || currentSpawn >= enemysQueue[selectedLevel - 1].enemys.Length) {
			return;
		}
		
		if (enemysQueue[selectedLevel - 1].enemys[currentSpawn].isBoss && totalEnemy > 1) {
			return;
		} else {
			enemysQueue[selectedLevel - 1].enemys[currentSpawn].isBoss = false;
		}
		
	    spawnTime += Time.deltaTime;
		var currentSpawnTime = enemysQueue[selectedLevel - 1].enemys[currentSpawn].time;
	    
	    if (spawnTime > currentSpawnTime) {
	    	spawnTime -= currentSpawnTime;
	    	var enemy = enemysQueue[selectedLevel - 1].enemys[currentSpawn].enemy;
	    	var spawnPoint = enemysQueue[selectedLevel - 1].enemys[currentSpawn].isFly ? flyPoint : groundPoint;
	    	GameObject.Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	    	currentSpawn++;
	    }
	}
    
	public void EnemyDied() {
		totalEnemy--;
		infoText.text = $"<b>Remaining</b> {totalEnemy}";
		if (totalEnemy == 0) {
			var levelStatus = PlayerPrefs.GetInt($"level-{selectedLevel}");
			if (levelStatus == 0) {
				var skillPoint = PlayerPrefs.GetInt("skill-point");
				PlayerPrefs.SetInt("skill-point", skillPoint + 2);
				
				PlayerPrefs.SetInt($"level-{selectedLevel}", 1);
				PlayerPrefs.SetInt($"level-{selectedLevel + 1}", 0);			
			}
			
			UpdatePointUI();
			
			if (selectedLevel == 10) {
				NextButton.SetActive(false);
			}
			
			WinPanel.SetActive(true);
		}
	}
	
	public void GameOver() {
		GameOverPanel.SetActive(true);
	}
	
	public void Pause(){
		Time.timeScale = 0f;
	}
	
	public void Unpause(){
		Time.timeScale = 1f;
	}
	
	public void LoadSceneMenu(){
		Initiate.Fade("Menu", Color.black, 1f);
		Time.timeScale = 1f;
	}
	
	public void RestartLevel(){
		Initiate.Fade("Game", Color.black, 1f);
	}
	
	public void NextLevel(){
		PlayerPrefs.SetInt("SelectedLevel", selectedLevel + 1);
		Initiate.Fade("Game", Color.black, 1f);
	}
	
	public void Quit(){
		Application.Quit();
	}
	
	public void UpgradePoint(string skillName) {
		var totalPoint = PlayerPrefs.GetInt("skill-point");
		
		if (totalPoint > 0) {
			var point = PlayerPrefs.GetInt($"{skillName}-point");
			PlayerPrefs.SetInt($"{skillName}-point", point + 1);
			PlayerPrefs.SetInt("skill-point", totalPoint - 1);
		}
		
		UpdatePointUI();
	}
	
	public void UpdatePointUI() {
		skillPointText.text = $"Point {PlayerPrefs.GetInt("skill-point")}";
		hpPointText.text = $"HP ({PlayerPrefs.GetInt("hp-point")})";
		damagePointText.text = $"Damage ({PlayerPrefs.GetInt("damage-point")})";
		speedPointText.text = $"Speed ({PlayerPrefs.GetInt("speed-point")})";
	}
}
