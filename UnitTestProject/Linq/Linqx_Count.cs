using Lavn.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject.Linq
{
	[TestClass]
	public class Linqx_Count
	{
		[TestMethod, TestCategory("Linqx")]
		public void CountOver_False()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.CountOver(3);
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountOver_Predicate_True()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountOver(2, e => e.Contains("Test"));
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountOver_Predicate_False()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountOver(3, e => e.Contains("Test"));
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_True1()
		{
			var result = new string[] {
			}.CountUnder(1);
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_True2()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.CountUnder(4);
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_False1()
		{
			var result = new string[] {
			}.CountUnder(0);
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_False2()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.CountUnder(3);
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_Predicate_True1()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountUnder(1, e => e.Contains("AAA"));
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_Predicate_True2()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountUnder(4, e => e.Contains("Test"));
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_Predicate_False1()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountUnder(0, e => e.Contains("AAA"));
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountUnder_Predicate_False2()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountUnder(3, e => e.Contains("Test"));
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountEquals_True()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.CountEquals(3);
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountEquals_False1()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.CountEquals(2);
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountEquals_False2()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
			}.CountEquals(4);
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountEquals_Predicate_True()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountEquals(3, e => e.Contains("Test"));
			Assert.AreEqual(true, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountEquals_Predicate_False1()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountEquals(2, e => e.Contains("Test"));
			Assert.AreEqual(false, result);
		}

		[TestMethod, TestCategory("Linqx")]
		public void CountEquals_Predicate_False2()
		{
			var result = new[] {
				"Start_Test1_End",
				"Start_Test2_End",
				"Start_Test3_End",
				"Start_XXXX4_End",
			}.CountEquals(4, e => e.Contains("Test"));
			Assert.AreEqual(false, result);
		}
	}
}
