using Microsoft.Xna.Framework;
using MyGame.View;
using NUnit.Framework;

namespace MyGame.Tests
{
    [TestFixture]
    public class TestAnimation
    {
        [TestCase(64, 64, 512, 128, 0f, 0f, 1f, 0f, 64f, 0f)]
        [TestCase(64, 64, 512, 128, 64f, 0f, 1f, 0f, 64f, 0f)]
        [TestCase(64, 64, 512, 128, 0f, 0f, -1f, 0f, 64f, 0f)]
        [TestCase(64, 64, 512, 128, 64f, 0f, -1f, 0f, 64f, 0f)]
        public void AnimateObject_ShouldReturnCorrectPosition(
            int width, int height, int widthImage, int heightImage,
            float posX, float posY, float diffX, float diffY,
            float expectedX, float expectedY)
        {
            // Arrange
            var imagePos = new Vector2(posX, posY);
            var different = new Vector2(diffX, diffY);
            var expected = new Vector2(expectedX, expectedY);

            // Act
            var actual = Animation.AnimateObject(width, height, widthImage, imagePos, different);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}