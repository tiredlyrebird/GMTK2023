using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    public void MoveToGameOver()
	{
		SceneManager.LoadScene("YouDied");
	}
}
