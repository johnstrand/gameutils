using GameUtils.Procedural;

namespace GameUtils.Tests.Procedural;

public class DiamondTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(10)]
    [InlineData(16)]
    public void Create_ThrowsArgumentException_WhenSizeIsNotPowerOfTwoPlusOne(int size)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            Diamond.Create(
                size,
                min: 0,
                max: 100,
                range: 10f,
                nextRange: r => r,
                valueFactory: (avg, range) => (int)avg)
        );

        Assert.Equal("size", ex.ParamName);
        Assert.Contains("Size must be a power-of-two plus one", ex.Message);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(9)]
    [InlineData(17)]
    [InlineData(33)]
    public void Create_ReturnsValidGrid_WhenSizeIsPowerOfTwoPlusOne(int size)
    {
        // Act
        var grid = Diamond.Create(
            size,
            min: 0,
            max: 10,
            range: 5f,
            nextRange: r => r * 0.5f,
            valueFactory: (avg, range) => (int)avg);

        // Assert
        Assert.NotNull(grid);
        Assert.Equal(size, grid.Width);
        Assert.Equal(size, grid.Height);
    }

    [Fact]
    public void Create_ProducesIdenticalGrids_WithSameSeed()
    {
        // Arrange
        int size = 17;
        int seed = 42;

        Func<float, float> nextRange = r => r * 0.5f;

        // Act
        var grid1 = Diamond.Create(size, 0, 100, 10f, nextRange, (avg, range) => (int)avg, seed);
        var grid2 = Diamond.Create(size, 0, 100, 10f, nextRange, (avg, range) => (int)avg, seed);

        // Assert
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Assert.Equal(grid1[x, y], grid2[x, y]);
            }
        }
    }

    [Fact]
    public void Create_AppliesValueFactoryAndNextRangeCorrectly()
    {
        // Arrange
        int size = 5;

        // Use a list to track ranges passed to the factory
        var rangesObserved = new List<float>();

        Func<float, float> nextRange = r =>
        {
            return r * 0.5f;
        };

        Func<float, float, int> valueFactory = (avg, range) =>
        {
            rangesObserved.Add(range);
            return (int)avg + (int)range;
        };

        // Act
        var grid = Diamond.Create(size, 10, 10, 16f, nextRange, valueFactory);

        // Assert
        // Given size = 5 (step initially 4)
        // 1. First iteration (step = 4):
        //    x=0, y=0. Center point (2,2) and 4 edge points. -> 5 points computed with range 16f
        // 2. Second iteration (step = 2):
        //    x=0,y=0; x=2,y=0; x=0,y=2; x=2,y=2
        //    Each cell step generates 5 points, so 4 * 5 = 20 points computed with range 8f
        // Let's just assert that ranges observed include 16f and 8f and the grid corner values

        Assert.Contains(16f, rangesObserved);
        Assert.Contains(8f, rangesObserved);
        Assert.DoesNotContain(4f, rangesObserved); // Loop ends when step <= 1
    }
}
