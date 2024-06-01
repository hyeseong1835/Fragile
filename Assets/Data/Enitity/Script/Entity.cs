using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Entity<EntityT, DataT> : MonoBehaviour 
    where EntityT : Entity<EntityT, DataT> 
    where DataT : EntityData<EntityT, DataT>
{
    public EntityT entity;
    public DataT data;
}
