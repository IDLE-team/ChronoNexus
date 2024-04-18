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
        public bool _IsTargetFounded;
        public List<Entity> _movableEntities;
    }

    Group _group;

    public void CallTargetSetToGroup(ITargetable target)
    {

        //найти группу и активировать ивенты
        foreach (Group _movableEntitiesGroup in _movableEntitiesGroups)
        {
            foreach (Entity _movableEntity in _movableEntitiesGroup._movableEntities)
            {
                if (_movableEntity.TargetFinder.Target == target)
                {
                    Debug.Log("Группа - "+_movableEntitiesGroup);
                    _group = _movableEntitiesGroup;
                    _movableEntitiesGroup._IsTargetFounded = true;
                    TargetGroupSet(target);
                    break;
                }

                //_movableEntity.TargetFinder.SetTarget(target);
            }
        }
    }

    public void TargetGroupSet(ITargetable target)
    {
        foreach (Entity _movableEntity in _group._movableEntities)
        {
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
                _movableEntity.TargetFinder.OnTargetFinded -= CallTargetSetToGroup;
            }
        }
    }
}