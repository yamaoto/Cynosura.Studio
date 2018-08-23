﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Merge
{
    public class DmpMerge : IMerge
    {
        private async Task<string> ReadFileAsync(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return await fileReader.ReadToEndAsync();
            }
        }

        private async Task WriteFileAsync(string filePath, string content)
        {
            using (var fileWriter = new StreamWriter(filePath))
            {
                await fileWriter.WriteAsync(content);
            }
        }

        private void EnsureDirectoryExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public async Task MergeFileAsync(string originalFilePath, string theirFilePath, string myFilePath)
        {
            var original = await ReadFileAsync(originalFilePath);
            var their = await ReadFileAsync(theirFilePath);
            if (original == their)
                return;
            var my = await ReadFileAsync(myFilePath);
            var dmp = new DiffMatchPatch.diff_match_patch();
            var patches = dmp.patch_make(original, their);
            var result = dmp.patch_apply(patches, my);
            var resultText = (string) result[0];
            if (resultText == my)
                return;
            await WriteFileAsync(myFilePath, resultText);
        }

        private IList<string> GetFiles(string directoryPath)
        {
           return Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        }

        private IList<FileCompare> Compare(string leftPath, string rightPath)
        {
            var leftFiles = GetFiles(leftPath)
                .Select(f => new FileCompare()
                {
                    Name = Path.GetRelativePath(leftPath, f),
                    LeftPath = f,
                });
            var rightFiles = GetFiles(rightPath)
                .Select(f => new FileCompare()
                {
                    Name = Path.GetRelativePath(rightPath, f),
                    RightPath = f,
                });
            return leftFiles.Concat(rightFiles)
                .GroupBy(f => f.Name)
                .Select(g => new FileCompare()
                {
                    Name = g.Key,
                    LeftPath = g.Select(i => i.LeftPath).FirstOrDefault(i => i != null),
                    RightPath = g.Select(i => i.RightPath).FirstOrDefault(i => i != null),
                })
                .ToList();
        }

        public async Task MergeDirectoryAsync(string originalDirectoryPath, string theirDirectoryPath, string myDirectoryPath)
        {
            var compareFiles = Compare(originalDirectoryPath, theirDirectoryPath);
            foreach (var compareFile in compareFiles)
            {
                var myFilePath = Path.Combine(myDirectoryPath, compareFile.Name);
                if (compareFile.LeftPath == null)
                {
                    var rightFileContent = await ReadFileAsync(compareFile.RightPath);
                    EnsureDirectoryExists(myFilePath);
                    await WriteFileAsync(myFilePath, rightFileContent);
                }
                else if (compareFile.RightPath == null)
                {
                    if (File.Exists(myFilePath))
                        File.Delete(myFilePath);
                }
                else
                {
                    await MergeFileAsync(compareFile.LeftPath, compareFile.RightPath, myFilePath);
                }
            }
        }

        class FileCompare
        {
            public string Name { get; set; }
            public string LeftPath { get; set; }
            public string RightPath { get; set; }
        }
    }
}