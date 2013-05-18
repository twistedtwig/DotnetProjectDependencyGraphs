using System;

namespace YumlOutput.Class.Models
{
    public class YumlInterface : YumlModel
    {
        public String Name { get; set; }

        protected override string Draw()
        {
            return string.Format("<<{0}>>", Name);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is YumlInterface))
            {
                return -1;
            }

            var o = other as YumlInterface;
            return this.Name == o.Name && BackGroundColour == o.BackGroundColour ? 0 : 1;
        }
    }
}
