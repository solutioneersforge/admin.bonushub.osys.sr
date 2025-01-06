namespace Client.Constants
{
    public class Env
    {
#if DEBUG
        public const bool Production = false;
#else
        public const bool Production = true;
#endif
    }
}
