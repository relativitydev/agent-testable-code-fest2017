﻿using System;
using System.Linq;
using kCura.Relativity.Client;
using NUnit.Framework;
using Relativity.API;
using Relativity.Test.Helpers;
using Relativity.Test.Helpers.Application;
using Relativity.Test.Helpers.GroupHelpers;
using Relativity.Test.Helpers.ServiceFactory.Extentions;
using Relativity.Test.Helpers.SharedTestHelpers;
using Relativity.Test.Helpers.WorkspaceHelpers;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using NUnit.Framework.Internal;

namespace AgentNunitIntegrationTest
{
	[TestFixture]
	public class AgentIntegrationTest
	{
		#region Variables

		public IRSAPIClient Client;
		public Relativity.API.IServicesMgr ServicesManager;
		public int FieldArtifactId;
		public const string NewFieldName = "Demo Document Field";
		private int _workspaceId;

		#endregion


		#region TestfixtureSetup

		[OneTimeSetUp]
		public void Execute_TestFixtureSetup()
		{
			//Create Workspace
			Dictionary<string, string> configDictionary = new Dictionary<string, string>();
			foreach (string testParameterName in TestContext.Parameters.Names)
			{
				configDictionary.Add(testParameterName, TestContext.Parameters[testParameterName]);
			}
			
			TestHelper helper = new TestHelper(configDictionary);

			ServicesManager = helper.GetServicesManager();

			//Create client
			Client = helper.GetServicesManager().GetProxy<IRSAPIClient>(ConfigurationHelper.ADMIN_USERNAME, ConfigurationHelper.DEFAULT_PASSWORD);
			_workspaceId = 1017533; 
		}

		#endregion

		#region TestfixtureTeardown

		[OneTimeTearDown]
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
			FieldArtifactId = GetFieldArtifactID(NewFieldName, Client);

			//Get the field Count of the field.
			if (FieldArtifactId == 0)
			{
				throw new Exception("Field failed to create");
			}

			//Assert
			Assert.Greater(FieldArtifactId, 1);
		}

		#endregion


		public static Int32 GetFieldArtifactID(String fieldname, IRSAPIClient client)
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

