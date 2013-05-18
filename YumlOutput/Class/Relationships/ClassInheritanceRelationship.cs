using YumlOutput.Class.Models;

namespace YumlOutput.Class.Relationships
{
    public class ClassInheritanceRelationship : YumlRelationshipBase
    {
        public YumlModel Parent { get; set; }
        public YumlModel Child { get; set; }

        protected override string GenerateRelationMap()
        {
            return string.Format("{0}^->{1}", Parent, Child);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is ClassInheritanceRelationship))
            {
                return -1;
            }

            var o = other as ClassInheritanceRelationship;
            return o.Parent.Equals(Parent) && o.Child.Equals(Child) ? 0 : 1;
        }
    }
}