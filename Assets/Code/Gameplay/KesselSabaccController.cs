namespace KesselSabacc.Gameplay
{
	public class KesselSabaccController
	{
		private Model.KesselSabacc _model;

		public Model.KesselSabacc Model => _model;

		public KesselSabaccController(Model.KesselSabacc model)
		{
			_model = model;
		}
	}
}
