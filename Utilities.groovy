#!/usr/bin/env groovy


def build_solution(msbuildlocation, location) 
         {
            bat msbuildlocation location
        }
		
		
def run_nunit3_tests() {
        bat "powershell -Command ./RunNUnitTests.ps1"
        }


return this;

