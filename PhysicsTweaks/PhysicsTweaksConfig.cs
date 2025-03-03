using Torch;

namespace PhysicsTweaks
{
    public class PhysicsTweaksConfig : ViewModel
    {
        private int _ClusterSize = 5000;

        public int ClusterSize { get => _ClusterSize; set => SetValue(ref _ClusterSize, value); }
    }
}
