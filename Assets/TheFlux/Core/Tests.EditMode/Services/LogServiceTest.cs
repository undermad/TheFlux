using System.Collections.Generic;
using NUnit.Framework;
using TheFlux.Core.Scripts.Services.LogService;
using UnityEngine;
using VContainer;
using Logger = TheFlux.Core.Scripts.Services.LogService.Logger;

namespace TheFlux.Core.Tests.EditMode.Services
{
    public class LogServiceTest
    {
        private List<(string condition, string stackTrace, LogType type)> capturedLogs = new();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SetupVContainerAndLogger();
            SetupLogCaptor();
        }

        [TearDown]
        public void TearDown()
        {
            capturedLogs.Clear();
        }

        [Test]
        public void ShouldLogInfoMessagesToTheConsole()
        {
            //Act
            LogService.Log("Test");

            //Assert
            var result = capturedLogs.Find(log => 
                log.stackTrace.Contains("Test") &&
                log.stackTrace.Contains("<color=#0000FF>[General]</color>") &&
                log.stackTrace.Contains("[Info]"));
            Assert.IsNotNull(result);
        }
    
        [Test]
        public void ShouldLogWarningMessagesToTheConsole()
        {
            //Act
            LogService.Log("UI Test", LogLevel.Warning, LogCategory.UI);

            //Assert
            var result = capturedLogs.Find(log => 
                log.stackTrace.Contains("UI Test") &&
                log.stackTrace.Contains("<color=#0000FF>[UI]</color>") &&
                log.stackTrace.Contains("[Warning]"));
            Assert.IsNotNull(result);
        }

        private static void SetupVContainerAndLogger()
        {
            var config = ScriptableObject.CreateInstance<LoggerConfig>();
            config.categories = new List<CategoryEntry>
            {
                new()
                {
                    logCategory = LogCategory.General,
                    active = true,
                    color = Color.blue
                },
                new()
                {
                    logCategory = LogCategory.UI,
                    active = true,
                    color = Color.green
                },
                new()
                {
                    logCategory = LogCategory.Error,
                    active = false,
                    color = Color.red
                }
            };
        
            var builder = new ContainerBuilder();
            builder.RegisterInstance(config.categories);
            builder.RegisterInstance(config);
            builder.Register<Logger>(Lifetime.Singleton).WithParameter(config);
            builder.RegisterBuildCallback(container =>
            {
                _ = container.Resolve<Logger>();
            });
            builder.Build();
        }
    
        private void SetupLogCaptor()
        {
            Application.logMessageReceived += Callback;
            return;

            void Callback(string condition, string stackTrace, LogType type)
            {
                capturedLogs.Add((condition, stackTrace, type));
            }
        }
    }
}
