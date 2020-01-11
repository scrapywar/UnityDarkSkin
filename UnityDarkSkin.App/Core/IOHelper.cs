﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace UnityDarkSkin.App.Core
{
    public class IOHelper
    {
        public static void OpenFolderDialog(string initialDirectory, Action<string> onDone = null, Action onCancel = null)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = initialDirectory
            };
            OpenDialog(dialog, onDone, onCancel);
        }

        public static void OpenFileDialog(string initialDirectory, Action<string> onDone = null, Action onCancel = null)
        {
            var dialog = new CommonOpenFileDialog {InitialDirectory = initialDirectory};
            OpenDialog(dialog, onDone, onCancel);
        }

        public static void OpenDialog(CommonOpenFileDialog dialog, Action<string> onDone, Action onCancel = null)
        {
            var result = dialog.ShowDialog();

            switch (result)
            {
                case CommonFileDialogResult.Ok:
                    onDone?.Invoke(dialog.FileName);
                    break;
                case CommonFileDialogResult.Cancel:
                    onCancel?.Invoke();
                    break;
            }
        }

        //

        public static string[] SearchFile(string directory, string fileName, bool recursive = true, bool containsName = false)
        {
            List<string> files = new List<string>();
            InternalSearchFile(ref files, directory, fileName, recursive, containsName);
            return files.ToArray();
        }

        private static void InternalSearchFile(ref List<string> files, string directory, string fileName, bool recursive, bool containsName)
        {
            if (Directory.Exists(directory))
            {
                string[] sub_dirs = new string[0];
                string[] matchingFiles = new string[0];

                try
                {
                    sub_dirs = Directory.GetDirectories(directory);
                    //
                    Func<string, bool> search;
                    if (containsName)
                        search = (p) => Path.GetFileName(p).Contains(fileName);
                    else
                        search = (p) => Path.GetFileName(p).Equals(fileName);

                    matchingFiles = Directory.GetFiles(directory).Where(search).ToArray();
                    files.AddRange(matchingFiles);
                }
                catch
                {
                    // Don't care
                }
                //

                foreach (string dir in sub_dirs)
                {
                    if (recursive)
                        InternalSearchFile(ref files, dir, fileName, recursive, containsName);
                }
            }
        }
    }
}