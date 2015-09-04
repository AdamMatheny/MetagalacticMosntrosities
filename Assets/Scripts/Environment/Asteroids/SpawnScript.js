#pragma strict

// Variable to store the enemy prefab
public var enemy : GameObject;

// Variable to know how fast we should create new enemies
public var spawnTime : float = 2;


function Update()
{

	spawnTime -= Time.timeScale;
	
	if(spawnTime <= 0)
	{
	
		addEnemy();
		spawnTime = Random.value * 500; //Change this back to 200 for main build
	}
}

// New function to spawn an enemy
function addEnemy() 
{  
    // Variables to store the X position of the spawn object
    // See image below
    var x1 = transform.position.x - GetComponent.<Renderer>().bounds.size.x/2;
    var x2 = transform.position.x + GetComponent.<Renderer>().bounds.size.x/2;

    // Randomly pick a point within the spawn object
    var spawnPoint = new Vector2(Random.Range(x1, x2), transform.position.y);

    // Create an enemy at the 'spawnPoint' position
    Instantiate(enemy, spawnPoint, Quaternion.identity);
}