namespace SharpCraft.Core.Util
{
    public interface IProgressListener
    {
        void StartLoading(string str);

        void DisplayLoadingString(string str);

        void SetLoadingProgress(int progress);
    }
}
