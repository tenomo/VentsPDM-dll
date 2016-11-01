 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VentsPDM_dll;
using System.Collections.Generic;

namespace PDMUnitTests
{
    [TestClass]
    public class PDMUnitTests
    {
        PDM pdm;
        string segmentName;
        public PDMUnitTests()
        {
            pdm = new PDM();
            segmentName = "3535";
        }

        [TestMethod]
        public void SearchDoc_Result_NotNull()
        {
            IEnumerable<DataModel> method_result = pdm.SearchDoc(segmentName);
         Assert.IsNotNull( method_result);
        }

        [TestMethod]
        public void SearchDoc_Result_items_containe_segment_name()
        {
            IEnumerable<DataModel> method_result = pdm.SearchDoc(segmentName);
            foreach (var item in method_result)
            {
                Assert.IsTrue(item.FileName.Contains(segmentName));
            }            
        }
    }
}
