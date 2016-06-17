using System;
using System.IO;
using System.Reflection;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace AsyncChain
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class SyncChain
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
         * Exercise 1: Implement a manual version of the chain of responsibility by chaining method calls
         */
        [Test]
        public void ComposeManual()
        {
            Action done = () =>
            {
                Console.WriteLine("done");
            };

            // TODO: Compose here the chain manually


            Approvals.Verify(writer);
        }

        // TODO: Extend and implement this method
        public static void Son()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        }

        // TODO: Extend and implement this method
        public static void Wife()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        }

        // TODO: Extend and implement this method
        public static void Husband()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        }

        /*
         * Exercise 2: Implement a more generic version of the chain of responsibility pattern
         * Hints/Suggestions: Lists, Recursion, While loop?
         */
        [Test]
        public void ComposeGeneric()
        {
            Action done = () =>
            {
                Console.WriteLine("done");
            };

            // TODO: Compose here the chain in a more generic way, reuse the methods Son(), Wife(), Husband()

            Invoke();

            Approvals.Verify(writer);
        }

        // TODO: Extend and implement this method
        public static void Invoke()
        {

        }

        /*
         * Exercise 3: Introduce an exception filter which catches InvalidOperationExceptions
         */
        [Test]
        public void ComposeGenericWithFilters()
        {
            Action done = () =>
            {
                Console.WriteLine("done");
            };

            // TODO: Compose here the chain in a more generic way, reuse the methods Son(), Wife(), Husband() and Invoke()
            // - put EvilMethod() right before done
            // - Add filter on the top of the chain


            Approvals.Verify(writer);
        }

        // TODO: Extend and implement this method,
        static void FilterInvalidOperationException(Action next)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            // TODO: Move this line where appropriate
            Console.WriteLine("Filtered!");
        }

        static void EvilMethod(Action next)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            throw new InvalidOperationException("Boomer!");
        }

    }
}