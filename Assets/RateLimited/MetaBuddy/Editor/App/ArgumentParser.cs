using System;
using System.Collections.Generic;
using System.IO;
using MetaBuddyLib.Config;

namespace MetaBuddy.App
{
    public static class ArgumentParser
    {
        private const string CheckOption = "-mb-check";
        private const string AllCheckName = "all";
        private const string ChangesCheckName = "changes";

        private const string ConfigOption = "-mb-config";
        private const string VerboseOption = "-mb-verbose";
        private const string AnalyseOnEditorStartupOption = "-mb-analyse-on-editor-startup";
        private const string LogIgnoredFiles = "-mb-log-ignored-files";

        private static void ParseConfigOption(string arg, Queue<string> argQueue, ConfigModel model)
        {
            if (arg == ConfigOption)
            {
                model.Config = argQueue.Count > 0
                    ? new FileInfo(argQueue.Dequeue())
                    : throw new ArgumentException($"Expected '{ConfigOption}' to be followed by a path to a config file.");
            }
        }

        private static void ParseCheckOption(string arg, Queue<string> argQueue, ConfigModel model)
        {
            if (arg == CheckOption)
            {
                if (argQueue.Count > 0)
                {
                    var check = argQueue.Dequeue();

                    switch (check)
                    {
                        case AllCheckName:
                            {
                                model.CheckCommand = CheckCommand.All;
                                return;
                            }

                        case ChangesCheckName:
                            {
                                model.CheckCommand = CheckCommand.Changes;
                                return;
                            }

                        default:
                            {
                                throw new NotImplementedException("Unhandled check command");
                            }
                    }
                }

                throw new ArgumentException($"Expected '{CheckOption}' to be followed by either '{AllCheckName}' or '{ChangesCheckName}'.");
            }
        }

        private static void ParseVerboseOption(string arg, Queue<string> argQueue, ConfigModel model)
        {
            if (arg == VerboseOption)
            {
                model.Verbose = true;
            }
        }

        private static void ParseNoEditorStartOption(string arg, Queue<string> argQueue, ConfigModel model)
        {
            if (arg == AnalyseOnEditorStartupOption)
            {
                model.AnalyseOnEditorStartup = true;
            }
        }

        private static void ParseLogIgnoredFilesOption(string arg, Queue<string> argQueue, ConfigModel model)
        {
            if (arg == LogIgnoredFiles)
            {
                model.ListIgnoredFiles = true;
            }
        }

        private static void ParseArgument(string arg, Queue<string> argQueue, ConfigModel model)
        {
            ParseCheckOption(arg, argQueue, model);
            ParseConfigOption(arg, argQueue, model);
            ParseVerboseOption(arg, argQueue, model);
            ParseNoEditorStartOption(arg, argQueue, model);
            ParseLogIgnoredFilesOption(arg, argQueue, model);
        }

        public static ConfigModel ParseCommandLine()
        {
            var model = new ConfigModel();

            var argQueue = new Queue<string>(Environment.GetCommandLineArgs());

            while (argQueue.Count > 0)
            {
                var arg = argQueue.Dequeue();

                ParseArgument(arg, argQueue, model);
            }

            return model;
        }
    }
}

