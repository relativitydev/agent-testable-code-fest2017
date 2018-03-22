using System;
using System.Linq;
using kCura.Relativity.Client;
using NUnit.Framework;
using Relativity.API;
using Relativity.Test.Helpers;
using Relativity.Test.Helpers.ServiceFactory.Extentions;
using Relativity.Test.Helpers.SharedTestHelpers;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace AgentNunitIntegrationTest
{
	[TestFixture]
	public class AgentIntegrationTest
	{
		#region Variables

		public IRSAPIClient Client;
		public int WorkspaceId;
		public IDBContext WorkspaceDbConext;
		public Relativity.API.IServicesMgr ServicesManager;
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
			//WorkspaceId = 1018783;
			//Create Workspace

			//Create instance of Test Helper & set up services manager and db context
			var helper = new TestHelper();
			ServicesManager = helper.GetServicesManager();
			EddsDbContext = helper.GetDBContext(-1);
			

			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			//Create client
			Client = helper.GetServicesManager().GetProxy<IRSAPIClient>(Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.ADMIN_USERNAME, Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.DEFAULT_PASSWORD);
			_workspaceId = CreateWorkspace();
			WorkspaceDbConext = helper.GetDBContext(WorkspaceId);

			//Create Field
			CreateField_LongText(Client, _workspaceId, NewFieldName);
		}

		#endregion

		#region TestfixtureTeardown

		[TestFixtureTearDown]
		public void Execute_TestFixtureTeardown()
		{
			Relativity.Test.Helpers.WorkspaceHelpers.DeleteWorkspace.Delete(Client, _workspaceId);
		}

		#endregion

		#region Golden Flow

		[Test, Description("Golden Flow Unit Test")]
		public void Integration_Test_Golden_Flow_Valid()
		{
			//Arrange and Act
			// Get the Field Artifact ID of "Demo Document Field"
			FieldArtifactId = GetFieldArtifactID(NewFieldName, WorkspaceDbConext, Client);

			//Get the field Count of the field.
			if (FieldArtifactId == 0)
			{
				throw new Exception("Field failed to create");
			}

			//Assert
			Assert.Greater(FieldArtifactId, 1);
		}

		#endregion


		private Int32 CreateWorkspace()
		{
			try
			{
				Client.APIOptions.WorkspaceID = -1;
				// retry logic for workspace creation
				var j = 1;

				while (j < 5)
				{
					j++;
					try
					{
						Console.WriteLine("Creating workspace.....");

						//Not Using Test Helpers
						//int? templateArtifactID = null;
						//kCura.Relativity.Client.DTOs.Query<kCura.Relativity.Client.DTOs.Workspace> query = new kCura.Relativity.Client.DTOs.Query<kCura.Relativity.Client.DTOs.Workspace>();
						//query.Condition = new kCura.Relativity.Client.TextCondition(kCura.Relativity.Client.DTOs.FieldFieldNames.Name, kCura.Relativity.Client.TextConditionEnum.EqualTo, ConfigurationHelper.TEST_WORKSPACE_TEMPLATE_NAME);
						//query.Fields = kCura.Relativity.Client.DTOs.FieldValue.AllFields;
						//kCura.Relativity.Client.DTOs.QueryResultSet<kCura.Relativity.Client.DTOs.Workspace> resultSet = Client.Repositories.Workspace.Query(query, 0);

						//templateArtifactID = resultSet.Results.FirstOrDefault().Artifact.ArtifactID;
						//var workspaceDTO = new kCura.Relativity.Client.DTOs.Workspace();
						//workspaceDTO.Name = _workspaceName;

						//ProcessOperationResult result = new kCura.Relativity.Client.ProcessOperationResult();
						//Create a ProcessInformation to return the status of the Workspace creation process.
						//kCura.Relativity.Client.ProcessInformation processInfo;
						//try
						//{
						//	Call CreateAsync passing the templateID and workspaceDTO.
						//	This returns a ProcessOperationResult with a Success property and a ProcessID property.
						//	NOTE: The Success property indicates the success of starting the create process, 
						//	not the success of the actual workspace creation.
						//	result = Client.Repositories.Workspace.CreateAsync(templateArtifactID.Value, workspaceDTO);

						//}
						//catch (Exception ex)
						//{
						//	Console.WriteLine(ex.Message, "Unhandled Exception");

						//}

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


		public static Int32 GetFieldArtifactID(String fieldname, IDBContext workspaceDbContext, IRSAPIClient client)
		{
			int fieldArtifactID = 0;
			kCura.Relativity.Client.DTOs.Query<kCura.Relativity.Client.DTOs.Field> query = new kCura.Relativity.Client.DTOs.Query<kCura.Relativity.Client.DTOs.Field>();
			query.Condition = new kCura.Relativity.Client.TextCondition(kCura.Relativity.Client.DTOs.FieldFieldNames.Name, kCura.Relativity.Client.TextConditionEnum.EqualTo, fieldname);
			query.Fields = kCura.Relativity.Client.DTOs.FieldValue.AllFields;
			kCura.Relativity.Client.DTOs.QueryResultSet<kCura.Relativity.Client.DTOs.Field> resultSet = client.Repositories.Field.Query(query, 0);

			fieldArtifactID = resultSet.Results[0].Artifact.ArtifactID;
			return fieldArtifactID;
		}

		//public static class RelativityInformation
		//{
		//	/ <summary>
		//	/ Gets the version of the current Relativity instance
		//	/ </summary>
		//	/ <returns>The Relativity version in the format "x.x.x.x"</returns>
		//	public static async Task<string> GetRelativityVersionAsync()
		//	{
		//		string relativityVersion;
		//		using (IInstanceDetailsManager proxy = helper.GetServicesManager().GetDefaultProxy<IInstanceDetailsManager>())
		//		{
		//			relativityVersion = await proxy.GetRelativityVersionAsync();
		//		}

		//		return relativityVersion;
		//	}
		//}

		public static Int32 CreateField_LongText(IRSAPIClient client, int workspaceID, string fieldName)
		{

			Int32 fieldID = 0;

			//Set the workspace ID
			client.APIOptions.WorkspaceID = workspaceID;

			//Create a Field DTO
			kCura.Relativity.Client.DTOs.Field fieldDTO = new kCura.Relativity.Client.DTOs.Field();

			//Set primary fields
			//The name of the sample data is being set to a random string so that sample data can be debugged
			//and never causes collisions. You can set this to any string that you want
			fieldDTO.Name = string.Format(fieldName);
			fieldDTO.ObjectType = new kCura.Relativity.Client.DTOs.ObjectType()
			{
				DescriptorArtifactTypeID = (int)ArtifactType.Document
			};
			fieldDTO.FieldTypeID = kCura.Relativity.Client.FieldType.LongText;

			// Set secondary fields
			fieldDTO.AllowHTML = false;
			fieldDTO.AllowGroupBy = false;
			fieldDTO.AllowPivot = false;
			fieldDTO.AllowSortTally = false;
			fieldDTO.AvailableInViewer = false;
			fieldDTO.IncludeInTextIndex = true;
			fieldDTO.IsRequired = false;
			fieldDTO.OpenToAssociations = false;
			fieldDTO.Linked = false;
			fieldDTO.Unicode = true;
			fieldDTO.Width = "";
			fieldDTO.Wrapping = true;

			//Create the field
			kCura.Relativity.Client.DTOs.WriteResultSet<kCura.Relativity.Client.DTOs.Field> resultSet = client.Repositories.Field.Create(fieldDTO);

			//Check for success
			if (resultSet.Success)
			{
				fieldID = resultSet.Results.FirstOrDefault().Artifact.ArtifactID;
				return fieldID;
			}
			else
			{
				Console.WriteLine("Field was not created");
				return fieldID;
			}
		}
	}
}

