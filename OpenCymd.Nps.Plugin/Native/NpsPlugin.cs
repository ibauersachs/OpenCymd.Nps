namespace OpenCymd.Nps.Plugin.Native
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using log4net;

    /// <summary>
    /// Provides the entry points for the NPS service (indirectly called by the C++/CLR wrapper).
    /// </summary>
    internal static class NpsPlugin
    {
        private const string InstallationDirectory = @"C:\Program Files\OpenCymd\Nps";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(NpsPlugin));

        private static readonly object SyncLock = new object();

        private static FileSystemWatcher watcher;

        private static AppDomain pluginDomain;

        private static List<INpsPlugin> plugins;

        /// <summary>
        /// Called by the NPS host to initialize the plugin.
        /// </summary>
        /// <returns>Always 0.</returns>
        public static uint RadiusExtensionInit()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(InstallationDirectory, "NpsPlugin.config")));
            if (pluginDomain == null)
            {
                Logger.Info("Initializing NPS plugin.");
            }
            else
            {
                Logger.Info("Not initializing, plugin is already loaded.");
                return 0;
            }

            try
            {
                watcher = new FileSystemWatcher(InstallationDirectory, "OpenCymd.Nps.*Plugin.dll");
                watcher.Changed += PluginsChanged;
                watcher.Renamed += PluginsChanged;
                watcher.Created += PluginsChanged;
                watcher.Deleted += PluginsChanged;
                watcher.EnableRaisingEvents = true;

                LoadPluginsIntoAppDomain();
                Logger.Info("NPS Plugin loaded.");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to initialize NPS base plugin.", ex);
            }

            return 0;
        }

        /// <summary>
        /// Called by the NPS host to terminate the plugin.
        /// </summary>
        public static void RadiusExtensionTerm()
        {
            Logger.Info("Terminating NPS plugin...");
            UnloadPlugins();
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            Logger.Info("done.");
        }

        /// <summary>
        /// Called by the NPS host to process an authentication or authorization request.
        /// </summary>
        /// <param name="pEcb">Pointer to the extension control block.</param>
        /// <returns>0 if all plugins were processed successfully or 5 (access denied) when at least one of the plugins failed.</returns>
        public static uint RadiusExtensionProcess2(IntPtr pEcb)
        {
            bool hadError;
            lock (SyncLock)
            {
                var pr = (PluginRunner)pluginDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(PluginRunner).FullName);
                pr.EcbPointer = pEcb;
                pr.CallPlugins();
                hadError = pr.HadError;
            }

            return (uint)(hadError ? 5 : 0);
        }

        private static void LoadPluginsIntoAppDomain()
        {
            lock (SyncLock)
            {
                pluginDomain = AppDomain.CreateDomain("PluginDomain");
                pluginDomain.UnhandledException += (sender, e) => Logger.Error("Plugin app-domain crashed.", (Exception)e.ExceptionObject);
                pluginDomain.DoCallBack(() => log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(InstallationDirectory, "NpsPlugin.config"))));
                pluginDomain.DoCallBack(LoadPlugins);
            }
        }

        private static void PluginsChanged(object sender, FileSystemEventArgs e)
        {
            Logger.Info("Plugins changed, reloading...");
            lock (SyncLock)
            {
                UnloadPlugins();
                LoadPluginsIntoAppDomain();
            }
        }

        private static void UnloadPlugins()
        {
            if (pluginDomain == null)
            {
                return;
            }

            lock (SyncLock)
            {
                pluginDomain.DoCallBack(
                    () =>
                        {
                            foreach (var plugin in plugins.OfType<IDisposable>())
                            {
                                plugin.Dispose();
                            }
                        });

                var tempFolder = (string)pluginDomain.GetData("tempFolder");
                AppDomain.Unload(pluginDomain);
                pluginDomain = null;
                Directory.Delete(tempFolder, true);
            }
        }

        private static void LoadPlugins()
        {
            var tempFolder = Directory.CreateDirectory(Path.Combine(InstallationDirectory, Guid.NewGuid().ToString("N"))).FullName;
            AppDomain.CurrentDomain.SetData("tempFolder", tempFolder);
            foreach (var f in Directory.GetFiles(InstallationDirectory, "*.dll"))
            {
                File.Copy(f, Path.Combine(tempFolder, new FileInfo(f).Name));
            }

            plugins = (from pluginAssembly in Directory.GetFiles(tempFolder, "OpenCymd.Nps.*Plugin.dll")
                       select Assembly.LoadFrom(pluginAssembly) into assembly
                       from type in assembly.GetTypes().Where(t => typeof(INpsPlugin).IsAssignableFrom(t) && t != typeof(INpsPlugin))
                       select (INpsPlugin)Activator.CreateInstance(type)).ToList();

            Logger.InfoFormat("Loading {0} plugins", plugins.Count);
            foreach (var plugin in plugins)
            {
                Logger.InfoFormat("Initializing plugin {0}", plugin.GetType().FullName);
                try
                {
                    plugin.Initialize();
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to initialize a plugin", ex);
                }
            }
        }

        [Serializable]
        private class PluginRunner : MarshalByRefObject
        {
            public IntPtr EcbPointer { private get; set; }

            public bool HadError { get; private set; }

            public void CallPlugins()
            {
                Logger.Debug("Processing RADIUS message");
                var ec = new ExtensionControl(this.EcbPointer);
// ReSharper disable RedundantNameQualifier -> this is indeed necessary!
                foreach (var plugin in NpsPlugin.plugins)
// ReSharper restore RedundantNameQualifier
                {
                    try
                    {
                        Logger.DebugFormat("Calling plugin {0} with pointer {1}", plugin.GetType().FullName, this.EcbPointer);
                        plugin.RadiusExtensionProcess(ec);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Plugin failed", ex);
                        try
                        {
                            switch (ec.RequestType)
                            {
                                case RadiusCode.AccessRequest:
                                    ec.ResponseType = RadiusCode.AccessReject;
                                    break;
                                case RadiusCode.AccountingRequest:
                                    ec.ResponseType = RadiusCode.Discard;
                                    break;
                            }
                        }
                        catch (Exception inner)
                        {
                            Logger.Error("Setting response type after plugin failure failed", inner);
                            this.HadError = true;
                        }
                    }
                }
            }
        }
    }
}
