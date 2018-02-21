#!/usr/bin/env groovy

node('master') {
         def location = ".\\RelativityAgent1\\RelativityAgent.sln"
        
		
        stage('Stage Checkout') {
			echo 'Pulling...' + env.BRANCH_NAME
               shallow_clone_git_repo('master', 'https://github.com/relativitydev/agent-testable-code-fest2017.git')
        }
		
		bat 'echo before utilites'
		def	utilites = load ("Utilities.groovy")
		bat 'echo after utilites'
		
        stage('Stage build'){
                fileExists location
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
	   


	   
	   
