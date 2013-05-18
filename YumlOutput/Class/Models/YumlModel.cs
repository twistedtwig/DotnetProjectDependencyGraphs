using System;

namespace YumlOutput.Class.Models
{
    public abstract class YumlModel : IComparable<YumlModel>
    {
        protected YumlModel()
        {
            BackGroundColour = string.Empty;
        }

        protected abstract string Draw();
        protected abstract int Compare<T>(T other) where T : YumlModel;
        public string BackGroundColour { get; set; }

        public override string ToString()
        {
            string draw = Draw();

            if (!string.IsNullOrWhiteSpace(BackGroundColour) && !draw.Contains("{bg:"))
            {
                draw = draw.Substring(0, draw.Length - 1) + string.Format("{{bg:{0}}}]", BackGroundColour);
            }

            return draw;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj as YumlModel) == 0;
        }

        public int CompareTo(YumlModel other)
        {
            return Compare(other);
        }


    }
}