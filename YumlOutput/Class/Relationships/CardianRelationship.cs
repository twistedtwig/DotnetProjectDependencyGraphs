using YumlOutput.Class.Models;

namespace YumlOutput.Class.Relationships
{
    public class CardianRelationship : YumlRelationshipBase
    {
        public YumlModel Parent { get; set; }
        public string ParentCardianRelationShip { get; set; }

        public YumlModel Child { get; set; }
        public string ChildCardianRelationShip { get; set; }

        protected override string GenerateRelationMap()
        {
            return string.Format("{0}{1}-{2}{3}", Parent, ParentCardianRelationShip, ChildCardianRelationShip, Child);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is CardianRelationship))
            {
                return -1;
            }

            var o = other as CardianRelationship;
            return o.Parent.Equals(Parent) && o.Child.Equals(Child) ? 0 : 1;
        }
    }
}