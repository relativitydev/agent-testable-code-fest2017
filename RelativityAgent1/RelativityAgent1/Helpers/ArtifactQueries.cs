using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using kCura.Relativity.Client;
using Relativity.API;

namespace RelativityAgent1.Helpers
{
	public class ArtifactQueries : IArtifactQueries
	{
		public int CreateFixedLengthTextField(int workspaceId, IServicesMgr svcMgr, ExecutionIdentity identity)
		{
			var fieldId = 0;

			try
			{
				using (IRSAPIClient client = svcMgr.CreateProxy<IRSAPIClient>(identity))
				{
					//Set the workspace ID
					client.APIOptions.WorkspaceID = workspaceId;

					//Create a Field DTO
					kCura.Relativity.Client.DTOs.Field fieldDto = new kCura.Relativity.Client.DTOs.Field();

					//Set primary fields
					//The name of the sample data is being set to a random string so that sample data can be debugged
					//and never causes collisions. You can set this to any string that you want
					fieldDto.Name = "Demo Document Field";
					fieldDto.ObjectType = new kCura.Relativity.Client.DTOs.ObjectType(){DescriptorArtifactTypeID = (int)ArtifactType.Document};
					fieldDto.FieldTypeID = FieldType.FixedLengthText;

					//Set secondary fields
          fieldDto.AllowHTML = false;
					fieldDto.AllowGroupBy = false;
					fieldDto.AllowPivot = false;
					fieldDto.AllowSortTally = false;
					fieldDto.IncludeInTextIndex = true;
					fieldDto.IsRequired = false;
					fieldDto.OpenToAssociations = false;
					fieldDto.Length = 255;
					fieldDto.Linked = false;
					fieldDto.Unicode = true;
					fieldDto.Width = "";
					fieldDto.Wrapping = true;
					fieldDto.IsRelational = false;

					//Create the field
					kCura.Relativity.Client.DTOs.WriteResultSet<kCura.Relativity.Client.DTOs.Field> resultSet = client.Repositories.Field.Create(fieldDto);

					//Check for success
				  if (!resultSet.Success)
				  {
				    Console.WriteLine("Field was not created");
				    return fieldId;
				  }

				  var firstOrDefault = resultSet.Results.FirstOrDefault();
				  if (firstOrDefault != null) fieldId = firstOrDefault.Artifact.ArtifactID;

				  return fieldId;
				}
			}
			catch (Exception)
			{
				throw new Exception("Failed in the create field method.");
			}
		}
		public int GetFieldArtifactId(string fieldName, IDBContext workspaceDbContext)
		{
			const string sqlQuery = @"SELECT [ArtifactID] FROM [EDDSDBO].[Field] WHERE [DisplayName] LIKE '%@fieldName%'";
      var sqlParams = new List<SqlParameter>{new SqlParameter("@fieldName", SqlDbType.NVarChar) {Value = fieldName}};

      return workspaceDbContext.ExecuteSqlStatementAsScalar<int>(sqlQuery, sqlParams);
		}

		public int GetFieldCount(IDBContext workspaceDbContext, int fieldArtifactId)
		{
			const string sqlQuery = @"select count(*) from [EDDSDBO].[ExtendedField] where ArtifactID = @fieldArtifactId";
      var sqlParams = new List<SqlParameter>{new SqlParameter("@fieldArtifactId", SqlDbType.NVarChar) {Value = fieldArtifactId}};

			return workspaceDbContext.ExecuteSqlStatementAsScalar<int>(sqlQuery, sqlParams);
		}
	}
}
