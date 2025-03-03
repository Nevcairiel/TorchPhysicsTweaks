using NLog;
using System.IO;
using System;
using System.Windows.Controls;
using Torch;
using Torch.API;
using Torch.API.Plugins;
using VRageMath.Spatial;

namespace PhysicsTweaks
{
    public class PhysicsTweaksPlugin : TorchPluginBase, IWpfPlugin
    {

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static PhysicsTweaksPlugin Instance { get; private set; }

        private static readonly string CONFIG_FILE_NAME = "PhysicsTweaksConfig.cfg";

        private PhysicsTweaksControl _control;
        public UserControl GetControl() => _control ?? (_control = new PhysicsTweaksControl(this));

        private Persistent<PhysicsTweaksConfig> _config;
        public PhysicsTweaksConfig Config => _config?.Data;

        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            Instance = this;
            SetupConfig();

            SetupClusterSize(Config.ClusterSize);
        }

        private void SetupClusterSize(float size)
        {
            var clusterSize = new VRageMath.Vector3(size);
            MyClusterTree.IdealClusterSize = clusterSize;
            MyClusterTree.IdealClusterSizeHalfSqr = clusterSize * clusterSize / 4f;
            MyClusterTree.MinimumDistanceFromBorder = clusterSize / 50f;
            MyClusterTree.MaximumForSplit = clusterSize * 2f;
            MyClusterTree.MaximumClusterSize = size * 5f;
        }

        private void SetupConfig()
        {

            var configFile = Path.Combine(StoragePath, CONFIG_FILE_NAME);

            try
            {
                _config = Persistent<PhysicsTweaksConfig>.Load(configFile);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null)
            {
                Log.Info("Create Default Config, because none was found!");

                _config = new Persistent<PhysicsTweaksConfig>(configFile, new PhysicsTweaksConfig());
                _config.Save();
            }
        }

        public void Save()
        {
            try
            {
                _config.Save();
                Log.Info("Configuration Saved.");
            }
            catch (IOException e)
            {
                Log.Warn(e, "Configuration failed to save");
            }

            SetupClusterSize(Config.ClusterSize);
        }
    }
}
