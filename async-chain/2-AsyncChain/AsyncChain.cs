using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncChain
{
    [TestFixture]
    public class AsyncChain
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
         * Exercise 4: Implement a manual version of the chain of responsibility by chaining method calls
         */
        [Test]
        public async Task ComposeManual()
        {
            Func<Task> done = () =>
            {
                Console.WriteLine("done");
                return Task.CompletedTask;
            };

            // TODO: Compose here the chain manually


            Assert.That(writer.ToString(), Is.EqualTo(@"Son
Wife
Husband
done
"));
        }

        // TODO: Extend and implement this method
        public static async Task Son()
        {
            Console.WriteLine("Son");
        }

        // TODO: Extend and implement this method
        public static async Task Wife()
        {
            Console.WriteLine("Wife");
        }

        // TODO: Extend and implement this method
        public static async Task Husband()
        {
            Console.WriteLine("Husband");
        }

        /*
         * Exercise 5: Implement a more generic version of the chain of responsibility pattern
         * Hints/Suggestions: Lists, Recursion, While loop?
         */
        [Test]
        public async Task ComposeGeneric()
        {
            Func<Task> done = () =>
            {
                Console.WriteLine("done");
                return Task.CompletedTask;
            };

            // TODO: Compose here the chain in a more generic way, reuse the methods Son(), Wife(), Husband()

            await Invoke();

            Assert.That(writer.ToString(), Is.EqualTo(@"Son
Wife
Husband
done
"));
        }

        // TODO: Extend and implement this method
        public static async Task Invoke()
        {
        }

        /*
         * Exercise 6: Introduce an async exception filter which catches InvalidOperationExceptions
         */
        [Test]
        public async Task ComposeGenericWithFilters()
        {
            Func<Task> done = () =>
            {
                Console.WriteLine("done");
                return Task.CompletedTask;
            };

            // TODO: Compose here the chain in a more generic way, reuse the methods Son(), Wife(), Husband() and Invoke()
            // - put EvilMethod() right before done
            // - Add filter on the top of the chain


            Assert.That(writer.ToString(), Is.EqualTo(@"FilterInvalidOperationException
Son
Wife
Husband
EvilMethod
Filtered!
"));
        }

        // TODO: Extend and implement this method
        static async Task FilterInvalidOperationException(Func<Task> next)
        {
            Console.WriteLine("FilterInvalidOperationException");

            // TODO: Move this line where appropriate
            Console.WriteLine("Filtered!");
        }

        static async Task EvilMethod(Func<Task> next)
        {
            Console.WriteLine("EvilMethod");
            await Task.Yield();
            throw new InvalidOperationException("Boomer!");
        }

        /*
         * Exercise 7.1: Write a retrier which retries 3 times in case of an exception,
         * if after 3 retries the chain still throws an exception then it rethrows the last exception it saw
         */
        [Test]
        public void RetriesAndRethrows()
        {
            Func<Task> done = () =>
            {
                Console.WriteLine("done");
                return Task.CompletedTask;
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                // TODO: Compose here the chain in a more generic way, reuse the methods Son(), Wife(), Husband() and Invoke()
                // - put ThrowsAlways() right before done
                // - Add RetryThreeTimesAndRethrowExceptionIfStillOccurs on the top of the chain
            });

            Assert.That(writer.ToString(), Is.EqualTo(@"RetryThreeTimesAndRethrowExceptionIfStillOccurs
Son
Wife
Husband
ThrowsAlways
Son
Wife
Husband
ThrowsAlways
Son
Wife
Husband
ThrowsAlways
"), writer.ToString());
        }

        /*
         * Exercise 7.2: Write a retrier which retries 3 times in case of an exception,
         * if after less than 3 retries the chain does not throw any more then the chain is successful
         */
        [Test]
        public void RetriesAndDoesNotRethrow()
        {
            Func<Task> done = () =>
            {
                Console.WriteLine("done");
                return Task.CompletedTask;
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                // TODO: Compose here the chain in a more generic way, reuse the methods Son(), Wife(), Husband() and Invoke()
                // - put ThrowsTwoTimes() right before done
                // - Add RetryThreeTimesAndRethrowExceptionIfStillOccurs on the top of the chain
            });

            Assert.That(writer.ToString(), Is.EqualTo(@"RetryThreeTimesAndRethrowExceptionIfStillOccurs
Son
Wife
Husband
ThrowsTwoTimes
Son
Wife
Husband
ThrowsTwoTimes
"), writer.ToString());
        }

        // TODO: Implement this method
        public static async Task RetryThreeTimesAndRethrowExceptionIfStillOccurs(Func<Task> next)
        {
            Console.WriteLine("RetryThreeTimesAndRethrowExceptionIfStillOccurs");

        }

        public static async Task ThrowsAlways(Func<Task> next)
        {
            Console.WriteLine("ThrowsAlways");
            await Task.Yield();
            throw new InvalidOperationException();
        }

        static int invocationCounter;

        public static async Task ThrowsTwoTimes(Func<Task> next)
        {
            Console.WriteLine("ThrowsTwoTimes");
            await Task.Yield();

            invocationCounter++;
            if (invocationCounter < 2)
            {
                throw new InvalidOperationException();
            }
        }
    }
}