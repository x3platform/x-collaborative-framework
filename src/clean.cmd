@for /d /r %%d in (.vs,bin,obj,ClientBin,Generated_Code,TestResults) do @if exist "%%d" ( rd /s /q "%%d" )

@del *.csproj.user /s/f/q
@del *.csproj.vspscc /s/f/q

@del *.docstates.suo /s/f/q
@del *.docstates.suo /s/f/q/a:h-s

@del StyleCop.Cache /s/f/q
@del StyleCop.Cache /s/f/q/a:h-s

cd ../bin/

del *.dll
del *.pdb
del *.xml

cd ../src/
