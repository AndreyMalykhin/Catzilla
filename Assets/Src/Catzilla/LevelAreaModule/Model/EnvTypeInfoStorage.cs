using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Model {
    public class EnvTypeInfoStorage {
        private readonly IDictionary<EnvType, EnvTypeInfo> envTypeInfos =
            new Dictionary<EnvType, EnvTypeInfo>();
        private readonly List<LevelAreaRect> playerSpawnLocations = new List<LevelAreaRect>() {
            new LevelAreaRect(new LevelAreaPoint(6, 0), 3, 6)
        };

        public EnvTypeInfo Get(EnvType envType) {
            return envTypeInfos[envType];
        }

        public EnvTypeInfoStorage() {
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("TrackStart");
            envTypeInfos[EnvType.TrackStart] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("TrackMiddle");
            envTypeInfos[EnvType.TrackMiddle] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("TrackEnd");
            envTypeInfos[EnvType.TrackEnd] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("HoodStart");
            envTypeInfos[EnvType.HoodStart] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("HoodMiddle");
            envTypeInfos[EnvType.HoodMiddle] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("HoodEnd");
            envTypeInfos[EnvType.HoodEnd] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("ParkStart");
            envTypeInfos[EnvType.ParkStart] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("ParkMiddle");
            envTypeInfos[EnvType.ParkMiddle] = new EnvTypeInfo(viewProto, spawnLocations);
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
            // spawnLocations[LevelObjectType.Food] = new List<LevelAreaRect>() {
            //     new LevelAreaRect(new LevelAreaPoint(2, 7), 1, 17),
            //     new LevelAreaRect(new LevelAreaPoint(13, 7), 1, 17)
            // };
            var viewProto = Resources.Load<EnvView>("ParkEnd");
            envTypeInfos[EnvType.ParkEnd] = new EnvTypeInfo(viewProto, spawnLocations);
        }
    }
}
