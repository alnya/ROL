using System;
using System.Collections.Generic;
using System.Text;

namespace nunitTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitTests.SYSTEMUSERTests test = new UnitTests.SYSTEMUSERTests();
            //---------------------
            // List Tests
            //---------------------
            //test.SearchTest();
            //test.DeleteAllTest();

            //---------------------
            // Item Tests
            //---------------------
            test.AddTest();
            //test.EditTest();
            //test.LoadTest();
            //test.DeleteTest();
        }
    }
}
