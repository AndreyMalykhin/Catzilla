using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Model {
    [CreateAssetMenuAttribute]
    public class EnvTypeInfoStorage: ScriptableObject {
        [SerializeField]
        private EnvTypeInfo[] items;

        private readonly IDictionary<EnvType, EnvTypeInfo> itemsMap =
            new Dictionary<EnvType, EnvTypeInfo>();
        private readonly List<LevelAreaRect> playerSpawnLocations = new List<LevelAreaRect>() {
            new LevelAreaRect(new LevelAreaPoint(6, 0), 3, 6)
        };

        public EnvTypeInfo Get(EnvType envType) {
            return itemsMap[envType];
        }

        private void OnEnable() {
            // Debug.Log("EnvTypeInfoStorage.OnEnable()");

            for (var i = 0; i < items.Length; ++i) {
                itemsMap.Add(items[i].Type, items[i]);
            }

            InitTrackStartEnv();
            InitTrackMiddleEnv();
            InitTrackEndEnv();
            InitParkStartEnv();
            InitParkMiddleEnv();
            InitParkEndEnv();
            InitHoodStartEnv();
            InitHoodMiddleEnv();
            InitHoodEndEnv();
        }

        private void InitTrackStartEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Player] = playerSpawnLocations;
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            itemsMap[EnvType.TrackStart].SpawnLocations = spawnLocations;
        }

        private void InitTrackMiddleEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            spawnLocations[LevelObjectType.Mine] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(7, 7), 2, 17)
            };
            itemsMap[EnvType.TrackMiddle].SpawnLocations = spawnLocations;
        }

        private void InitTrackEndEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            spawnLocations[LevelObjectType.Mine] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(7, 7), 2, 17)
            };
            itemsMap[EnvType.TrackEnd].SpawnLocations = spawnLocations;
        }

        private void InitHoodStartEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Player] = playerSpawnLocations;
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            itemsMap[EnvType.HoodStart].SpawnLocations = spawnLocations;
        }

        private void InitHoodMiddleEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            spawnLocations[LevelObjectType.Mine] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(7, 7), 2, 17)
            };
            itemsMap[EnvType.HoodMiddle].SpawnLocations = spawnLocations;
        }

        private void InitHoodEndEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            spawnLocations[LevelObjectType.Mine] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(7, 7), 2, 17)
            };
            itemsMap[EnvType.HoodEnd].SpawnLocations = spawnLocations;
        }

        private void InitParkStartEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Player] = playerSpawnLocations;
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            itemsMap[EnvType.ParkStart].SpawnLocations = spawnLocations;
        }

        private void InitParkMiddleEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            spawnLocations[LevelObjectType.Mine] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(7, 7), 2, 17)
            };
            itemsMap[EnvType.ParkMiddle].SpawnLocations = spawnLocations;
        }

        private void InitParkEndEnv() {
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations =
                new Dictionary<LevelObjectType, List<LevelAreaRect>>();
            spawnLocations[LevelObjectType.Civilian] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(0, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(15, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.Cop] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(1, 7), 1, 17),
                new LevelAreaRect(new LevelAreaPoint(14, 7), 1, 17)
            };
            spawnLocations[LevelObjectType.CivilianCar] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(3, 7), 4, 17),
                new LevelAreaRect(new LevelAreaPoint(9, 7), 4, 17)
            };
            spawnLocations[LevelObjectType.Mine] = new List<LevelAreaRect>() {
                new LevelAreaRect(new LevelAreaPoint(7, 7), 2, 17)
            };
            itemsMap[EnvType.ParkEnd].SpawnLocations = spawnLocations;
        }
    }
}
