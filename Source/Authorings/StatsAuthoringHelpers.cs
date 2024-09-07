using Unity.Entities;
using UnityEngine;

namespace Nanory.Unity.Entities.Stats
{
    public class StatsAuthoringHelpers : MonoBehaviour {
        private class StatsAuthoringHelperBaker : Baker<StatsAuthoringHelpers> {
            public override void Bake(StatsAuthoringHelpers authoring) {
            }
        }
    }
}
