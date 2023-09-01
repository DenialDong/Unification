chcp  65001
REM cd %~dp0..\Tools\Proto\protoc-24.0-win64\bin\
REM 编译.proto文件，转换为C#，输出到当前目录下
@echo compile proto to C#

REM @call ./Tools/Proto/protoc-24.0-win64/bin/protoc.exe  --csharp_out ./ test.proto

echo begin gen

echo %~dp0
echo %~dp0..\..\..\..\Protos


for %%i in (%~dp0\*.proto) do (  
	echo gen 
	echo %%i
	@call protoc.exe --proto_path=%~dp0  --csharp_out=%~dp0..\client\Unity\Assets\Scripts\hotfix\Main\Codes\Generate\Protos %%i
)



pause
