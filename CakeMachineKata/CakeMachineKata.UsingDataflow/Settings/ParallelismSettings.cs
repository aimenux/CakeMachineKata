namespace CakeMachineKata.UsingDataflow.Settings
{
    public class ParallelismSettings
    {
        public ParallelismSettings() : this(3, 5, 2)
        {
        }

        public ParallelismSettings(int prepareMaxDegree, int cookMaxDegree, int packageMaxDegree)
        {
            PrepareMaxDegree = prepareMaxDegree;
            CookMaxDegree = cookMaxDegree;
            PackageMaxDegree = packageMaxDegree;
        }

        public int PrepareMaxDegree { get; }

        public int CookMaxDegree { get; }

        public int PackageMaxDegree { get; }
    }
}