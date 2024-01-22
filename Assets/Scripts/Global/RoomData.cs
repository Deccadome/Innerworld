using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public List<string> entranceNames;
    public List<Vector3> entrancePositions;
    public Dictionary<string, Vector3> entrances;
    //public Dictionary<string, Vector2> cameraBorders;

    private bool roomInitialized = false;

    private void Awake()
    {
        entrances = new Dictionary<string, Vector3>();
        //cameraBorders = new Dictionary<string, Vector2>
        //{
        //    { "Up", new Vector2(0, 1) },
        //    { "Right", new Vector2(1, 0) },
        //    { "Left", new Vector2(-1, 0) },
        //    { "Down", new Vector2(0, -1) },
        //};

        for(int i = 0; i < entranceNames.Count; ++i)
        {
            Debug.Log("Entrance name: " + entranceNames[i]);
            entrances.Add(entranceNames[i], entrancePositions[i]);
        }
        roomInitialized = true;
    }

    private void Start()
    {
    }

    public IEnumerator WaitForRoomInit()
    {
        while (!roomInitialized) yield return null;
    }
}
