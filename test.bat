msbuild /p:Configuration=Debug /p:Platform="Any CPU"||exit /b
bin\Debug\net7.0\test-syntax-tree.exe %*
