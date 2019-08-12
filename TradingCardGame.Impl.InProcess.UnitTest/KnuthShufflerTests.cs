using System;
using System.Linq;
using Xunit;

namespace TradingCardGame.Impl.InProcess.UnitTest
{
    public class KnuthShufflerTests
    {
        [Fact]
        public void Test_Knuth_Shuffler_Uses_All_Elements_From_Given_Enumerable()
        {
            var knuthShuffler = new KnuthShuffler();
            var initial = new byte[] {0, 1, 2, 3, 3}.ToList();
            var shuffled = knuthShuffler.Shuffle(initial);
            
            Assert.Equal(initial.Count, shuffled.Count());
            foreach(var elm in shuffled)
            {
                initial.Remove(elm);
            }
            Assert.Empty(initial);
        }
    }
}