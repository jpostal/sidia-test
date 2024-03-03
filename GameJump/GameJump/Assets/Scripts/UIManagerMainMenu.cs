using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Controle da tela de Main Menu, As ações possíveis são: iniciar o jogo ou sair.
/// </summary>

public class UIManagerMainMenu : MonoBehaviour {

	public GameObject NewGameButton;
	public GameObject QuitButton;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	// Captura o evento de click no botão New Game
	public void NewGameButtonClicked()
	{
		// Carrega a scene do jogo.
		SceneManager.LoadScene (1);
	}

	// Captura o evento de click no botão Quit
	public void QuitButtonClicked()
	{
		// Sai do jogo.
		Application.Quit ();
	}
		
}
