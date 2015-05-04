using Lavn.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject.Linq
{
	[TestClass]
	public class Linqx_Count
	{
		[TestMethod, TestCategory("Linqx")]
		public void MixedResult_True()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.MixedResult(e => e.StartsWith("Start"));
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void MixedResult_False()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.MixedResult(e => e.EndsWith("Start"));
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void MixedResult_Null()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.MixedResult(e => e.Contains("Test1"));
			Assert.AreEqual(null, result);
		}
	}
}
