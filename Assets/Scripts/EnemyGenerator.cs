using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
    /// <summary>
    /// Store position, accessible enemy type
    /// TODO: read from JSON
    /// </summary>
    class BlockSetup
    {

    }

    /// <summary>
    /// Divide the arena into squares of same size to make efficient enemy generation
    /// Make sure mapSize is divisible by blockSize.
    /// </summary>
    public int blockSize;

    /// <summary>
    /// Temporarily global variable. The capacity of number of enemies in each block.
    /// TODO: Make it local for each block
    /// </summary>
    public int enemyDensity;

    /// <summary>
    /// Size of map. Assume it's squre
    /// TODO: read from Map object
    /// </summary>
    public int mapSize;

    private int _blockNumber;

    private List<Vector2> _blocks = new List<Vector2>();

	// Use this for initialization
	void Start ()
    {
        // create blocks
        _blockNumber = mapSize / blockSize;
        _blocks.Capacity = _blockNumber * _blockNumber;
        for (int i = 0; i < _blocks.Capacity; i++)
        {

        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
