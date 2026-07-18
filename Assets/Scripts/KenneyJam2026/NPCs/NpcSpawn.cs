using UnityEngine;

namespace KenneyJam2026.NPCs
{
    public class NpcSpawn : MonoBehaviour
    {
        public Npc Spawn(Npc prefab, NpcType type)
        {
            var npc = Instantiate(prefab, transform.position, transform.rotation);
            var model = Instantiate(type.ModelPrefab, npc.transform.position, npc.transform.rotation, npc.transform);
            model.transform.localScale = Vector3.one * type.Scale;
            npc.Type = type;
            npc.ExpectedQuantity = type.GetRandomExpectedQuantity();

            return npc;
        }
    }
}