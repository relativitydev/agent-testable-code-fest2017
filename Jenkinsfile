#!/usr/bin/env groovy

node('master') {
         def location = "RelativityAgent1\\RelativityAgent.sln"
		 
		bat 'echo before utilites'
		def	utilites = load ('S:/SourceCode/Fest2017/PowerShellScripts/Utilities.groovy')
		bat 'echo after utilites'
        
        stage('Stage Checkout') {
                checkout([$class: 'GitSCM', 
                branches: [[name: '*/master']], 
                doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], 
                userRemoteConfigs: [[url: 'https://github.com/relativitydev/agent-testable-code-fest2017.git']]])
				bat 'echo checkout complete'
        }
		
        stage('Stage build'){
                fileExists location
				bat 'S:/nuget.exe restore RelativityAgent1\\RelativityAgent.sln'
				bat 'echo nuget complete'
				bat 'echo build command starting'
			    utilites.build_solution()
			    bat 'echo build command done'
        }
		stage('Stage Test'){
		
					bat 'S:/NUnit-2.6.4/bin/nunit-console.exe RelativityAgent1\\AgentUnitTests\\bin\\Debug\\AgentUnitTests.dll'
		 
       }
}
	   
	   
