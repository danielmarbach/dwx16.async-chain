using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncChain
{
    [TestFixture]
    public class AsyncChainOO
    {
        #region BoringInfrastructureStuff

        StringWriter writer = new StringWriter();

        [SetUp]
        public void SetUp()
        {
            Console.SetOut(writer);
        }

        #endregion

        /*
         * Exercise 8: Now it is time to lift the things we learned into the OO world.
         * - First define the interface ILinkElement below
         * - Implement the Chain class
         * - Sequentially execute 3 chains (links should not be shared)
         */
        [Test]
        public async Task ExecuteThreeChainsSequentially()
        {

            for (int i = 0; i < 3; i++)
            {
                // TODO: Sequentially create a chain and execute it
            }

            Assert.That(writer.ToString(), Is.EqualTo(@"Son
Wife
Husband
Son
Wife
Husband
Son
Wife
Husband
"));
        }

        public class Chain
        {
            readonly List<ILinkElement> linkElements;

            public Chain(IEnumerable<ILinkElement> steps)
            {
                linkElements = new List<ILinkElement>(steps);
            }

            // TODO: Implement this method here
            public async Task Invoke()
            {
            }
        }

        // TODO: Define this interface
        public interface ILinkElement
        {
        }

        // TODO: Implement the interface properly
        class Son : ILinkElement
        {
            // Console.WriteLine("Son");
        }

        // TODO: Implement the interface properly
        class Wife : ILinkElement
        {
            // Console.WriteLine("Wife");
        }

        // TODO: Implement the interface properly
        class Husband : ILinkElement
        {
            // Console.WriteLine("Husband");
        }

        /*
         * Exercise 9: Reuse the infrastructure you wrote for Exercise 8 but this time execute the chain concurrently
         */
        [Test]
        public async Task ExecuteThreeChainsConcurrently()
        {
            var chainTasks = new Task[3];

            for (int i = 0; i < 3; i++)
            {
                // TODO: KickOff chain
            }

            await Task.WhenAll(chainTasks);

            // Evil, I'm sure you have better ideas
            var output = (from line in writer.ToString().Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                group line by line
                into g
                select g).ToDictionary(g => g.Key, g => g.ToList());

            Assert.AreEqual(3, output["Son"].Count);
            Assert.AreEqual(3, output["Wife"].Count);
            Assert.AreEqual(3, output["Husband"].Count);
        }
    }
}