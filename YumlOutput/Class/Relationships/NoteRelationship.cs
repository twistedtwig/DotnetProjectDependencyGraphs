using YumlOutput.Class.Models;

namespace YumlOutput.Class.Relationships
{
    public class NoteRelationship : YumlRelationshipBase
    {
        public YumlModel Item { get; set; }
        public YumlNote Note { get; set; }

        protected override string GenerateRelationMap()
        {
            return string.Format("{0}-{1}", Item, Note);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is NoteRelationship))
            {
                return -1;
            }

            var o = other as NoteRelationship;
            return o.Item.Equals(Item) && o.Note.Equals(Note) ? 0 : 1;
        }
    }
}