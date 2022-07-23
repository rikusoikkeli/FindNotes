using FindNotes.Interfaces;
using FindNotes.Models;
using FindNotes.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace FindNotes.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private string _programName = "FindNotes.dll";
        private string _resultsLocation = Directory.GetCurrentDirectory() + @"\TestData\TestOutputs";
        private string _cmdStart = @"C:\Users\riku.soikkeli\source\repos\FindNotesSolution2\FindNotes\bin\Debug\net5.0\FindNotes.dll";
        private string _cmdQuery = "data";
        private string _cmdRootPath = Directory.GetCurrentDirectory() + @"\TestData\FoldersToSearch";
        private string _referenceData1Path = Directory.GetCurrentDirectory() + $"\\TestData\\ReferenceData\\ReferenceData1.txt";
        private string _referenceData2Path = Directory.GetCurrentDirectory() + $"\\TestData\\ReferenceData\\ReferenceData2.txt";
        private string _referenceData3Path = Directory.GetCurrentDirectory() + $"\\TestData\\ReferenceData\\ReferenceData3.txt";
        private string _referenceData4Path = Directory.GetCurrentDirectory() + $"\\TestData\\ReferenceData\\ReferenceData4.txt";
        private IServiceProvider _serviceProvider;
        private string _testSettingsPath = "testsettings.json";
        private ReadWriteSettings _readWriteSettings;

        [TestInitialize]
        public void TestInit()
        {
            var globalOptions = new GlobalOptions()
            {
                ProgramName = _programName,
                InTestMode = true,
            };
            var readWriteSettings = new ReadWriteSettings(globalOptions);
            _serviceProvider = new ServiceCollection()
                .AddScoped<IFileOpener, FileOpener>()
                .AddSingleton<IReadWriteSettings>(readWriteSettings)
                .AddScoped<IOutput, WriteToFile>()
                .AddSingleton<IGlobalOptions>(globalOptions)
                .BuildServiceProvider();

            _readWriteSettings = (ReadWriteSettings)_serviceProvider.GetService<IReadWriteSettings>();
            EmptyResultsDirectory();
            DeleteTestSettingsIfExists();
        }

        [TestMethod]
        public void FindNotes_NoSpacesInFilePathAndNoQuotes_ReturnsCorrectResults()
        {
            // Arrange
            var cmdPath = _cmdRootPath + @"\folder123" + "\n";
            string cmdLine = $"{_cmdStart} -q {_cmdQuery} -p {cmdPath}";
            var referenceFile = File.ReadAllText(_referenceData1Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_SpacesInFilePathAndNoQuotes_ReturnsCorrectResults()
        {
            // Arrange
            var cmdPath = _cmdRootPath + @"\folder - 123" + "\n";
            string cmdLine = $"{_cmdStart} -q {_cmdQuery} -p {cmdPath}";
            var referenceFile = File.ReadAllText(_referenceData1Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_NoSpacesInFilePathAndQuotes_ReturnsCorrectResults()
        {
            // Arrange
            var cmdPath = _cmdRootPath + @"\folder123" + "\n";
            string cmdLine = $"{_cmdStart} -q \"{_cmdQuery}\" -p \"{cmdPath}\"";
            var referenceFile = File.ReadAllText(_referenceData1Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_SpacesInFilePathAndQuotes_ReturnsCorrectResults()
        {
            // Arrange
            var cmdPath = _cmdRootPath + @"\folder - 123" + "\n";
            string cmdLine = $"{_cmdStart} -q \"{_cmdQuery}\" -p \"{cmdPath}\"";
            var referenceFile = File.ReadAllText(_referenceData1Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_SpacesInFilePathAndQuotes_ReturnsNoResults()
        {
            // Arrange
            var cmdQuery = "aödslkjaöladf";
            var cmdPath = _cmdRootPath + @"\folder - 123" + "\n";
            string cmdLine = $"{_cmdStart} -q \"{cmdQuery}\" -p \"{cmdPath}\"";
            var referenceFile = File.ReadAllText(_referenceData1Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo("\nNo matches found!\n\r\n");
        }

        [TestMethod]
        public void FindNotes_SpacesInFilePathAndQuotesAndSpacesInQuery_ReturnsCorrectResults()
        {
            // Arrange
            var cmdQuery = "key concepts";
            var cmdPath = _cmdRootPath + @"\folder - 123" + "\n";
            string cmdLine = $"{_cmdStart} -q \"{cmdQuery}\" -p \"{cmdPath}\"";
            var referenceFile = File.ReadAllText(_referenceData2Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_SpacesInFilePathAndQuotesAndHyphensAndNumbersInQuery_ReturnsCorrectResults()
        {
            // Arrange
            var cmdQuery = "az204-evgrid-rg";
            var cmdPath = _cmdRootPath + @"\folder - 123" + "\n";
            string cmdLine = $"{_cmdStart} -q \"{cmdQuery}\" -p \"{cmdPath}\"";
            var referenceFile = File.ReadAllText(_referenceData3Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_SaveAPath_PathIsFoundFromSettings()
        {
            // Arrange
            var cmdSavePath = _cmdRootPath + @"\folder123";
            var cmdNickname = "foo";
            string cmdLine = $"{_cmdStart} -s {cmdSavePath} -n {cmdNickname}\n";
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);

            // Assert
            Settings s = _readWriteSettings.ReadSettings();
            s.SavedPaths.Should().NotBeNull();
            s.SavedPaths[cmdNickname].Should().BeEquivalentTo(cmdSavePath);
        }

        [TestMethod]
        public void FindNotes_SaveTwoDifferentPaths_PathsAreFoundFromSettings()
        {
            // Arrange
            var cmdSavePath1 = _cmdRootPath + @"\folder123";
            var cmdNickname1 = "foo";
            string cmdLine1 = $"{_cmdStart} -s {cmdSavePath1} -n {cmdNickname1}\n";
            var findNotes1 = new FindNotes(_serviceProvider);

            var cmdSavePath2 = _cmdRootPath + @"\folder - 123";
            var cmdNickname2 = "bar";
            string cmdLine2 = $"{_cmdStart} -s {cmdSavePath2} -n {cmdNickname2}\n";
            var findNotes2 = new FindNotes(_serviceProvider);

            // Act
            findNotes1.Run(cmdLine1);
            findNotes2.Run(cmdLine2);

            // Assert
            Settings s = _readWriteSettings.ReadSettings();
            s.SavedPaths.Should().NotBeNull();
            s.SavedPaths[cmdNickname1].Should().BeEquivalentTo(cmdSavePath1);
            s.SavedPaths[cmdNickname2].Should().BeEquivalentTo(cmdSavePath2);
        }

        [TestMethod]
        public void FindNotes_SaveSamePathTwice_OnlyOnePathIsFoundFromSettings()
        {
            // Arrange
            var cmdSavePath = _cmdRootPath + @"\folder123";
            var cmdNickname = "foo";
            string cmdLine = $"{_cmdStart} -s {cmdSavePath} -n {cmdNickname}\n";
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            findNotes.Run(cmdLine);

            // Assert
            Settings s = _readWriteSettings.ReadSettings();
            s.SavedPaths.Count.Should().Be(1);
            s.SavedPaths[cmdNickname].Should().BeEquivalentTo(cmdSavePath);
        }

        [TestMethod]
        public void FindNotes_PerformAQueryUsingASavedPath_ReturnsCorrectResults()
        {
            // Arrange
            string settings = "{\"SavedPaths\":{\"foo\":\"C:\\\\Users\\\\riku.soikkeli\\\\source\\\\repos\\\\FindNotesSolution2\\\\FindNotes.Tests\\\\bin\\\\Debug\\\\net5.0\\\\TestData\\\\FoldersToSearch\\\\folder123\"}}\r\n";
            File.WriteAllText(_testSettingsPath, settings);
            var cmdPath = "foo" + "\n";
            string cmdLine = $"{_cmdStart} -q {_cmdQuery} -p {cmdPath}";
            var referenceFile = File.ReadAllText(_referenceData1Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_PerformAListingOfSavedPaths_ReturnsCorrectResults()
        {
            // Arrange
            string settings = "{\"SavedPaths\":{\"foo\":\"C:\\\\Users\\\\riku.soikkeli\\\\source\\\\repos\\\\FindNotesSolution2\\\\FindNotes.Tests\\\\bin\\\\Debug\\\\net5.0\\\\TestData\\\\FoldersToSearch\\\\folder123\"}}\r\n";
            File.WriteAllText(_testSettingsPath, settings);
            var cmdQuery = "-l savedPaths" + "\n";
            string cmdLine = $"{_cmdStart} {cmdQuery}";
            var referenceFile = File.ReadAllText(_referenceData4Path);
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo(referenceFile);
        }

        [TestMethod]
        public void FindNotes_PerformAListingOfSavedPathsButNoExist_ReturnsErrorMessage()
        {
            // Arrange
            var cmdQuery = "-l savedPaths" + "\n";
            string cmdLine = $"{_cmdStart} {cmdQuery}";
            var findNotes = new FindNotes(_serviceProvider);

            // Act
            findNotes.Run(cmdLine);
            var resultFile = ReturnNewestResultFile();

            // Assert
            resultFile.Should().BeEquivalentTo("\nNo saved paths exist!\n\r\n");
        }

        public string ReturnNewestResultFile()
        {
            var file = new DirectoryInfo(_resultsLocation)
                .GetFiles()
                .Where(f => f.Name.EndsWith(".txt"))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList().FirstOrDefault();

            var fileContents = File.ReadAllText(file.FullName);
            return fileContents;
        }

        public void EmptyResultsDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(_resultsLocation);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        public void DeleteTestSettingsIfExists()
        {
            bool exists = File.Exists(_testSettingsPath);
            if (exists)
            {
                File.Delete(_testSettingsPath);
            }
        }
    }
}
