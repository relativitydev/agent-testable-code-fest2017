using System;
using Relativity.API;
using RelativityAgent1.Helpers;


namespace RelativityAgent1
{

	[kCura.Agent.CustomAttributes.Name("RelativityTestAgent Job")]
	[System.Runtime.InteropServices.Guid("2E29383D-C993-4358-AE6D-8BC462703EE5")]
	public class RelativityTestAgentJob
	{
		public IArtifactQueries ArtifactQueries { get; set; }
		public IAPILog Logger { get; private set; }
		public IServicesMgr SvcManager { get; private set; }
		public ExecutionIdentity CurrentUserIdentity { get; set; }
		public int WorkspaceArtifactId { get; private set; }
		public IAgentHelper AgentHelper { get; private set; }
    private int _fieldArtifactId;

		public RelativityTestAgentJob(IArtifactQueries artifactQueries, IAPILog logger, IServicesMgr svcManager, ExecutionIdentity userIdentity, int workspaceId, IAgentHelper agentHelper)
		{
			ArtifactQueries = artifactQueries;
			Logger = logger;
			WorkspaceArtifactId = workspaceId;
			SvcManager = svcManager;
			CurrentUserIdentity = userIdentity;
			AgentHelper = agentHelper;
		}

		public void Execute()
		{
            int expectedArtifactID;
			   //create field 
			expectedArtifactID = ArtifactQueries.CreateFixedLengthTextField(WorkspaceArtifactId, SvcManager, CurrentUserIdentity);

			if (expectedArtifactID == 0)
				   {
				   throw new Exception("Field failed to create field");
				   }

            //fix
            //In a real scenario, workspace Artifact id would be coming in from a manager queue record.
            //Check to see if field exists
           /* const int newWorkspaceArtifactId = 1018783;  
                                   
            _fieldArtifactId = ArtifactQueries.GetFieldArtifactId("Demo Document Field", AgentHelper.GetDBContext(newWorkspaceArtifactId));
              if (_fieldArtifactId == 0)
                  {
                  Console.WriteLine("Field is already present in the database :)");
                  }
                  
         expectedArtifactID = ArtifactQueries.CreateFixedLengthTextField(newWorkspaceArtifactId, SvcManager, CurrentUserIdentity);*/
    }
  }
}
