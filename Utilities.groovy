#!/usr/bin/env groovy


def build_solution(location) 
         {
            bat "\"C:/Program Files (x86)/MSBuild/14.0/bin/MSBuild.exe\" RelativityAgent1\\RelativityAgent.sln /p:Configuration=Debug /p:Platform=Any CPU"
        }

return this;

