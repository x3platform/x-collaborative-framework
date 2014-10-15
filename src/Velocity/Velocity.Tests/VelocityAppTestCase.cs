namespace X3Platform.Velocity.Test
{
	using System;
	using System.IO;
	using NUnit.Framework;
	using X3Platform.Velocity.App;
	using X3Platform.Velocity.Runtime;

	/// <summary>
	/// This class is intended to test the App.Velocity class.
	/// </summary>
	/// <author> <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
	/// </author>
	/// <author> <a href="mailto:jon@latchkey.com">Jon S. Stevens</a>
	/// </author>
	[TestFixture]
	public class VelocityAppTestCase : BaseTestCase
	{

		private String input1 = "My name is $name -> $Floog";
		private String result1 = "My name is jason -> floogie woogie";

		public VelocityAppTestCase()
		{
			try
			{
				Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateTest.FILE_RESOURCE_LOADER_PATH);
				Velocity.Init();
			}
			catch (Exception e)
			{
				throw new Exception("Cannot setup VelocityAppTestCase!", e);
			}
		}

		/// <summary>
		/// Runs the test.
		/// </summary>
		[Test]
		public virtual void Test_Run()
		{
			StringWriter compare1 = new StringWriter();
			VelocityContext context = new VelocityContext();
			context.Put("name", "jason");
			context.Put("Floog", "floogie woogie");

			Velocity.Evaluate(context, compare1, "evaltest", input1);

			/*
	    FIXME: Not tested right now.

	    StringWriter result2 = new StringWriter();
	    Velocity.mergeTemplate("mergethis.vm",  context, result2);

	    StringWriter result3 = new StringWriter();
	    Velocity.invokeVelocimacro("floog", "test", new String[2], 
	    context, result3);*/
			Assert.AreEqual(result1, compare1.ToString());
			//if (!result1.Equals(compare1.ToString()))
			//{
				//Assert.Fail("Output incorrect.");
			//}
		}

	}
}