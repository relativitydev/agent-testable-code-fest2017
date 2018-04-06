#!/usr/bin/env groovy


def build_solution() 
         {	
            bat "\"C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe\" RelativityAgent1\\RelativityAgent.sln"
        }
		
		
def run_nunit3_tests() {
        bat "powershell -Command ./RunNUnitTests.ps1"
        }


return this;


