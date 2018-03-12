#!/usr/bin/env groovy

def create_nunit_app_config() {
    
    def smoke_template = readFile 'C:\Program Files (x86)\Jenkins\workspace\Build stuff 4\RelativityAgent1\AgentNunitIntegrationTest\app.config'
    
    smoke_template = smoke_template.replace('$AdminPassword', params.AdminPassword.toString())
    smoke_template = smoke_template.replace('$AdminUsername', params.AdminUsername)
    smoke_template = smoke_template.replace('$RESTServerAddress', params.RESTServerAddress)
    smoke_template = smoke_template.replace('$RSAPIServerAddress', params.RSAPIServerAddress)
    
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


