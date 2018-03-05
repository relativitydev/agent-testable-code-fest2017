#!/usr/bin/env groovy

def create_nunit_app_config() {
    
    def smoke_template = readFile 'S:\SourceCode\Fest2017\agent-testable-code-fest2017\RelativityAgent1\AgentNunitIntegrationTest\app.config'
    
    smoke_template = smoke_template.replace('$AdminPassword', params.AdminPassword.toString())
    smoke_template = smoke_template.replace('$AdminUsername', params.AdminUsername)
    smoke_template = smoke_template.replace('$RESTServerAddress', params.RESTServerAddress)
    smoke_template = smoke_template.replace('$RSAPIServerAddress', params.RSAPIServerAddress)
    smoke_template = smoke_template.replace('$SQLServerAddress', params.SQLServerAddress)
    smoke_template = smoke_template.replace('$SQLUsername', params.SQLUsername)
    smoke_template = smoke_template.replace('$SQLPassword', params.SQLPassword.toString())
    smoke_template = smoke_template.replace('$DistSQLServerAddress', params.DistSQLServerAddress)
    smoke_template = smoke_template.replace('$DistSQLUsername', params.DistSQLUsername)
    smoke_template = smoke_template.replace('$DistSQLPassword', params.DistSQLPassword.toString())
    
    retry(3) {
        writeFile file: "C:/${params.ProjectName}/${params.AssemblyName}.dll.config", text: smoke_template
    }    
}

def build_solution() 
         {	
            bat "\"C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe\" RelativityAgent1\\RelativityAgent.sln"
        }
		
		
def run_nunit3_tests() {
        bat "powershell -Command ./RunNUnitTests.ps1"
        }


return this;

