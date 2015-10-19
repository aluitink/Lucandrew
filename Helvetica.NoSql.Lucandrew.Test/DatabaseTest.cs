using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helvetica.NoSql.Lucandrew.Test
{
    [TestClass]
    public class DatabaseTest
    {
        private Database _database;

        private class TestClass1
        {
            public String Name { get; set; }
            public DateTime Date { get; set; }
            public Int16 IntSixteen { get; set; }
            public Int32 IntThirtyTwo { get; set; }
            public Int64 IntSixtyFour { get; set; }
            public bool Boolean { get; set; }

            public TestClass2 Class2 { get; set; }
            public override string ToString()
            {
                return
                    String.Format(
                        "Name = {0}, Date = {1}, IntSixteen = {2}, IntThirtyTwo = {3}, IntSixtyFour = {4}, Boolean = {5}, Class2 = [{6}]",
                        Name, Date, IntSixteen, IntThirtyTwo, IntSixtyFour, Boolean, Class2 == null ? "Null" : Class2.ToString());
            }
        }

        private class TestClass2
        {
            public String Name { get; set; }
            public DateTime Date { get; set; }
            public Int16 IntSixteen { get; set; }
            public Int32 IntThirtyTwo { get; set; }
            public Int64 IntSixtyFour { get; set; }
            public bool Boolean { get; set; }
            public TestClass3 Class3 { get; set; }

            public override string ToString()
            {
                return
                    String.Format(
                        "Name = {0}, Date = {1}, IntSixteen = {2}, IntThirtyTwo = {3}, IntSixtyFour = {4}, Boolean = {5}, Class3 = [{6}]",
                        Name, Date, IntSixteen, IntThirtyTwo, IntSixtyFour, Boolean, Class3 == null ? "Null" : Class3.ToString());
            }
        }

        private class TestClass3
        {
            public String Name { get; set; }
            public DateTime Date { get; set; }
            public Int16 IntSixteen { get; set; }
            public Int32 IntThirtyTwo { get; set; }
            public Int64 IntSixtyFour { get; set; }
            public bool Boolean { get; set; }
            public TestClass1 Class1 { get; set; }

            public override string ToString()
            {
                return
                    String.Format(
                        "Name = {0}, Date = {1}, IntSixteen = {2}, IntThirtyTwo = {3}, IntSixtyFour = {4}, Boolean = {5}, Class1 = [{6}]",
                        Name, Date, IntSixteen, IntThirtyTwo, IntSixtyFour, Boolean, Class1 == null ? "Null" : Class1.ToString());
            }
        }

        [TestInitialize()]
        public void Initialize()
        {
            _database = new Database(DateTime.Now.Ticks.ToString());
        }

        [TestCleanup()]
        public void Cleanup()
        {
            _database.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DatabaseCanHandleNullObject()
        {
            TestClass1 c1 = null;

            ObjectReference<TestClass1> reference = _database.Store(c1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DatabaseCanHandleNullSearch()
        {
            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;

            ObjectReference<TestClass1> reference = _database.Store(c1);

            Assert.IsNotNull(reference.Object);
            Assert.IsNotNull(reference.Id);

            Assert.AreEqual(c1.ToString(), reference.Object.ToString());

            var ret = _database.Search<TestClass1>(null);

            //need to evaluate the enumerable to execute because search is a yeild return.
            int count = ret.Count();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DatabaseCanHandleNullUpdate()
        {
            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName123";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;

            _database.Store(c1);

            ObjectReference<TestClass1> result = _database.Search<TestClass1>(new { Name = "SuperName123" }).First();

            Assert.IsNotNull(result.Object);
            Assert.IsNotNull(result.Id);

            Assert.AreEqual(c1.ToString(), result.Object.ToString());


            result = null;

            _database.Update(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DatabaseCanHandleNullDelete()
        {
            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName123";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;

            _database.Store(c1);

            ObjectReference<TestClass1> result = _database.Search<TestClass1>(new { Name = "SuperName123" }).First();

            Assert.IsNotNull(result.Object);
            Assert.IsNotNull(result.Id);

            Assert.AreEqual(c1.ToString(), result.Object.ToString());


            result = null;

            _database.Delete(result);
        }


        [TestMethod]
        public void DatabaseCanStoreObject()
        {
            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;

            ObjectReference<TestClass1> reference = _database.Store(c1);

            Assert.IsNotNull(reference.Object);
            Assert.IsNotNull(reference.Id);

            Assert.AreEqual(c1.ToString(), reference.Object.ToString());
        }

        [TestMethod]
        public void DatabaseCanStoreComplexObject()
        {
            TestClass3 c3 = new TestClass3();
            c3.Boolean = true;
            c3.Date = DateTime.Now;
            c3.Name = "SuperName";
            c3.IntSixteen = 16;
            c3.IntThirtyTwo = 32;
            c3.IntSixtyFour = 64;
            c3.Class1 = new TestClass1();
            c3.Class1.Boolean = true;
            c3.Class1.Date = DateTime.Now;
            c3.Class1.Name = "SuperName";
            c3.Class1.IntSixteen = 16;
            c3.Class1.IntThirtyTwo = 32;
            c3.Class1.IntSixtyFour = 64;


            TestClass2 c2 = new TestClass2();
            c2.Boolean = true;
            c2.Date = DateTime.Now;
            c2.Name = "SuperName";
            c2.IntSixteen = 16;
            c2.IntThirtyTwo = 32;
            c2.IntSixtyFour = 64;
            c2.Class3 = c3;

            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;
            c1.Class2 = c2;


            ObjectReference<TestClass1> reference = _database.Store(c1);

            Assert.IsNotNull(reference.Object);
            Assert.IsNotNull(reference.Id);

            Assert.AreEqual(c1.ToString(), reference.Object.ToString());
        }

        [TestMethod]
        public void DatabaseCanSearchObject()
        {
            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName123";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;

            _database.Store(c1);

            ObjectReference<TestClass1> result = _database.Search<TestClass1>(new {Name = "SuperName123"}).First();

            Assert.IsNotNull(result.Object);
            Assert.IsNotNull(result.Id);

            Assert.AreEqual(c1.ToString(), result.Object.ToString());
        }

        [TestMethod]
        public void DatabaseCanUpdateObject()
        {
            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName123";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;

            _database.Store(c1);

            ObjectReference<TestClass1> result = _database.Search<TestClass1>(new { Name = "SuperName123" }).First();

            Assert.IsNotNull(result.Object);
            Assert.IsNotNull(result.Id);

            Assert.AreEqual(c1.ToString(), result.Object.ToString());

            result.Object.Name = "NotSoSuperName123";
            result.Update();

            ObjectReference<TestClass1> result2 = _database.Search<TestClass1>(new { Name = "NotSoSuperName123" }).First();

            Assert.IsNotNull(result2.Object);
            Assert.IsNotNull(result2.Id);

            Assert.AreEqual(result.Object.ToString(), result2.Object.ToString());

        }

        [TestMethod]
        public void DatabaseCanDeleteObject()
        {
            TestClass1 c1 = new TestClass1();
            c1.Boolean = true;
            c1.Date = DateTime.Now;
            c1.Name = "SuperName321";
            c1.IntSixteen = 16;
            c1.IntThirtyTwo = 32;
            c1.IntSixtyFour = 64;

            _database.Store(c1);

            ObjectReference<TestClass1> result = _database.Search<TestClass1>(new { Name = "SuperName321" }).First();

            Assert.IsNotNull(result.Object);
            Assert.IsNotNull(result.Id);

            Assert.AreEqual(c1.ToString(), result.Object.ToString());

            result.Delete();

            var ret = _database.Search<TestClass1>(new {Name = "SuperName321"});

            Assert.IsTrue(!ret.Any());
        }

    }
}
