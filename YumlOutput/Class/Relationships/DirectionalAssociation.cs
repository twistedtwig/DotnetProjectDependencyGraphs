using YumlOutput.Class.Models;

namespace YumlOutput.Class.Relationships
{
    public class DirectionalAssociation : YumlRelationshipBase
    {
        public YumlModel Parent { get; set; }
        public YumlModel Child { get; set; }
        public string DirectionalMessage { get; set; }

        protected override string GenerateRelationMap()
        {
            return string.Format("{0}-{1} >{2}", Parent, DirectionalMessage, Child);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is DirectionalAssociation))
            {
                return -1;
            }

            var o = other as DirectionalAssociation;
            return o.Parent.Equals(Parent) && o.Child.Equals(Child) ? 0 : 1;
        }
    }
}