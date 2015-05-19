using System.Collections.Generic;
using System.Linq;
using Lavn.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject.Linq
{
	[TestClass]
	public class Linqx
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


		private class HierarchicalClass
		{
			public int Sid { get; set; }
			public List<HierarchicalClass> Child { get; set; }
		}

		[TestMethod, TestCategory("Linqx")]
		public void Descendants_Flat1()
		{
			var root = new HierarchicalClass();
			root.Child = new List<HierarchicalClass>();
			root.Child.Add(new HierarchicalClass() { Sid = 0 });
			root.Child.Add(new HierarchicalClass() { Sid = 1 });
			root.Child.Add(new HierarchicalClass() { Sid = 2 });

			var result = root.Child.Descendants(e => e.Child).ToList();
			Assert.IsTrue(result.Select(e => e.Sid).SequenceEqual(new[] { 0, 1, 2 }));
		}

		[TestMethod, TestCategory("Linqx")]
		public void Descendants_Flat2()
		{
			var root = new HierarchicalClass();
			root.Child = new List<HierarchicalClass>();
			root.Child.Add(new HierarchicalClass() { Sid = 0 });
			root.Child.Add(new HierarchicalClass() { Sid = 1, Child = new List<HierarchicalClass>() });
			root.Child.Add(new HierarchicalClass() { Sid = 2 });

			var result = root.Child.Descendants(e => e.Child).ToList();
			Assert.IsTrue(result.Select(e => e.Sid).SequenceEqual(new[] { 0, 1, 2 }));
		}

		[TestMethod, TestCategory("Linqx")]
		public void Descendants_Hierarchical1()
		{
			var root = new HierarchicalClass();
			root.Child = new List<HierarchicalClass>();
			root.Child.Add(new HierarchicalClass() { Sid = 0 });
			root.Child.Add(new HierarchicalClass()
			{
				Sid = 1,
				Child = new List<HierarchicalClass>()
				{
					new HierarchicalClass() { Sid = 2 },
					new HierarchicalClass() { Sid = 3 },
					new HierarchicalClass() { Sid = 4 },
				}
			});
			root.Child.Add(new HierarchicalClass() { Sid = 5 });

			var result = root.Child.Descendants(e => e.Child).ToList();
			Assert.IsTrue(result.Select(e => e.Sid).SequenceEqual(new[] { 0, 1, 2, 3, 4, 5 }));
		}

		[TestMethod, TestCategory("Linqx")]
		public void Descendants_Hierarchical2()
		{
			var root = new HierarchicalClass();
			root.Child = new List<HierarchicalClass>();
			root.Child.Add(new HierarchicalClass() { Sid = 0 });
			root.Child.Add(new HierarchicalClass()
			{
				Sid = 1,
				Child = new List<HierarchicalClass>()
				{
					new HierarchicalClass() { Sid = 2 },
					new HierarchicalClass()
					{
						Sid = 3,
						Child = new List<HierarchicalClass>()
						{
							new HierarchicalClass() { Sid = 4 },
						},
					},
					new HierarchicalClass() { Sid = 5 },
				}
			});
			root.Child.Add(new HierarchicalClass() { Sid = 6 });

			var result = root.Child.Descendants(e => e.Child).ToList();
			Assert.IsTrue(result.Select(e => e.Sid).SequenceEqual(new[] { 0, 1, 2, 3, 4, 5, 6 }));
		}
	}
}
