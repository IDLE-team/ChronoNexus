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
        [HideInInspector]public bool _IsTargetFounded;
        public List<Entity> _movableEntities;
    }

    Group _group;


    public Group CreateGroup(List<Entity> _entities)
    {
        Debug.Log("Entity: "+_entities[0]);
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (Entity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                if (_movableEntity == null)
                {
                    continue;
                }
                _movableEntity.TargetFinder.OnTargetFinded -= CallTargetSetToGroup;
            }
        }
        _movableEntitiesGroups = new List<Group>();
        Group _newGroup = new Group();
        _movableEntitiesGroups.Add(_newGroup);
        _newGroup._movableEntities = new List<Entity>();
        foreach (Entity _entity in _entities)
        {
            _newGroup._movableEntities.Add(_entity);
            Debug.Log("Entity: "+_entity);
            Debug.Log("TargetFinder: "+_entity.TargetFinder);
            
            _entity.TargetFinder.OnTargetFinded += CallTargetSetToGroup;
        }

        return _newGroup;
    }
    public void CallTargetSetToGroup(ITargetable target)
    {
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (Entity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                if (_movableEntity == null)
                {
                    continue;
                }

                if (_movableEntity.TargetFinder.Target == target)
                {
                    Debug.Log("Группа - " + _movableEntitiesGroup);
                    _group = _movableEntitiesGroup;
                    _movableEntitiesGroup._IsTargetFounded = true;
                    TargetGroupSet(target);
                    break;
                }
            }
        }
    }
    public void TargetGroupSet(ITargetable target)
    {
        foreach (Entity _movableEntity in _group._movableEntities)
        {
            if (_movableEntity == null)
            {
                continue;
            }
            if (_movableEntity.TargetFinder.Target == target)
            {
                continue;
            }

            _movableEntity.TargetFinder.SetTarget(target);
        }
    }

    private void Start()
    {
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (Entity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                if (_movableEntity == null)
                {
                    continue;
                }
                _movableEntity.TargetFinder.OnTargetFinded += CallTargetSetToGroup;
            }
        }
    }

    private void OnDisable()
    {
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (Entity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                if (_movableEntity == null)
                {
                    continue;
                }
                _movableEntity.TargetFinder.OnTargetFinded -= CallTargetSetToGroup;
            }
        }
    }
}