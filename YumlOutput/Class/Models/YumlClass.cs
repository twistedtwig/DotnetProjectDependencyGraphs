namespace YumlOutput.Class.Models
{
    public class YumlClass : YumlModel
    {
        public string Name { get; set; }

        protected override string Draw()
        {
            return string.Format("[{0}]", Name);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is YumlClass))
            {
                return -1;
            }

            var o = other as YumlClass;
            return Name == o.Name && BackGroundColour == o.BackGroundColour ? 0 : 1;
        }
    }
}