using System;

namespace YumlOutput.Class.Relationships
{
    public abstract class YumlRelationshipBase : IComparable<YumlRelationshipBase> 
    {
        protected abstract string GenerateRelationMap();
        protected abstract int Compare<T>(T other) where T : YumlRelationshipBase;


        public int CompareTo(YumlRelationshipBase other)
        {
            return Compare(other);
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj as YumlRelationshipBase) == 0;
        }

        public override string ToString()
        {
            return GenerateRelationMap();
        }
    }
}