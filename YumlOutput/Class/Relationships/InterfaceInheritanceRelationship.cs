using YumlOutput.Class.Models;

namespace YumlOutput.Class.Relationships
{
    public class InterfaceInheritanceRelationship : YumlRelationshipBase
    {
        public YumlInterface Parent { get; set; }
        public YumlModel Child { get; set; }

        protected override string GenerateRelationMap()
        {
            return string.Format("{0}^-.-{1}", Parent, Child);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is InterfaceInheritanceRelationship))
            {
                return -1;
            }

            var o = other as InterfaceInheritanceRelationship;
            return o.Parent.Equals(Parent) && o.Child.Equals(Child) ? 0 : 1;
        }
    }
}