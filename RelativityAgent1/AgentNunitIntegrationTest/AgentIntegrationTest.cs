using System;
using kCura.Relativity.Client;
using NUnit.Framework;
using Relativity.API;
using Relativity.Test.Helpers;
using Relativity.Test.Helpers.SharedTestHelpers;
using IServicesMgr = Relativity.Test.Helpers.Interface.IServicesMgr;

namespace AgentNunitIntegrationTest
{
	[TestFixture]
	public class AgentIntegrationTest
	{
		#region Variables

		public IRSAPIClient Client;
		public int WorkspaceId;
		public IDBContext WorkspaceDbConext;
		public IServicesMgr ServicesManager;
		public IDBContext EddsDbContext;
		public int FieldArtifactId;
		public const string NewFieldName = "Demo Document Field";
		private int _workspaceId;
		private string _workspaceName = "Test Workspace CI";

		#endregion


		#region TestfixtureSetup

		[TestFixtureSetUp]
		public void Execute_TestFixtureSetup()
		{
			WorkspaceId = 1018783;
			//Create Workspace

			//Create instance of Test Helper & set up services manager and db context
			var helper = new TestHelper();
			ServicesManager = helper.GetServicesManager();
			EddsDbContext = helper.GetDBContext(-1);
			//_workspaceId = CreateWorkspace();
			WorkspaceDbConext = helper.GetDBContext(WorkspaceId);

			//Create client
			Client = helper.GetServicesManager().GetProxy<IRSAPIClient>(Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.ADMIN_USERNAME, Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.DEFAULT_PASSWORD);
		}

		#endregion

		#region TestfixtureTeardown

		[TestFixtureTearDown]
		public void Execute_TestFixtureTeardown() { }

		#endregion

		#region Golden Flow

		[Test, Description("Golden Flow Unit Test")]
		public void Integration_Test_Golden_Flow_Valid()
		{
			//Arrange and Act
			// Get the Field Artifact ID of "Demo Document Field"
			FieldArtifactId = Relativity.Test.Helpers.ArtifactHelpers.Fields.GetFieldArtifactID(NewFieldName, WorkspaceDbConext);

			//Get the field Count of the field.
			var fieldCount = Relativity.Test.Helpers.ArtifactHelpers.Fields.GetFieldCount(WorkspaceDbConext, FieldArtifactId);

			//Assert
			Assert.AreEqual(fieldCount, 1);
		}

		#endregion


		private Int32 CreateWorkspace()
		{
			try
			{
				// retry logic for workspace creation
				var j = 1;

				while (j < 5)
				{
					j++;
					try
					{
						Console.WriteLine("Creating workspace.....");
						_workspaceId =
							Relativity.Test.Helpers.WorkspaceHelpers.CreateWorkspace.CreateWorkspaceAsync(_workspaceName,
								ConfigurationHelper.TEST_WORKSPACE_TEMPLATE_NAME, ServicesManager, ConfigurationHelper.ADMIN_USERNAME,
								ConfigurationHelper.DEFAULT_PASSWORD).Result;
						Console.WriteLine($"Workspace created [WorkspaceArtifactId= {_workspaceId}].....");
						j = 5;
					}
					catch (Exception e)
					{
						Console.WriteLine("Failed to create workspace, Retry now...");

						if (j != 5)
							continue;
						Client = null;
						throw new Exception(
							$"Failed to create workspace in the setup. Reset the Client to null\nError Message:\n{e.Message}.\nInner Exception Message:\n{e.InnerException.Message}.\nStrack trace:\n{e.StackTrace}.");
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error encountered while creating a new Workspace.", ex);
			}
			finally
			{
				Console.WriteLine($"Workspace created [WorkspaceArtifactId= {_workspaceId}].....");
			}
			return _workspaceId;
		}
	}
}

