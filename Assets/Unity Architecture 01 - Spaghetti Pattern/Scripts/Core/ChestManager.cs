using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UnityArchitecture.SpaghettiPattern
{
    public class ChestManager : MonoBehaviour
    {
        private static ChestManager _instance;
        public static ChestManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<ChestManager>();
                return _instance;
            }
            private set => _instance = value;
        }
        
        [FormerlySerializedAs("playerManager")] [Header("References")]
        public PlayerController playerController;

        [Header("Chests")]
        public float chestSpawnTime = 15f;
        private float _timeSinceLastChestSpawn = 0f;
        public Vector2 chestBounds = new(20f, 20f);
        public ChestItems tier1ChestItems;
        public ChestItems tier2ChestItems;
        public ChestItems tier3ChestItems;
        public ChestItems tier4ChestItems;
        public ChestItems[] allChestItems;
        public Chest chestPrefab;
        public Chest currentChest = null;

        [Header("Pity")]
        public int tier3Pity;
        public int tier4Pity;

        [Header("UI")]
        public GameObject chestMenu;
        public GameObject hudMenu;

        public void Start()
        {
            // Chest has to spawn inside the level.
            if (chestBounds.magnitude > GameManager.Instance.levelBounds.magnitude)
                chestBounds = GameManager.Instance.levelBounds;

            allChestItems = new[] { tier1ChestItems, tier2ChestItems, tier3ChestItems, tier4ChestItems };
            
            SpawnChest();
        }

        private void Update()
        {
            if (GameManager.Instance.isPaused) return;
            
            _timeSinceLastChestSpawn += Time.deltaTime;
            if (_timeSinceLastChestSpawn > chestSpawnTime)
            {
                _timeSinceLastChestSpawn = 0f;
                SpawnChest();
            }
        }

        public void ReduceChestSpawnTime()
        {
            _timeSinceLastChestSpawn += 1f;
        }

        private Vector3 GetRandomChestSpawn()
        {
            return new Vector3(Random.Range(-chestBounds.x, chestBounds.x), 0f, Random.Range(-chestBounds.y, chestBounds.y));
        }

        private void SpawnChest()
        {
            var chest = Instantiate(chestPrefab, GetRandomChestSpawn(), Quaternion.identity);
            chest.minTier = 1;
            chest.maxTier = 5;
            chest.GenerateItems();
        }

        public void SpawnBossChest(Vector3 position)
        {
            var groundPosition = new Vector3(position.x, 0, position.z);
            var chest = Instantiate(chestPrefab, groundPosition, Quaternion.identity);
            chest.minTier = 2;
            chest.maxTier = 4;
            chest.transform.localScale *= 1.5f; // Make the boss chest 50% larger
            chest.GenerateItems();
        }

        public void PickupChest(Chest chest)
        {
            // monitor stats.
            GameManager.Instance.isGameActive = false;

            currentChest = chest;
            chestMenu.SetActive(true);
            hudMenu.SetActive(false);
        }

        public void ApplyItem(ChestItem item)
        {
            playerController.AddItem(item);

            // Attempt to not dash when pressing space to select an item in teh chest menu.
            StartCoroutine(WaitOneFrameToUnpause());

            chestMenu.SetActive(false);
            hudMenu.SetActive(true);
            currentChest = null;
        }

        private IEnumerator WaitOneFrameToUnpause()
        {
            yield return new WaitForEndOfFrame();
            GameManager.Instance.isGameActive = true;
        }

        public void Reset()
        {
            allChestItems = null;
            _timeSinceLastChestSpawn = 0f;

            tier3Pity = 0;
            tier4Pity = 0;
        }

        [ContextMenu("Populate Items")]
        public void PopulateItemsInEditor()
        {
            var allTierLists = new[] { tier1ChestItems, tier2ChestItems, tier3ChestItems, tier4ChestItems};

            // Clear existing items
            foreach (var chestItems in allTierLists)
            {
                chestItems.chestItems.Clear();
            }

            // Populate each chest with one item per stat type
            foreach (var statType in Enum.GetValues(typeof(StatType)))
            {
                foreach (var chestItems in allTierLists)
                {
                    var newItem = ScriptableObject.CreateInstance<ChestItem>();
                    chestItems.chestItems.Add(newItem);
                }
            }
        }

    }
}