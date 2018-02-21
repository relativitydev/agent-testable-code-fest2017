#!/usr/bin/env groovy


def build_solution(msbuildlocation) 
         {	
			print msbuildlocation
            bat msbuildlocation "RelativityAgent1\\RelativityAgent.sln"
        }
		
		
def run_nunit3_tests() {
        bat "powershell -Command ./RunNUnitTests.ps1"
        }


return this;

