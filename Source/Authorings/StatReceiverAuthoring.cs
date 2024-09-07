using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Nanory.Unity.Entities.Stats
{
    /// <summary>
    /// Authoring for <see cref="StatReceiverTag">Stat Receiver Tag</see>
    /// </summary>
    [DisallowMultipleComponent]
    public class StatReceiverAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<StatReceiverTag>(entity);
            dstManager.AddBuffer<StatRecievedElementEvent>(entity);
        }
    }
}
