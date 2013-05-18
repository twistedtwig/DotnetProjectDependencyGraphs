using YumlOutput.Class.Models;

namespace YumlOutput.Class.Relationships
{
    public class AggregationRelationship : YumlRelationshipBase
    {
        public YumlModel Parent { get; set; }
        public YumlModel Child { get; set; }
        public int? AggregateCount { get; set; }

        protected override string GenerateRelationMap()
        {
            if (AggregateCount.HasValue)
            {
                return string.Format("{0}<>-{1}{2}", Parent, AggregateCount.Value, Child);
            }

            return string.Format("{0}+->{1}", Parent, Child);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is AggregationRelationship))
            {
                return -1;
            }

            var o = other as AggregationRelationship;
            return o.Parent.Equals(Parent) && o.Child.Equals(Child) ? 0 : 1;
        }
    }
}