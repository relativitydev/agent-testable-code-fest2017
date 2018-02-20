#!/usr/bin/env groovy


def build_solution(location) 
         {
            bat "\"C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe\" RelativityAgent1\\RelativityAgent.sln"
        }

return this;

