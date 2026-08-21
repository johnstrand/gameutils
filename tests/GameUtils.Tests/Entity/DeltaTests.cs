using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class DeltaTests
{
    [TestMethod]
    public void Instance_IsSingleton()
    {
        var instance1 = Delta.Instance;
        var instance2 = Delta.Instance;

        Assert.AreSame(instance1, instance2);
    }
}
