using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;


public class GameBuilder
{
    struct BuildConfiguration
    {
        public BuildTarget buildTarget;
        public string pathSuffix;
        public string extension;

        public BuildConfiguration(BuildTarget buildTarget, string pathSuffix, string extension)
        {
            this.buildTarget = buildTarget;
            this.pathSuffix = pathSuffix;
            this.extension = extension;
        }
    }
    public static void Build()
    {
        string gameName = "JustAnotherDelivery";
        string buildDir = "bin";

        // Scenes must be in build order
        string[] scenes = {
            "Assets/Scenes/BootstrapScene.unity",
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/InventorySortingScene.unity",
            "Assets/Scenes/PackageDeliveryScene.unity",
            "Assets/Scenes/UpgradeMenuScene.unity"};

        List<BuildConfiguration> configs = new List<BuildConfiguration> {
            new BuildConfiguration(BuildTarget.StandaloneWindows, "_win32", ".exe" ),
            new BuildConfiguration(BuildTarget.StandaloneWindows64, "_win64", ".exe" ),
            new BuildConfiguration(BuildTarget.StandaloneOSX, "_mac", "" ) };

        foreach (var config in configs)
        {
            string buildLabel = gameName + config.pathSuffix;
            string fileName = gameName + config.extension;
            string path = buildDir + '/' + buildLabel + '/' + fileName;

            var report = BuildPipeline.BuildPlayer(scenes, path, config.buildTarget, BuildOptions.None);

            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build successful - Build written to {path}");
            }
            else if (report.summary.result == BuildResult.Failed)
            {
                Debug.LogError($"Build failed");
            }
        }
    }
}
