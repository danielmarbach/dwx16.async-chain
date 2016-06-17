using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsyncDolls;
using NUnit.Framework;

namespace AsyncChain
{
    [TestFixture]
    public class AsyncChainState
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
         * Exercise 10: Introduce state per chain
         */
        [Test]
        public async Task GetItRollingWithState()
        {
            var messages = new ConcurrentQueue<Message>();
            messages.Enqueue(new Message("1"));
            messages.Enqueue(new Message("2"));
            messages.Enqueue(new Message("3"));

            var countdown = new AsyncCountdownEvent(3);

            var chainFactory = new ChainFactory();
            chainFactory.Register(() => new Son());
            chainFactory.Register(() => new Wife());
            chainFactory.Register(() => new Husband());

            var chainTasks = new List<Task>();

            foreach (var message in messages)
            {
                chainTasks.Add(Connector(chainFactory, message));
            }

            await Task.WhenAll(chainTasks);

            // Evil, I'm sure you have better ideas
            var output = (from line in writer.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                          group line by line
                into g
                          select g).ToDictionary(g => g.Key, g => g.ToList());

            Assert.AreEqual(3, output["Son"].Count);
            Assert.AreEqual(3, output["Wife"].Count);
            Assert.AreEqual(3, output["Husband"].Count);
        }

        static Task Connector(ChainFactory factory, Message message)
        {
            // TODO: create the chain
            // TODO: create the context
            // TODO: invoke the chain
            return Task.CompletedTask; // This is just here to disturb you ;)
        }

        // TODO: Extend this interface to support state
        public interface ILinkElement
        {
            Task Invoke(Func<Task> next);
        }

        public class Chain
        {
            readonly List<ILinkElement> executingElements;

            public Chain(IEnumerable<ILinkElement> elements)
            {
                executingElements = new List<ILinkElement>(elements);
            }

            // TODO: Pass on state
            public Task Invoke()
            {
                return InnerInvoke();
            }

            // TODO: Pass on state
            Task InnerInvoke(int currentIndex = 0)
            {
                if (currentIndex == executingElements.Count)
                {
                    return Task.CompletedTask;
                }

                ILinkElement step = executingElements[currentIndex];

                // TODO: Pass the state to all the link elements recursively
                return Task.CompletedTask; // This is just here to disturb you ;)
            }
        }

        class Son : ILinkElement
        {
            public Task Invoke(Func<Task> next)
            {
                Console.WriteLine("Son");
                return next();
            }
        }

        class Wife : ILinkElement
        {
            public Task Invoke(Func<Task> next)
            {
                Console.WriteLine("Wife");
                return next();
            }
        }

        class Husband : ILinkElement
        {
            public Task Invoke(Func<Task> next)
            {
                Console.WriteLine("Husband");
                return next();
            }
        }

        public class ChainFactory
        {
            private readonly Queue<Func<ILinkElement>> registeredLinkElementFactories = new Queue<Func<ILinkElement>>();

            public ChainFactory Register(Func<ILinkElement> elementFactory)
            {
                registeredLinkElementFactories.Enqueue(elementFactory);

                return this;
            }

            public Chain Create()
            {
                var elements = registeredLinkElementFactories.Select(factory => factory()).ToList();

                return new Chain(elements);
            }
        }

        public class IncomingContext
        {
            public IncomingContext(Message message)
            {
                Message = message;
            }

            public Message Message { get; }
        }

        public class Message
        {
            public Message(string id)
            {
                Id = id;
            }

            public string Id { get; }
        }
    }
}