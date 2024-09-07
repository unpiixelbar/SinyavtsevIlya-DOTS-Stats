using Unity.Entities;
using UnityEngine;

namespace Nanory.Unity.Entities.Stats
{
    /// <summary>
    /// Authoring for <see cref="StatReceiverTag">Stat Receiver Tag</see>
    /// </summary>
    [DisallowMultipleComponent]
    public class StatReceiverAuthoring : MonoBehaviour {
        private class StatReceiverAuthoringBaker : Baker<StatReceiverAuthoring> {
            public override void Bake(StatReceiverAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<StatReceiverTag>(entity);
                AddBuffer<StatReceivedElementEvent>(entity);
            }
        }
    }
}
