using Unity.Entities;
using UnityEngine;

namespace Nanory.Unity.Entities.Stats
{
    /// <summary>
    /// Base Authoring for all Stat Components. 
    /// </summary>
    /// <typeparam name="TStatComponent"></typeparam>
    public abstract class StatAuthoringBase<TStatComponent> : MonoBehaviour, IStatAuthoring where TStatComponent : unmanaged, IStatComponent
    {
        [SerializeField] StatOpType _opType;

        private class StatAuthoringBaseBaker : Baker<StatAuthoringBase<TStatComponent>>
        {
            public override void Bake(StatAuthoringBase<TStatComponent> authoring)
            {
                // var entity = GetEntity(TransformUsageFlags.None);
                // var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                //
                // var statEntity = CreateAdditionalEntity(TransformUsageFlags.None);
                // AddComponent(statEntity, authoring.GetStat());
                //
                // if (authoring._opType == StatOpType.Additive) {
                //     AddComponent<AdditiveStatTag>(statEntity);
                // }
                //
                // if (authoring._opType == StatOpType.Multiply) {
                //     AddComponent<MultiplyStatTag>(statEntity);
                // }
                //
                // var stats = entityManager.HasBuffer<StatElement>(entity)
                //     ? entityManager.GetBuffer<StatElement>(entity)
                //     : AddBuffer<StatElement>(entity);
                //
                // stats.Add(new StatElement() { Value = statEntity });
                //
                // if (entityManager.HasComponent<StatReceiverTag>(entity)) {
                //     AddSharedComponent(statEntity, new StatReceiverLink() { Value = entity });
                //     AddComponent(entity, authoring.GetStat());
                // }
            }
        }

        /// <summary>
        /// Expects the new instance of TStatComponent created by the conversion process.
        /// </summary>
        protected abstract TStatComponent GetStat();
    }

    internal interface IStatAuthoring
    {
    }
}
