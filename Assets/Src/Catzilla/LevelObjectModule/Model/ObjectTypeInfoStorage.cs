using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    public class ObjectTypeInfoStorage {
        private readonly IDictionary<LevelObjectType, ObjectTypeInfo> objectTypeInfos =
            new Dictionary<LevelObjectType, ObjectTypeInfo>() {
            {LevelObjectType.Player, new ObjectTypeInfo(3, 6, 0, 0f, 0, 0f, Resources.Load<LevelObjectView>("Player"))},
            {LevelObjectType.House, new ObjectTypeInfo(6, 8, 1, 1f, 1, 1f, Resources.Load<LevelObjectView>("House"))},
            {LevelObjectType.CivilianCar, new ObjectTypeInfo(2, 4, 2, 1f, 2, 1f, Resources.Load<LevelObjectView>("CivilianCar"))},
            {LevelObjectType.Bench, new ObjectTypeInfo(1, 2, 3, 1f, 2, 1f, Resources.Load<LevelObjectView>("Bench"))},
            {LevelObjectType.Tree, new ObjectTypeInfo(1, 1, 4, 1f, 2, 1f, Resources.Load<LevelObjectView>("Tree"))},
            {LevelObjectType.Civilian, new ObjectTypeInfo(1, 1, 4, 1f, 1, 1f, Resources.Load<LevelObjectView>("Civilian"))},
            {LevelObjectType.Cop, new ObjectTypeInfo(1, 1, 4, 1f, 1, 1f, Resources.Load<LevelObjectView>("Cop"))},
            {LevelObjectType.Mine, new ObjectTypeInfo(1, 1, 5, 1f, 2, 1f, Resources.Load<LevelObjectView>("Mine"))},
            {LevelObjectType.Food, new ObjectTypeInfo(1, 1, 5, 0.125f, 1, 0f, Resources.Load<LevelObjectView>("Food"))}
        };

        public ObjectTypeInfo Get(LevelObjectType type) {
            return objectTypeInfos[type];
        }
    }
}
