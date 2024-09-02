using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorByKills : MonoBehaviour, SceneObjective
{
    [SerializeField] private List<Entity> entityInRoom;
    [SerializeField] private List<DoorTrigger> doorTriggers;

    private int entityCount;

    private void Start()
    {
        entityCount = 0;
        foreach (Entity entity in entityInRoom) 
        {
            entity.OnDie += DecreaseEntityCount;
            entityCount++;
        }
    }

    private void DecreaseEntityCount()
    {
        entityCount -= 1;
    }


    public void Activate()
    {
        foreach (DoorTrigger door in doorTriggers)
        {
            door.UnlockDoor();
        }
    }
}
