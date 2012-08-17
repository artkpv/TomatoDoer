using System.Windows.Forms;

namespace TomatoDoer
{
	public interface IFormsFactory
	{
		Form ConstructAboutDialogForm();
	}

	public class FormsFactory : IFormsFactory
	{
		public Form ConstructAboutDialogForm()
		{
			return new About();
		}
	}
}