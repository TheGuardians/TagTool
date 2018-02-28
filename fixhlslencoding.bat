set root=%cd%
set path=%path%;%root%/Tools\dos2unix
cd %root%/TagTool/ShaderGenerator/shader_code
set current_dir=%cd%
dos2unix *
for /d %%i in ("%current_dir%\*") do ( cd "%%i" & dos2unix * )