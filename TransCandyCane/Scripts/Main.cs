using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TransCandyCane.Scripts
{
    public static class Main
    {
        private static Texture2D OutputTexture;

        // Config
        private static ConfigEntry<Color> NewColour1;
        private static ConfigEntry<Color> NewColour2;
        private static ConfigEntry<Color> NewColour3;

        public static void LoadConfig()
        {
            ColorUtility.TryParseHtmlString("#73ECFF", out Color ColourLine1);
            ColorUtility.TryParseHtmlString("#FD8BFF", out Color ColourLine2);
            ColorUtility.TryParseHtmlString("#E3E3E3", out Color ColourLine3);

            var customFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "TransCandyCane.cfg"), true);
            NewColour1 = customFile.Bind("Configuration", "Outer Line Colour", ColourLine1, "The colour of the outer line");
            NewColour2 = customFile.Bind("Configuration", "Inner Line Colour", ColourLine2, "The colour of the inner line");
            NewColour3 = customFile.Bind("Configuration", "Middle Line Colour", ColourLine3, "The colour of the middle line");
        }

        public static void LoadTexture()
        {
            if (OutputTexture != null) return;

            Texture2D tex = new Texture2D(48, 35, TextureFormat.RGBA32, false);
            tex.name = "MainTex";
            tex.filterMode = FilterMode.Point;
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("TransCandyCane.Resources.MainTex.png");

            try
            {
                // Load the image from the stream
                byte[] bytes = new byte[str.Length];
                str.Read(bytes, 0, bytes.Length);
                tex.LoadImage(bytes);
                tex.Apply();

                // Replace the colours of the image (https://stackoverflow.com/a/68636018)
                for (int y = 0; y < tex.height; y++)
                {
                    for (int x = 0; x < tex.width; x++)
                    {
                        if (tex.GetPixel(x, y) == Color.red) tex.SetPixel(x, y, NewColour1.Value);
                        else if (tex.GetPixel(x, y) == Color.green) tex.SetPixel(x, y, NewColour2.Value);
                        else if (tex.GetPixel(x, y) == Color.blue) tex.SetPixel(x, y, NewColour3.Value);
                    }
                }
                tex.Apply();
            }
            catch (Exception exception) { Debug.LogException(exception); }
            finally { OutputTexture = tex; }
        }

        public static void RegisterCandyCanes(TransferrableObject[] transferrableObjects)
            => Array.ForEach(transferrableObjects, TransferrableObject => RegisterCandyCane(TransferrableObject));

        public static void RegisterCandyCane(TransferrableObject transferrableObject)
            => transferrableObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = OutputTexture;
    }
}
