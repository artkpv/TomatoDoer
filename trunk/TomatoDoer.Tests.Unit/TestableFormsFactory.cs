using System;
using System.Windows.Forms;
using Rhino.Mocks;

namespace TomatoDoer.Tests.Unit
{
	public class TestableFormsFactory : IFormsFactory
	{
		public Form ConstructAboutDialogForm()
		{
			return MockRepository.GenerateMock<Form>();
		}
	}
}