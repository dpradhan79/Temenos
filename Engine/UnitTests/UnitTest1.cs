using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Setup;
using StandardUtilities;
namespace Engine.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        public void TestRelativePath()
        {
            string str = FileUtilities.MakeRelativePath(EngineSetup.TestReportFileName,EngineSetup.GetScreenShotPath());
        }
    }
}
