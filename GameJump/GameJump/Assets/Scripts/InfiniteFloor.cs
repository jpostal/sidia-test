using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Script que controla a mecânica do jogo: Chão infinito, spawn aleatório de obstáculos, condição de Game over e controle do player.
/// </summary>

public class InfiniteFloor : MonoBehaviour {

	public GameObject prefabPlatform;
	public GameObject prefabObstacle;

	// Lista para armazenar as plataformas, criando o chão.
	private List<GameObject> floor = new List<GameObject>();

	// Lista para armazenar os obstáculos, que variam em altura.
	private List<GameObject> obstacles = new List<GameObject>();
	private ArrayList obstacle_scale_y = new ArrayList (); // Auxilia na randomização da altura do obstáculo.
	private ArrayList obstacle_position_y = new ArrayList (); 

	// Para controlar o pulo do player.
	private bool grounded = true;
	private float jumpPower = 11f;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody> ();

		floor.Add(prefabPlatform);
		obstacles.Add (prefabObstacle);

		// Cria as possíveis alturas dos obstáculos 1.5, 2 ou 2.5.
		for (int i = 0; i < 3; i++) {
			obstacle_scale_y.Add (prefabObstacle.transform.localScale.y + (i * 0.5f));
			obstacle_position_y.Add (prefabObstacle.transform.position.y + (i * 0.25f));
		}

		// Cria as plataformas adicionais.
		for (int i = 1; i < 5; i++) {
			// A próxima plataforma será posicionada na <posição da plataforma atual + sua scale z (igual para todas)>
			float plat_pos_z = prefabPlatform.transform.position.z + prefabPlatform.transform.localScale.z * i;
			Vector3 plat_pos = new Vector3 (prefabPlatform.transform.position.x, prefabPlatform.transform.position.y, plat_pos_z);
			GameObject new_plat = (GameObject) Instantiate (prefabPlatform, plat_pos, Quaternion.identity);
			new_plat.name = "Plat" + (i+1);
			floor.Add(new_plat);

			// O obstáculo é criado em cima da plataforma recém-criada.
			int rand_index = Random.Range (0, 3); // Auxilia na randomização da altura do obstáculo.
			float plat_scale_z_offset = prefabPlatform.transform.localScale.z / 2; // Auxilia na randomização da posição do obstáculo na plataforma.

			Vector3 obstacle_scale = new Vector3(prefabObstacle.transform.localScale.x, (float) obstacle_scale_y[rand_index], prefabObstacle.transform.localScale.z);
			float z_pos = Random.Range (new_plat.transform.position.z - plat_scale_z_offset, new_plat.transform.position.z + plat_scale_z_offset);
			Vector3 obstacle_position = new Vector3 (prefabObstacle.transform.position.x, (float) obstacle_position_y[rand_index], z_pos);
			GameObject obstacle = (GameObject) Instantiate (prefabObstacle, obstacle_position, Quaternion.identity);
			obstacle.name = "Obst" + (i+1);
			obstacle.transform.localScale = obstacle_scale;
			obstacles.Add(obstacle);
	
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// Evita que o atrito do player com o chão na hora do pulo faça sua rotação mudar.
		transform.localRotation = Quaternion.identity;

		// Se o player ja estiver no chão, pode pular novamente.
		if(!grounded && rb.velocity.y == 0) {
			grounded = true;

		}

		// Executa o pulo ao pressionar a barra de espaço.
		if (Input.GetKeyDown(KeyCode.Space) && grounded == true) {
			rb.AddForce(0,jumpPower,0,ForceMode.Impulse);
			grounded = false;
		}
			
	}
	/* 
	 Para criar a impressão de chão infinito com baixo custo e reaproveitando os objetos, a plataforma armazenada na primeira posição da Lista floor
	 é reposicionada toda vez que o player passa na terceira plataforma, bem como o seu obstáculo correspondente, que está armazenado na Lista obstacles. 
	*/
	void OnCollisionEnter(Collision col){
		// 
		float plat_scale_z_offset = prefabPlatform.transform.localScale.z / 2;
		GameObject plat_to_move = floor [0]; // guarda a primeira plataforma
		GameObject obstacle_to_move = obstacles [0]; // guarda o obstáculo da primeira plataforma.

		// O player colidiu com a terceira plataforma
		if (col.gameObject.name == floor [2].name) {

			// Calcula a nova posição da primeira plataforma.
			float new_pos_plat_z = floor [4].transform.position.z + floor [0].transform.localScale.z;
			Vector3 new_pos_plat = new Vector3 (floor [0].transform.position.x, floor [0].transform.position.y, new_pos_plat_z);
			floor [0].transform.position = new_pos_plat; // Reposiciona.

			// Randomiza novamente o obstáculo para a plataforma recém-reposicionada.
			int rand_index = Random.Range (0, 2);
			float rand_new_pos_obstacle_z = Random.Range (new_pos_plat_z - plat_scale_z_offset, new_pos_plat_z + plat_scale_z_offset);
			Vector3 new_obstacle_scale = new Vector3 (prefabObstacle.transform.localScale.x, (float)obstacle_scale_y [rand_index], prefabObstacle.transform.localScale.z);
			Vector3 new_pos_obstacle = new Vector3 (prefabObstacle.transform.position.x, (float)obstacle_position_y [rand_index], rand_new_pos_obstacle_z);
			obstacles [0].transform.localScale = new_obstacle_scale;
			obstacles [0].transform.position = new_pos_obstacle;

			// Shift de objetos pra esquerda, para organizar a nova posição das plataformas e dos obstáculos.
			for (int i = 1; i < 5; i++) {
				floor [i - 1] = floor [i];
				obstacles [i - 1] = obstacles [i];
			} 
			// Agora a primeira plataforma e obstáculo recém movidos são os ultimos da lista.
			floor [4] = plat_to_move; 
			obstacles[4] = obstacle_to_move;
		}

		// Se o player colidir com um obstáculo, é Game Over.
		if (obstacles.IndexOf(col.gameObject) > -1){
			Debug.Log ("you died");
			SceneManager.LoadScene (2);
		}
	}


}
