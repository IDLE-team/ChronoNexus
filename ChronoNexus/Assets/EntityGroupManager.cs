using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class EntityGroupManager : MonoBehaviour
{
    [SerializeField] private List<Group> _movableEntitiesGroups;
    [Serializable]
    public class Group
    {
        public List<Entity> _movableEntities;
    }

    public void CallTargetSetToGroup(ITargetable target)
    {
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (MovableEntity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                if (_movableEntity.TargetFinder.Target == target)
                {
                    return;
                }
                _movableEntity.TargetFinder.SetTarget(target);
            }
        }
    }

    private void Start()
    {
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (MovableEntity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                _movableEntity.TargetFinder.OnTargetFinded += CallTargetSetToGroup;
            }
        }
    }

    private void OnDisable()
    {
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (MovableEntity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                _movableEntity.TargetFinder.OnTargetFinded -= CallTargetSetToGroup;
            }
        }
    }
}