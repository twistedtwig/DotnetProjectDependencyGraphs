using YumlOutput.Class.Models;

namespace YumlOutput.Class.Relationships
{
    public class CompositionRelationship : YumlRelationshipBase
    {
        public YumlModel Parent { get; set; }
        public YumlModel Child { get; set; }
        public int? CompositionCount { get; set; }

        protected override string GenerateRelationMap()
        {            
            return string.Format("{0}++-{1}{2}", Parent, CompositionCount.HasValue ? CompositionCount.Value.ToString() : string.Empty, Child);            
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is CompositionRelationship))
            {
                return -1;
            }

            var o = other as CompositionRelationship;
            return o.Parent.Equals(Parent) && o.Child.Equals(Child) ? 0 : 1;
        }
    }
}