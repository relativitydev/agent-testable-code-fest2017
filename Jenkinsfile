#!/usr/bin/env groovy

node('master') {
         def location = ".\\RelativityAgent1\\RelativityAgent.sln"
		 def branch = env.BRANCH_NAME
		 
        
        stage('Stage Checkout') {
                checkout([$class: 'GitSCM', 
                branches: [[name: branch]], 
                doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], 
                userRemoteConfigs: [[url: 'https://github.com/relativitydev/agent-testable-code-fest2017.git']]])
				bat 'echo checkout complete'
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
	   
	   
