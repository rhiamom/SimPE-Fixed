using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SimPe
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Helper.LoadGameRootFromFile();
                InitializeWrappers();
                desktop.MainWindow = new MainForm();
            }
            base.OnFrameworkInitializationCompleted();
        }

        private static void InitializeWrappers()
        {
            var tr = new SimPe.PackedFiles.TypeRegistry();
            FileTable.ProviderRegistry    = tr;
            FileTable.ToolRegistry        = tr;
            FileTable.WrapperRegistry     = tr;
            FileTable.CommandLineRegistry = tr;
            FileTable.HelpTopicRegistry   = tr;
            FileTable.SettingsRegistry    = tr;

            var wloader = new LoadFileWrappersExt();

            // Static (built-in) wrappers
            FileTable.WrapperRegistry.Register(new SimPe.CommandlineHelpFactory());
            FileTable.WrapperRegistry.Register(new SimPe.Custom.SettingsFactory());
            FileTable.WrapperRegistry.Register(new SimPe.PackedFiles.Wrapper.Factory.SimFactory());
            FileTable.WrapperRegistry.Register(new SimPe.PackedFiles.Wrapper.Factory.ExtendedWrapperFactory());
            FileTable.WrapperRegistry.Register(new SimPe.PackedFiles.Wrapper.Factory.DefaultWrapperFactory());
            FileTable.WrapperRegistry.Register(new SimPe.Plugin.ScenegraphWrapperFactory());
            FileTable.WrapperRegistry.Register(new SimPe.Plugin.RefFileFactory());
            FileTable.WrapperRegistry.Register(new SimPe.PackedFiles.Wrapper.Factory.ClstWrapperFactory());

            // Dynamic plugin DLLs from the Plugins folder
            string folder = Helper.SimPePluginPath;
            if (System.IO.Directory.Exists(folder))
            {
                var files = new System.Collections.Generic.List<string>(
                    System.IO.Directory.GetFiles(folder, "*.plugin.dll"));
                files.AddRange(System.IO.Directory.GetFiles(folder, "*.wizard.dll"));
                foreach (string file in files)
                {
                    try { LoadFileWrappersExt.LoadWrapperFactory(file, wloader); } catch { }
                    // Tool factories are WinForms UI panels — not loaded on this platform
                }
            }
        }
    }
}
