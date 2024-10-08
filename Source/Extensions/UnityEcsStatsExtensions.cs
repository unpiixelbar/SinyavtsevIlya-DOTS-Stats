﻿using Unity.Entities;
using Unity.Collections;

namespace Nanory.Unity.Entities.Stats
{
    public static class UnityEcsStatsExtensions
    {
        /// <summary>
        /// Notifies the <see cref="StatReceiverTag">Stat-Reciver</see> entity about applying the Stat of statContextEntity. (Entity command buffer version)
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="statContextEntity">Entity which holds one or more Stat-Entities (e.g. sword, potion)</param>
        /// <param name="statReceiverEntity">Entity which receives and accumulates all it's Stat-Entities values. <see cref="StatReceiverTag">See more</see></param>
        public static void SetStatsChanged(this EntityCommandBuffer entityCommandBuffer, Entity statContextEntity, Entity statReceiverEntity)
        {
            entityCommandBuffer.AddComponent(entityCommandBuffer.CreateEntity(), new StatsChangedRequest() { StatContext = statContextEntity, StatReceiver = statReceiverEntity });
        }

        /// <summary>
        /// Notifies the <see cref="StatReceiverTag">Stat-Reciver</see> entity about removing the Stat of statContextEntity.
        /// </summary>
        /// <param name="statContextEntity">Entity which holds one or more Stat-Entities (e.g. sword, potion)</param>
        public static void SetStatsRemoved(this EntityCommandBuffer entityCommandBuffer, Entity statContextEntity)
        {
            entityCommandBuffer.AddComponent(entityCommandBuffer.CreateEntity(), new StatsRemovedRequest() { StatContext = statContextEntity });
        }

        /// <summary>
        /// Notifies the <see cref="StatReceiverTag">Stat-Reciver</see> entity about applying the Stat of statContextEntity.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="statContextEntity">Entity which holds one or more Stat-Entities (e.g. sword, potion)</param>
        /// <param name="statReceiverEntity">Entity which receives and accumulates all it's Stat-Entities values. <see cref="StatReceiverTag">See more</see></param>
        public static void SetStatsChanged(this EntityManager manager, Entity statContextEntity, Entity statReceiverEntity)
        {
            if (!manager.HasComponent<StatElement>(statContextEntity)) return; 

            var statEntites = manager.GetBuffer<StatElement>(statContextEntity).ToNativeArray(Allocator.Temp);
           
            for (int i = 0; i < statEntites.Length; i++)
            {
                var statEntity = statEntites[i].Value;
                manager.AddComponent<StatsChangedEvent>(statEntity);

                var statReceiverLink = new StatReceiverLink() { Value = statReceiverEntity };

                if (manager.HasComponent<StatReceiverLink>(statContextEntity))
                    manager.SetSharedComponentManaged(statEntity, statReceiverLink);
                else
                    manager.SetSharedComponentManaged(statEntity, statReceiverLink);
            }
        }
        /// <summary>
        /// Notifies the <see cref="StatReceiverTag">Stat-Reciver</see> entity about removing the Stat of statContextEntity.
        /// </summary>
        /// <param name="statContextEntity">Entity which holds one or more Stat-Entities (e.g. sword, potion)</param>
        public static void SetStatsRemoved(this EntityManager manager, Entity statContextEntity)
        {
            if (!manager.HasComponent<StatElement>(statContextEntity)) return;

            var statEntities = manager.GetBuffer<StatElement>(statContextEntity).ToNativeArray(Allocator.Temp);

            for (int i = 0; i < statEntities.Length; i++)
            {
                var statEntity = statEntities[i].Value;
                manager.AddComponent<StatsRemovedEvent>(statEntity);
                
            }
        }
    }
}
