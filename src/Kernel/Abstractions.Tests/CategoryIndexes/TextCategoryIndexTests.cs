namespace X3Platform.Tests.CategoryIndexes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.CategoryIndexes;

    /// <summary></summary>
    [TestClass]
    public class TextCategoryIndexTestSuite
    {
        [TestMethod]
        public void TestWrite()
        {
            CategoryIndexWriter writer = new CategoryIndexWriter("选择类别");

            writer.Read("一级类别01\\二级类别01\\三级类别01");
            writer.Read("一级类别01\\二级类别02\\三级类别02");
            writer.Read("一级类别01\\二级类别03\\三级类别03");
            writer.Read("一级类别02\\二级类别01\\三级类别01");
            writer.Read("一级类别02\\二级类别02\\三级类别02");
            writer.Read("一级类别02\\二级类别03\\三级类别03");
            //writer.Read("11\\113\\123");
            //writer.Read("11\\11\\11");
            //writer.Read("11\\1223\\122");
            //writer.Read("122\\12234");
            writer.Read("22\\12\\2345");

            ICategoryIndex index = writer.Write();

            Assert.IsNotNull(index);
        }

        [TestMethod]
        public void TestInit()
        {
            ICategoryIndex index = new TextCategoryIndex("一级类别02\\二级类别03\\三级类别03");

            Assert.IsNotNull(index);
        }
    }
}
