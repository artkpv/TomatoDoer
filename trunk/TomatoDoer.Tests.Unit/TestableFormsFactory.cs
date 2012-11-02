using System;
using System.Windows.Forms;
using Moq;

namespace TomatoDoer.Tests.Unit
{
	public class TestableFormsFactory : IFormsFactory
	{
		public Form ConstructAboutDialogForm()
		{
			return new Mock<Form>().Object;
		}
	}
}