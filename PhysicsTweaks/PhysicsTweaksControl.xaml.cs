using System.Windows;
using System.Windows.Controls;

namespace PhysicsTweaks
{
    public partial class PhysicsTweaksControl : UserControl
    {

        private PhysicsTweaksPlugin Plugin { get; }

        private PhysicsTweaksControl()
        {
            InitializeComponent();
        }

        public PhysicsTweaksControl(PhysicsTweaksPlugin plugin) : this()
        {
            Plugin = plugin;
            DataContext = plugin.Config;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Plugin.Save();
        }
    }
}
