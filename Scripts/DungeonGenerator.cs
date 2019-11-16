using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
	public Vector2Int roomCoordinate;
	public Dictionary<string, Room> neighbors;

	private string[,] population;

	public Room(int xCoordinate, int yCoordinate)
	{
		this.roomCoordinate = new Vector2Int(xCoordinate, yCoordinate);
		this.neighbors = new Dictionary<string, Room>();
		this.population = new string[12,6];
		for (int xIndex = 0; xIndex < 12; xIndex += 1){
			for (int yIndex = 0; yIndex < 6; yIndex += 1){
				this.population[xIndex, yIndex] = "";
			}
		}
		this.population[7,4] = "Player";
	}

	public Room(Vector2Int roomCoordinate)
	{
		this.roomCoordinate = roomCoordinate;
		this.neighbors = new Dictionary<string, Room>();
		this.population = new string[12,6];
		for (int xIndex = 0; xIndex < 12; xIndex += 1){
			for (int yIndex = 0; yIndex < 6; yIndex += 1){
				this.population[xIndex, yIndex] = "";
			}
		}
		this.population[7,4] = "Player";
	}

	public List<Vector2Int> NeighborCoordinates(){
		List<Vector2Int> neighborCoordinates = new List<Vector2Int>();
		neighborCoordinates.Add(new Vector2Int(this.roomCoordinate.x, this.roomCoordinate.y - 1));
		neighborCoordinates.Add(new Vector2Int(this.roomCoordinate.x + 1, this.roomCoordinate.y));
		neighborCoordinates.Add(new Vector2Int(this.roomCoordinate.x, this.roomCoordinate.y + 1));
		neighborCoordinates.Add(new Vector2Int(this.roomCoordinate.x - 1, this.roomCoordinate.y));

		return neighborCoordinates;
	}

	public void Connect(Room neighbor)
	{
		string direction = "";

		if (neighbor.roomCoordinate.y < this.roomCoordinate.y){
			direction = "N";
		}
		if (neighbor.roomCoordinate.x > this.roomCoordinate.x){
			direction = "E";
		}
		if (neighbor.roomCoordinate.y > this.roomCoordinate.y){
			direction = "S";
		}
		if (neighbor.roomCoordinate.x < this.roomCoordinate.x){
			direction = "W";
		}
		this.neighbors.Add(direction, neighbor);
	}

	public string PrefabName()
	{
		string name = "Room_";
		foreach (KeyValuePair<string, Room> neighborPair in neighbors) {
			name += neighborPair.Key;
		}
		return name;
	}

	public Room Neighbor(string direction)
	{
		Debug.Log(direction);
		return this.neighbors[direction];
	}

	public void PopulateObstacles (int numberOFObstacles, Vector2Int[] possibleSizes)
	{
		for (int obstacleIndex = 0; obstacleIndex < numberOFObstacles; obstacleIndex++){
			int sizeIndex = Random.Range(0, possibleSizes.Length);
			Vector2Int regionSize = possibleSizes[sizeIndex];
			List<Vector2Int> region = FindFreeRegion(regionSize);
			foreach (Vector2Int coordinate in region){
				// Debug.Log(coordinate.x.ToString());
				// Debug.Log(coordinate.y.ToString());
				this.population[coordinate.x, coordinate.y] = "Obstacle";
			}
		}
	}

	private List<Vector2Int> FindFreeRegion(Vector2Int sizeInTiles)
	{
		List<Vector2Int> region = new List<Vector2Int>();
		do{
			region.Clear();

			Vector2Int centerTile= new Vector2Int(UnityEngine.Random.Range(2, 9), UnityEngine.Random.Range(2, 3));

			region.Add(centerTile);

			int initialXCoordinate = (centerTile.x - (int)Mathf.Floor(sizeInTiles.x / 2));
			int initialYCoordinate = (centerTile.y - (int)Mathf.Floor(sizeInTiles.y / 2));
			for (int xCoordinate = initialXCoordinate;
					xCoordinate < initialXCoordinate + sizeInTiles.x;
					xCoordinate++){
				for (int yCoordinate = initialYCoordinate;
						yCoordinate < initialYCoordinate + sizeInTiles.y;
						yCoordinate++){
							region.Add(new Vector2Int(xCoordinate, yCoordinate));
				}
			}
		}while (!IsFree(region));
		return region;
	}

	private bool IsFree(List<Vector2Int> region)
	{
		foreach(Vector2Int tile in region){
			if (this.population[tile.x, tile.y] != ""){
				return false;
			}
		}
		return true;
	}

	public void AddPopulationToTilemap(Tilemap tilemap, TileBase obstacleTile){
		for (int  xIndex = 0; xIndex < 12; xIndex++){
			for (int yIndex = 0; yIndex < 6; yIndex++){
				if (this.population[xIndex, yIndex] == "Obstacle"){
					tilemap.SetTile(new Vector3Int(xIndex - 6, yIndex - 3, 0), obstacleTile);
				}
			}
		}
	}
}

public class DungeonGenerator : MonoBehaviour
{
	[SerializeField]
	private int numberOfRooms;

	[SerializeField]
	private TileBase obstacleTile;

	[SerializeField]
	private int numberOFObstacles;

	private Room[,] rooms;

	private Room currentRoom;

	private Vector2Int[] possibleObstacleSizes = new Vector2Int[] {
		new Vector2Int(1, 1),
		new Vector2Int(1, 2),
		new Vector2Int(2, 1),
		new Vector2Int(3, 1),
		new Vector2Int(1, 3)
	};

	static private DungeonGenerator instance;

	private void Awake()
	{
		if (instance == null){
			DontDestroyOnLoad(this.gameObject);
			instance = this;
			this.currentRoom = GenerateDungeon();
		} else {
			string roomPrefabName = instance.currentRoom.PrefabName();
			GameObject roomObject = (GameObject) Instantiate(Resources.Load(roomPrefabName));
			Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap>();
			instance.currentRoom.AddPopulationToTilemap(tilemap, instance.obstacleTile);
			Destroy(this.gameObject);
		}
	}

    void Start()
    {
        /* GenerateDungeon();
		printGrid(); */

		string roomPrefabName = currentRoom.PrefabName();
		GameObject roomObject = (GameObject)Instantiate(Resources.Load(roomPrefabName));
		Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap>();
		this.currentRoom.AddPopulationToTilemap(tilemap, this.obstacleTile);
    }

	/*
		The algorithm to generate the dungeon room is as follows:

		-Create a empty grid where the rooms will be saved.
		-Create an initial room and save it in a rooms_to_create list.
		-While the number of rooms is less than a desired number “n”, repeat:
		--Pick the first room in the rooms_to_create list
		--Add the room to the grid in the correspondent location
		--Create a random number of neighbors and add them to rooms_to_create
		-Connect the neighbor rooms.
	*/
	private Room GenerateDungeon(){
		int gridSize = 3 * numberOfRooms;

		rooms = new Room[gridSize, gridSize];

		// first room in the center of the grid
		Vector2Int initialRoomCoordinate = new Vector2Int ((gridSize / 2) - 1, (gridSize / 2) - 1);

		Queue<Room> roomsToCreate = new Queue<Room>();
		roomsToCreate.Enqueue(new Room(initialRoomCoordinate.x, initialRoomCoordinate.y));
		List<Room> createdRooms = new List<Room>();
		while(roomsToCreate.Count > 0 && createdRooms.Count < numberOfRooms){
			Room currentRoom = roomsToCreate.Dequeue();
			this.rooms[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = currentRoom;
			createdRooms.Add(currentRoom);
			AddNeighbors(currentRoom, roomsToCreate);
		}

		foreach (Room room in createdRooms){
			List<Vector2Int> neighborCoordinates = room.NeighborCoordinates();
			foreach (Vector2Int coordinate in neighborCoordinates){
				Room neighbor = this.rooms[coordinate.x, coordinate.y];
				if (neighbor != null){
					room.Connect(neighbor);
				}
			}
			room.PopulateObstacles(this.numberOFObstacles, this.possibleObstacleSizes);
		}

		return this.rooms[initialRoomCoordinate.x, initialRoomCoordinate.y];
	}

	private void AddNeighbors(Room currentRoom, Queue<Room> roomsToCreate){
		List<Vector2Int> neighborCoordinates = currentRoom.NeighborCoordinates();
		List<Vector2Int> availableNeighbors = new List<Vector2Int>();
		foreach (Vector2Int coordinate in neighborCoordinates){
			if (this.rooms[coordinate.x, coordinate.y] == null){
				availableNeighbors.Add(coordinate);
			}
		}

		int numberOfNeighbors = (int)Random.Range(1, availableNeighbors.Count);

		for (int neighborIndex = 0; neighborIndex < numberOfNeighbors; neighborIndex++){
			float randomNumber = Random.value;
			float roomFrac = 1f / (float)availableNeighbors.Count;
			Vector2Int chosenNeighbor = new Vector2Int(0, 0);
			foreach (Vector2Int coordinate in availableNeighbors){
				if (randomNumber < roomFrac){
					chosenNeighbor = coordinate;
					break;
				} else {
					roomFrac += 1f / (float)availableNeighbors.Count;
				}
			}
			roomsToCreate.Enqueue(new Room(chosenNeighbor));
			availableNeighbors.Remove(chosenNeighbor);
		}
	}

	private void printGrid(){
		for (int rowIndex = 0; rowIndex < this.rooms.GetLength(1); rowIndex++){
			string row = "";
			for (int columnIndex = 0; columnIndex < this.rooms.GetLength(0); columnIndex++){
				/* row += this.rooms[columnIndex, rowIndex] == null
					? "X"
					: "Y"; */
				if (this.rooms[columnIndex, rowIndex] == null){
					row += "X";
				} else{
					row += "R";
				}
			}
			Debug.Log(row);
		}
	}

	public void MoveToRoom(Room room){
		this.currentRoom = room;
	}

	public Room CurrentRoom(){
		return this.currentRoom;
	}
}
