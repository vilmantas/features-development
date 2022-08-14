namespace Features.Buffs
{
    public class BuffRemoveOptions
    {
        public BuffRemoveOptions()
        {
        }

        public BuffRemoveOptions(BuffBase buff)
        {
            Buff = buff;
        }

        public BuffBase Buff { get; set; }
        public bool RequestHandled { get; set; }
    }
}