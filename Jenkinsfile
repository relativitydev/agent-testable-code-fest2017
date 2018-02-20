#!/usr/bin/env groovy

node('master') {
         def location = "RelativityAgent1\\RelativityAgent.sln"
		 def utilities
		 def clone_git_repo(String branch, String gitURL)
		{
		checkout([$class: 'GitSCM',
		branches: [[name: '*/'branch]],
		doGenerateSubmoduleConfigurations: false,
		extensions: [], submoduleCfg: [],
		userRemoteConfigs: [[ url: gitURL]]
			]
			)
		}
 
        stage('Stage Checkout') {
				clone_git_repo('master', 'https://github.com/relativitydev/agent-testable-code-fest2017.git' )
        }
		
        stage('Stage build'){
                fileExists location
				bat 'S:/nuget.exe restore RelativityAgent1\\RelativityAgent.sln'
                bat "\"C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe\" RelativityAgent1\\RelativityAgent.sln"
        }
		stage('Stage Test'){
		
					bat 'S:/NUnit-2.6.4/bin/nunit-console.exe RelativityAgent1\\AgentUnitTests\\bin\\Debug\\AgentUnitTests.dll'
		 
       }
	   
}