using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Tests.Integ.Framework.XunitExtensions;
using Xunit;

[assembly: TestFramework(Constants.TestFramework, Constants.AssemblyName)]
[assembly: TestCollectionOrderer(Constants.TestCollectionOrderer, Constants.AssemblyName)]
[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: AssemblyFixture(typeof(TestsFixture))]
