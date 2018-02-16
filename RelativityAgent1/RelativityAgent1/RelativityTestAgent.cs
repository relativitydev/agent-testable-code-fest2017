using System;
using Relativity.API;
using RelativityAgent1.Helpers;

namespace RelativityAgent1
{
    [kCura.Agent.CustomAttributes.Name("RelativityTestAgent")]
	[System.Runtime.InteropServices.Guid("b5ce36a5-dab7-4e80-ab21-3b9a76e0c560")]
	public class RelativityTestAgent : kCura.Agent.AgentBase
	{
		private IAPILog _logger;
		public IServicesMgr SvcManager;
		public ExecutionIdentity IdentityCurrentUser;
		public int WorkspaceId;

    public override void Execute()
		{
			//Set up
			IArtifactQueries artifactQueries = new ArtifactQueries();
			_logger = Helper.GetLoggerFactory().GetLogger();
			SvcManager = Helper.GetServicesManager();
			IdentityCurrentUser = ExecutionIdentity.CurrentUser;

			try
			{
				//Create instance of Relativity Test Job  
				RelativityTestAgentJob job = new RelativityTestAgentJob(artifactQueries,  _logger, SvcManager, IdentityCurrentUser, -1, Helper);
				job.Execute();
			}
			catch (Exception ex)
			{
				//Your Agent caught an exception
				RaiseMessage(ex.Message, 0);
				RaiseError(ex.Message, ex.Message);
			}
		}

		public override string Name
		{
			get
			{
				return "RelativityTestAgent";
			}
		}
	}
}
