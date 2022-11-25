namespace Features.Buffs
{
    public class BuffRemoveOptions
    {
        public BuffRemoveOptions()
        {
        }

        public BuffRemoveOptions(BuffMetadata buff)
        {
            Buff = buff;
        }

        public BuffMetadata Buff { get; set; }
        public bool RequestHandled { get; set; }
    }
}