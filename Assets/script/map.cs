using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Pool;

public class map : SingletonMonoBehaviour<map>
{
	// Start is called before the first frame update
	public int set_FPS = 30;

	public int columns = 8;                                         //Number of columns in our game board.
	public int rows = 8;                                            //Number of rows in our game board.
	public GameObject[] floorTiles;                                 //Array of floor prefabs.
	public Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
	private List<Vector3> gridPositions = new List<Vector3>();  //A list of possible locations to place tiles.
	private piece[,] piece_mats;

	// public float self_x;
	// public float self_y;
	public Vector3 self_offset;
	//Clears our list gridPositions and prepares it to generate a new board.
	void InitialiseList()
	{
		//Clear our list gridPositions.
		gridPositions.Clear();

		//Loop through x axis (columns).
		for (int x = 0; x < columns; x++)
		{
			//Within each column, loop through y axis (rows).
			for (int y = 0; y < rows; y++)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
		piece_mats = new piece[columns, rows];
	}
	void BoardSetup()
	{
		boardHolder = new GameObject("Board").transform;
		boardHolder.position = transform.position;
		boardHolder.SetParent(this.transform);
		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for (int x = 0; x < columns; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for (int y = 0; y < rows; y++)
			{
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
				// if(x == -1 || x == columns || y == -1 || y == rows)
				// 	toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance = Instantiate(toInstantiate, self_offset + new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent(boardHolder);
			}
		}
	}
	public bool check_direction(int[,] mat, int x, int y, int dr, int dc, int length = 3)
	{
		int new_x;
		int new_y;
		bool normal = true;
		for (int i = 1; i < length; i++)
		{
			new_x = x + i * dr;
			new_y = y + i * dc;
			if (new_x < 0 || new_x >= columns || new_y < 0 || new_y >= rows)
			{
				normal = false;
				break;
			}
			if (mat[new_x, new_y] != mat[x, y])
			{
				normal = false;
				break;
			}
		}
		if (normal)
		{
			return true;
		}
		return false;
	}
	public void eliminate_single(int x, int y)
	{
		if (x < 0 || x >= columns || y < 0 || y >= rows)
		{
			Debug.Log(" out index");
			return;
		}
		if (piece_mats[x, y] != null)
		{
			GameObject.Destroy(piece_mats[x, y]?.gameObject);
			piece_mats[x, y] = null;
		}
	}
	public void eliminate_all()
	{
		for(int i=0;i<columns;i++){
			for(int j=0;j<rows;j++){
				eliminate_single(i,j);
			}
		}
	}
	public List<Vector3> check_eliminate()
	{
		string tag = "Building";
		int[,] mat = new int[columns, rows];
		bool[,] mat_mark = new bool[columns, rows];
		foreach (var vec in gridPositions)
		{
			Vector3 origin = vec + new Vector3(0, 0, -1) + self_offset;
			// 确定射线的方向，这里假设是物体正前方  
			// Vector3 direction = transform.forward;
			Vector3 direction = new Vector3(0, 0, 1);
			Physics2D.queriesHitTriggers = true;
			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);
			int vec_int_x = (int)vec.x;
			int vec_int_y = (int)vec.y;
			mat[vec_int_x, vec_int_y] = 0;
			if (hits.Length != 0)
			{
				foreach (var hit in hits)
				{
					if (hit.collider.CompareTag(tag))
					{
						Debug.Log("check_eliminate hit.collider.CompareTag " + hit.collider.name);
						piece pie = hit.collider.GetComponent<piece>();
						piece_mats[vec_int_x, vec_int_y] = pie;
						mat[vec_int_x, vec_int_y] = pie.piece_kind == Piece_Kind.White ? 1 : 2;
						break;
					}
				}
			}
		}

		List<Vector3> elim_list = new List<Vector3>();
		for (int i = 0; i < columns; i++)
		{
			if (mat[i, 0] == 0)
			{
				continue;
			}
			bool bool_res = check_direction(mat, i, 0, 0, 1, 3);
			if (bool_res)
			{
				for (int j = 0; j < rows; j++)
				{
					if (mat_mark[i, j] == false)
					{

						Vector3 elim_vec = new Vector3(i, j, 0);
						elim_list.Add(elim_vec);
						mat_mark[i, j] = true;
						Debug.Log(" yes eliminate");
						Debug.Log(elim_vec);
					}

				}
			}
		}
		for (int j = 0; j < rows; j++)
		{
			if (mat[0, j] == 0)
			{
				continue;
			}
			bool bool_res = check_direction(mat, 0, j, 1, 0, 3);
			if (bool_res)
			{
				for (int i = 0; i < columns; i++)
				{
					if (mat_mark[i, j] == false)
					{
						Vector3 elim_vec = new Vector3(i, j, 0);
						elim_list.Add(elim_vec);
						mat_mark[i, j] = true;
						Debug.Log(" yes eliminate");
						Debug.Log(elim_vec);
					}
				}
			}
		}
		if (mat[0, 0] != 0)
		{
			bool bool_res = check_direction(mat, 0, 0, 1, 1, 3);
			if (bool_res)
			{
				for (int i = 0; i < columns; i++)
				{
					if (mat_mark[i, i] == false)
					{
						Vector3 elim_vec = new Vector3(i, i, 0);
						elim_list.Add(elim_vec);
						mat_mark[i, i] = true;
						Debug.Log(" yes eliminate");
						Debug.Log(elim_vec);
					}
				}
			}
		}
		if (mat[columns - 1, 0] != 0)
		{
			bool bool_res = check_direction(mat, columns - 1, 0, -1, 1, 3);
			if (bool_res)
			{
				for (int i = 0; i < columns; i++)
				{
					if (mat_mark[columns - 1 - i, i] == false)
					{
						Vector3 elim_vec = new Vector3(columns - 1 - i, i, 0);
						elim_list.Add(elim_vec);
						mat_mark[columns - 1 - i, i] = true;
						Debug.Log(" yes eliminate");
						Debug.Log(elim_vec);
					}
				}
			}
		}
		return elim_list;
	}

	public bool check_has_space()
	{
		bool has_space = false;
		foreach (var vec in gridPositions)
		{
			if (check_single(vec + self_offset, "Building"))
			{
				continue;
			}
			else
			{
				has_space = true;
				break;
			}
		}
		return has_space;
	}
	bool check_single(Vector3 vector3, string tag)
	{
		Vector3 origin = vector3 + new Vector3(0, 0, -1);
		// 确定射线的方向，这里假设是物体正前方  
		// Vector3 direction = transform.forward;
		Vector3 direction = new Vector3(0, 0, 1);
		Physics2D.queriesHitTriggers = true;
		RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);
		bool has_tag = false;
		if (hits.Length != 0)
		{
			foreach (var hit in hits)
			{
				if (hit.collider.CompareTag(tag))
				{
					Debug.Log("hit.collider.CompareTag : Ground " + hit.collider.name);
					has_tag = true;
					break;
				}
			}
		}
		else
		{
			Debug.Log("Hit: none ");
		}
		if (has_tag)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void Start()
	{
		Application.targetFrameRate = set_FPS;
		self_offset = new Vector3(transform.position.x, transform.position.y, 0);
		InitialiseList();
		BoardSetup();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
