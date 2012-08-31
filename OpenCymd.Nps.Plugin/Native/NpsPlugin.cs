namespace OpenCymd.Nps.Plugin.Native
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using log4net;

    /// <summary>
    /// Provides the entry points for the NPS service (indirectly called by the C++/CLR wrapper).
    /// </summary>
    public static class NpsPlugin
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(NpsPlugin));

        private static IEnumerable<Lazy<INpsPlugin>> plugins;

        public static uint RadiusExtensionInit()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(@"C:\Program Files\OpenCymd\Nps\NpsPlugin.config"));
            Logger.Info("Initializing NPS plugin.");
            var mefContainer =
                new CompositionContainer(
                    new DirectoryCatalog(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "OpenCymd.Nps.*Plugin.dll"));
            plugins = mefContainer.GetExports<INpsPlugin>();
            foreach (var plugin in plugins)
            {
                Logger.InfoFormat("Initializing plugin {0}", plugin.Value.GetType().FullName);
                try
                {
                    plugin.Value.Initialize();
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to initialize a plugin", ex);
                }
            }

            return 0;
        }

        public static void RadiusExtensionTerm()
        {
            Logger.Info("Terminating NPS plugin.");
        }

        public static uint RadiusExtensionProcess2(IntPtr pECB)
        {
            var ec = new ExtensionControl(pECB);
            var hadError = false;
            foreach (var plugin in plugins)
            {
                try
                {
                    plugin.Value.RadiusExtensionProcess(ec);
                }
                catch (Exception ex)
                {
                    Logger.Error("Plugin failed", ex);
                    hadError = true;
                }
            }

            return (uint)(hadError ? 5 : 0);
        }
    }
}
