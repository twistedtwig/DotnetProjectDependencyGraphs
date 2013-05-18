namespace YumlOutput.Class.Models
{
    public class YumlNote : YumlModel
    {
        public string Text { get; set; }

        protected override string Draw()
        {
            return string.Format("[note: {0}]", Text);
        }

        protected override int Compare<T>(T other)
        {
            if (!(other is YumlNote))
            {
                return -1;
            }

            var o = other as YumlNote;
            return this.Text == o.Text && BackGroundColour == o.BackGroundColour ? 0 : 1;
        }
    }
}