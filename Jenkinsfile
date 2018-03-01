#!/usr/bin/env groovy

node('master') {
         def location = ".\\RelativityAgent1\\RelativityAgent.sln"
		 def unittestdll = ".\\RelativityAgent1\\AgentUnitTests\\bin\\Debug\\AgentUnitTests.dll"
		 def integrtiontestdll = ".\\RelativityAgent1\\AgentNunitIntegrationTest\\bin\\Debug\\AgentNunitIntegrationTest.dll"
        
		
        stage('Stage Checkout') {
	            shallow_clone_git_repo('master', 'https://github.com/relativitydev/agent-testable-code-fest2017.git')
        }
		
		bat 'echo before utilites'
		def	utilites = load ("Utilities.groovy")
		bat 'echo after utilites'
		
        stage('Stage build'){
                fileExists location
				fileExists unittestdll
				fileExists integrtiontestdll
				bat 'S:/nuget.exe restore RelativityAgent1\\RelativityAgent.sln'
				bat 'echo nuget complete'
				bat 'echo build command starting'
				echo location
			    utilites.build_solution()
			    bat 'echo build command done'
        }
		stage('Stage Test'){
				utilites.run_nunit3_tests()
       }
}	   
	   def shallow_clone_git_repo(String branch, String gitURL){
	    checkout([$class: 'GitSCM', 
                branches: [[name: branch]], 
                doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], 
                userRemoteConfigs: [[url: gitURL]]])
				bat 'echo checkout complete'
	   }
	   


	   
	   
