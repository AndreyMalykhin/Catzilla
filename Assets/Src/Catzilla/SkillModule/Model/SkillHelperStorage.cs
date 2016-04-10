using System.Collections.Generic;

namespace Catzilla.SkillModule.Model {
    public class SkillHelperStorage {
        private readonly IDictionary<int, ISkillHelper> items =
            new Dictionary<int, ISkillHelper>(8);

        public void Add(SkillType type, ISkillHelper helper) {
            items[(int) type] = helper;
        }

        public ISkillHelper Get(SkillType type) {
            return items[(int) type];
        }
    }
}
