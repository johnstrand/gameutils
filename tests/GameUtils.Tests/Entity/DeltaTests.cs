using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class DeltaTests
{
    [TestMethod]
    public void Instance_IsNotNullAndSingleton()
    {
        var instance1 = Delta.Instance;
        var instance2 = Delta.Instance;

        Assert.IsNotNull(instance1);
        Assert.AreSame(instance1, instance2);
    }
}
